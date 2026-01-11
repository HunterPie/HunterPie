using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Architecture.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Dialog;

/// <summary>
/// Interaction logic for DialogView.xaml
/// </summary>
public partial class DialogView : Window, INativeDialog, INotifyPropertyChanged
{
    public new NativeDialogResult DialogResult { get; private set; }
    public NativeDialogButtons Buttons
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.N(PropertyChanged);
            }
        }
    }

    public string DialogTitle
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.N(PropertyChanged);
            }
        }
    }

    public string Description
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.N(PropertyChanged);
            }
        }
    } = "This is a default dialog text";

    public DialogView()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Warn(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_WARN", buttons);

    public void Info(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_INFO", buttons);

    public void Error(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_ERROR", buttons);

    private void SetDialogInfo(string title, string description, string icon, NativeDialogButtons buttons)
    {
        DialogTitle = title;
        Description = description;
        Icon = FindResource(icon) as ImageSource;
        Buttons = buttons;
        _ = ShowDialog();
    }

    NativeDialogResult INativeDialog.DialogResult() => DialogResult;

    private void OnAccept(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Accept;
        Close();
    }

    private void OnReject(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Reject;
        Close();
    }

    private void OnCancel(object sender, EventArgs e)
    {
        DialogResult = NativeDialogResult.Cancel;
        Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (DialogResult == NativeDialogResult.NotFinished)
            DialogResult = NativeDialogResult.Cancel;
    }
}