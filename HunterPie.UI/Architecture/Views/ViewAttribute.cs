using System;

namespace HunterPie.UI.Architecture.Views;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ViewAttribute<T>() : Attribute where T : ViewModel;