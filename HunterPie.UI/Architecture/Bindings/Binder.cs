using HunterPie.Core.Architecture;
using System;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Bindings;

public static class Binder
{
    public static Binding Create(object property)
    {
        Type type = property.GetType();
        Type supportedType = typeof(Observable<>);

        if (supportedType.Name != type.Name || supportedType.Namespace != type.Namespace)
            throw new NotImplementedException($"Only observables are supported");

        return new Binding(nameof(Observable<object>.Value)) { Source = property };
    }
}