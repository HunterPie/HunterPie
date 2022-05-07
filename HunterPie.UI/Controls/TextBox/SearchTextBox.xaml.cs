using HunterPie.Core.Architecture;
using HunterPie.UI.Controls.TextBox.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TB = System.Windows.Controls.TextBox;

namespace HunterPie.UI.Controls.TextBox
{
    /// <summary>
    /// Interaction logic for SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        /// <summary>
        /// Event fired everytime the search string has changed
        /// </summary>
        public event EventHandler<SearchTextChangedEventArgs> OnSearchTextChanged;

        /// <summary>
        /// Event fired everytime the search button is clicked or when the user press enter while
        /// the SearchBox is focused
        /// </summary>
        public event EventHandler<SearchTextChangedEventArgs> OnSearch;

        public Observable<string> SearchText { get; } = "Search";

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

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TB tb)
            {
                if (tb.Text.Length == 0 && !IsPlaceholderVisible)
                {
                    IsPlaceholderVisible = true;
                    tb.Text = "Search";
                }
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TB tb)
            {
                if (tb.Text.Length > 0 && IsPlaceholderVisible)
                {
                    IsPlaceholderVisible = false;
                    tb.Text = string.Empty;
                }
            }
        }
    }
}
