using HunterPie.Core.Architecture;
using HunterPie.UI.Controls.TextBox.Events;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using TB = System.Windows.Controls.TextBox;

namespace HunterPie.UI.Controls.TextBox;

/// <summary>
/// Interaction logic for SearchTextBox.xaml
/// </summary>
public partial class SearchTextBox : UserControl
{
    private const string SearchPlaceholder = "Search...";

    /// <summary>
    /// Event fired every time the search string has changed
    /// </summary>
    public event EventHandler<SearchTextChangedEventArgs> OnSearchTextChanged;

    /// <summary>
    /// Event fired every time the search button is clicked or when the user press enter while
    /// the SearchBox is focused
    /// </summary>
    public event EventHandler<SearchTextChangedEventArgs> OnSearch;

    public Observable<string> SearchText { get; } = SearchPlaceholder;

    private bool IsPlaceholderVisible { get; set; } = true;

    public SearchTextBox()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsPlaceholderVisible)
            OnSearchTextChanged?.Invoke(this, new(SearchText));
    }

    private void OnSearchClick(object sender, EventArgs e)
    {
        if (!IsPlaceholderVisible)
            OnSearch?.Invoke(this, new(SearchText));
    }
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.Enter))
            OnSearch?.Invoke(this, new(SearchText));
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is not TB tb)
            return;
        if (tb.Text.Length <= 0 || !IsPlaceholderVisible)
            return;

        IsPlaceholderVisible = false;
        tb.Text = string.Empty;
    }

    private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is not TB tb)
            return;

        if (tb.Text.Length != 0 || IsPlaceholderVisible)
            return;

        IsPlaceholderVisible = true;
        tb.Text = SearchPlaceholder;
    }
}
