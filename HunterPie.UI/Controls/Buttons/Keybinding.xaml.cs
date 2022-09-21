﻿using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for Keybinding.xaml
/// </summary>
public partial class Keybinding : UserControl
{
    public ObservableCollection<string> Keys { get; private set; } = new();

    public string HotKey
    {
        get => (string)GetValue(HotKeyProperty);
        set => SetValue(HotKeyProperty, value);
    }

    public static readonly DependencyProperty HotKeyProperty =
        DependencyProperty.Register("HotKey", typeof(string), typeof(Keybinding));

    public Keybinding()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Credits to this stackoverflow post I found: https://stackoverflow.com/questions/2136431/how-do-i-read-custom-keyboard-shortcut-from-user-in-wpf
        // The text box grabs all input.
        e.Handled = true;

        // Fetch the actual shortcut key.
        Key key = e.Key == Key.System ? e.SystemKey : e.Key;

        // Ignore modifier keys.
        if (key is Key.LeftShift or Key.RightShift
            or Key.LeftCtrl or Key.RightCtrl
            or Key.LeftAlt or Key.RightAlt
            or Key.LWin or Key.RWin)
        {
            return;
        }

        Keys.Clear();

        // Delete key removes the HotKey
        if (key == Key.Delete)
        {
            Keys.Add("None");
            SetValue(HotKeyProperty, "None");
            return;
        }

        // Build the shortcut key name.
        var shortcutText = new StringBuilder();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            Keys.Add("Ctrl");
            _ = shortcutText.Append("Ctrl+");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
        {
            Keys.Add("Shift");
            _ = shortcutText.Append("Shift+");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
        {
            Keys.Add("Alt");
            _ = shortcutText.Append("Alt+");
        }

        Keys.Add(key.ToString());
        _ = shortcutText.Append(key.ToString());
        SetValue(HotKeyProperty, shortcutText.ToString());
    }

    private void OnClick(object sender, EventArgs e) => Focus();

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Keys.Clear();

        foreach (string key in HotKey.Split("+"))
            Keys.Add(key);
    }
}
