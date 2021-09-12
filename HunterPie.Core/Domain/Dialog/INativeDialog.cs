using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Dialog
{
    public interface INativeDialog
    {
        public void Warn(string title, string description, NativeDialogButtons buttons);
        public void Info(string title, string description, NativeDialogButtons buttons);
        public void Error(string title, string description, NativeDialogButtons buttons);
        public NativeDialogResult DialogResult();
    }
}
