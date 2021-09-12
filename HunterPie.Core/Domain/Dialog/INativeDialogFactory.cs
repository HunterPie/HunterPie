using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Dialog
{
    public interface INativeDialogFactory
    {
        public INativeDialog CreateDialog(NativeDialogType type, string title, string description, NativeDialogButtons buttons);
    }
}
