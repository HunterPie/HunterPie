using System;

namespace HunterPie.UI.Controls.TextBox.Events;

public class SearchTextChangedEventArgs(string text) : EventArgs
{
    public string Text { get; } = text;
}