using HunterPie.Core.Extensions;
using HunterPie.Core.Settings.Types;
using System;
using System.Windows.Controls;
using ColorDialog = System.Windows.Forms.ColorDialog;
using DialogResult = System.Windows.Forms.DialogResult;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for ColorPicker.xaml
/// </summary>
public partial class ColorPicker : UserControl
{
    public ColorPicker()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
        using ColorDialog colorDialog = new();

        if (colorDialog.ShowDialog() != DialogResult.OK)
            return;

        if (DataContext is Color color)
            color.Value = colorDialog.Color.ToHexString();
    }
}
