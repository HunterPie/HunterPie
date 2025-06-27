using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class MHWildsActivitiesViewModel : ViewModel, IActivitiesViewModel
{
    public MaterialRetrievalViewModel MaterialRetrieval { get; set; }

    public Observable<bool> IsMaterialRetrievalEnabled { get; } =
        ClientConfig.Config.Wilds.Overlay.ActivitiesWidget.IsMaterialRetrievalEnabled;

    public MHWildsActivitiesViewModel(
        MaterialRetrievalViewModel materialRetrieval)
    {
        MaterialRetrieval = materialRetrieval;
    }
}