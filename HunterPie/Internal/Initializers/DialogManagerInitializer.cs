using HunterPie.Core.Domain.Dialog;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Dialog;

namespace HunterPie.Internal.Initializers
{
    internal class DialogManagerInitializer : IInitializer
    {
        public void Init()
        {
            INativeDialogFactory factory = new UIDialogFactory();
            _ = new DialogManager(factory);
        }
    }
}
