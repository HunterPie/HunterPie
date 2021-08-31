using System;
using System.Collections.Generic;

namespace HunterPie.Core.Domain
{
    
    public delegate void Middleware<T>(ref T dto) where T : struct;

    /// <summary>
    /// Implementation for a scannable entity, handles the scan and middlewares internally
    /// </summary>
    internal class ScannableEntity
    {

        private readonly Dictionary<Type, HashSet<Delegate>> middlewares = new Dictionary<Type, HashSet<Delegate>>();
        private readonly List<Action> scanners = new List<Action>();
        
        /// <summary>
        /// Calls each 
        /// </summary>
        protected void Scan()
        {
            foreach (Action scanner in scanners)
                scanner();
        }

        /// <summary>
        /// Adds a new scanning action to the scanners list, actions added by this
        /// will be called in order by the <seealso cref="Scan"/> method
        /// </summary>
        /// <param name="type">Type of DTO this action will handle</param>
        /// <param name="scanner">Action to handle this DTO</param>
        /// <returns>True if scanner was added successfully, false otherwise</returns>
        protected bool AddScanner(Type type, Action scanner)
        {
            scanners.Add(scanner);

            if (!middlewares.ContainsKey(type))
                middlewares.Add(type, new HashSet<Delegate>());

            return true;
        }

        /// <summary>
        /// Calls the middlewares
        /// </summary>
        /// <typeparam name="T">Type of the DTO</typeparam>
        /// <param name="data">DTO to pass to the middlewares</param>
        protected void Next<T>(ref T data) where T : struct
        {
            foreach (var mid in middlewares[data.GetType()])
            {
                Middleware<T> function = (Middleware<T>)mid;
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
            if (!middlewares.ContainsKey(type))
                return false;

            middlewares[type].Add(middleware);

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
            if (!middlewares.ContainsKey(type))
                return false;

            middlewares[type].Remove(middleware);
            
            return true;
        }

    }
}
