using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Settings.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Custom
{
    /// <summary>
    /// Interaction logic for AbnormalityTrayList.xaml
    /// </summary>
    public partial class AbnormalityTrayList : UserControl, INotifyPropertyChanged
    {

        private int _selectedIndex;

        public int SelectedIndex { get => _selectedIndex; set { SetValue(ref _selectedIndex, value); } }

        private AbnormalityTrays ViewModel => (AbnormalityTrays)DataContext;

        public AbnormalityTrayList()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return;

            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnAddTrayClick(object sender, EventArgs e)
        {
            ViewModel.Trays.Add(new AbnormalityWidgetConfig() { Name = $"Abnormality Tray {ViewModel.Trays.Count + 1}" });

            SelectedIndex = Math.Max(0, ViewModel.Trays.Count - 1);
        }

        private void OnRemoveTrayClick(object sender, EventArgs e)
        {
            AbnormalityWidgetConfig config = ViewModel.Trays[SelectedIndex];

            NativeDialogResult confirmation = DialogManager.Warn(
                "Confirmation", 
                $"Are you sure you want to delete {(string)config.Name}?", 
                NativeDialogButtons.Accept | NativeDialogButtons.Cancel
            );

            if (confirmation != NativeDialogResult.Accept)
                return;

            ViewModel.Trays.RemoveAt(SelectedIndex);

            SelectedIndex = Math.Max(0, SelectedIndex - 1);
        }

        private void OnOpenConfigClick(object sender, EventArgs e)
        {
            AbnormalityWidgetConfig vm = ViewModel.Trays[SelectedIndex];
            var window = new AbnormalityWidgetConfigWindow(vm);
            window.ShowDialog();
        }
    }
}
