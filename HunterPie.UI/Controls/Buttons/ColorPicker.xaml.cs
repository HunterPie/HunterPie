using System;
using ColorDialog = System.Windows.Forms.ColorDialog;
using DialogResult = System.Windows.Forms.DialogResult;
using System.Windows.Controls;
using HunterPie.Core.Settings.Types;
using HunterPie.Core.Extensions;

namespace HunterPie.UI.Controls.Buttons
{
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
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (DataContext is Color color)
                {
                    color.Value = colorDialog.Color.ToHexString();
                }
            }
        }
    }
}
