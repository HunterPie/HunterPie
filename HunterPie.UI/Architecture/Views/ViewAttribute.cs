using System;

namespace HunterPie.UI.Architecture.Views;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
#pragma warning disable CS9113 // Parameter is unread.
public class ViewAttribute<T>(bool isNullable = false) : Attribute where T : ViewModel;
#pragma warning restore CS9113 // Parameter is unread.