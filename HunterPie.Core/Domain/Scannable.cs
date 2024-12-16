using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HunterPie.Core.Domain;

public delegate void Middleware<T>(ref T dto) where T : struct;

/// <summary>
/// Implementation for a scannable entity, handles the scan and middlewares internally
/// </summary>
public abstract class Scannable
{
    protected readonly IProcessManager Process;
    protected IMemory Memory => Process.Memory;
    private readonly Dictionary<Type, HashSet<Delegate>> _middlewares = new();
    private readonly List<Delegate> _scanners = new();
    private readonly Dictionary<Delegate, int> _troublesomeScannables = new();

    protected Scannable(IProcessManager process)
    {
        Process = process;

        AppendScannableMethods();
    }

    protected Scannable()
    {
        AppendScannableMethods();
    }

    private void AppendScannableMethods()
    {
        Type self = GetType();
        MethodInfo[] methods = self.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        foreach (MethodInfo method in methods)
        {
            object[] scannableAttributes = method.GetCustomAttributes(typeof(ScannableMethod), true);

            if (scannableAttributes.Length <= 0)
                continue;

            var attributes = (ScannableMethod)scannableAttributes.First();

            var func = (Action)Delegate.CreateDelegate(typeof(Action), this, method.Name);

            _ = Add(attributes.DtoType, func);
        }
    }

    /// <summary>
    /// Calls each scanning function added to the scanners list
    /// </summary>
    internal void Scan()
    {
        Delegate[] readOnlyScanners = _scanners.ToArray();

        foreach (Delegate scanner in readOnlyScanners)
            try
            {
                _ = scanner.DynamicInvoke();
            }
            catch (Exception err)
            {
                if (!_troublesomeScannables.ContainsKey(scanner))
                    _troublesomeScannables.Add(scanner, 0);

                _troublesomeScannables[scanner]++;

                if (_troublesomeScannables[scanner] >= 3)
                {
                    Log.Warn($"Scanner: {scanner.Method.Name} had multiple exceptions. Disabling scanner for now;\n{err}");

                    _scanners.Remove(scanner);

                    _troublesomeScannables.Remove(scanner);
                }
            }
    }

    /// <summary>
    /// Adds a new scanning action to the scanners list, actions added by this
    /// will be called in order by the <seealso cref="Scan"/> method
    /// </summary>
    /// <param name="type">Type of DTO this action will handle</param>
    /// <param name="scanner">Action to handle this DTO</param>
    /// <returns>True if scanner was added successfully, false otherwise</returns>
    protected bool Add<T>(Func<T> scanner)
    {
        Type type = typeof(T);

        _scanners.Add(scanner);

        if (!_middlewares.ContainsKey(type))
            _middlewares.Add(type, new HashSet<Delegate>());

        return true;
    }

    protected bool Add(Type type, Action scanner)
    {
        _scanners.Add(scanner);

        if (type is null)
            return true;

        if (!_middlewares.ContainsKey(type))
            _middlewares.Add(type, new());

        return true;
    }

    /// <summary>
    /// Calls the middlewares
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="data">DTO to pass to the middlewares</param>
    protected void Next<T>(ref T data) where T : struct
    {
        foreach (Delegate mid in _middlewares[data.GetType()])
        {
            var function = (Middleware<T>)mid;
            function(ref data);
        }
    }

    /// <summary>
    /// Adds a middleware for the type <typeparamref name="T"/> that will be called
    /// when a scanning function with that type runs.
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="middleware">Function to be called</param>
    /// <returns>true if the middleware was added successfully, false otherwise</returns>
    public bool MiddlewareFor<T>(Middleware<T> middleware) where T : struct
    {
        Type type = typeof(T);
        if (!_middlewares.ContainsKey(type))
            return false;

        _ = _middlewares[type].Add(middleware);

        return true;
    }

    /// <summary>
    /// Removes a middleware of the type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="middleware">Function to remove</param>
    /// <returns>true if the middleware was removed successfully, false otherwise</returns>
    public bool RemoveMiddleware<T>(Middleware<T> middleware) where T : struct
    {
        Type type = typeof(T);
        if (!_middlewares.ContainsKey(type))
            return false;

        _ = _middlewares[type].Remove(middleware);

        return true;
    }
}