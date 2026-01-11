using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class MHWildsActivitiesViewModel(
    MaterialRetrievalViewModel materialRetrieval,
    SupportShipViewModel supportShip,
    IngredientsCenterViewModel ingredientsCenter) : ViewModel, IActivitiesViewModel
{
    public MaterialRetrievalViewModel MaterialRetrieval { get; set; } = materialRetrieval;

    public SupportShipViewModel SupportShip { get; set; } = supportShip;

    public IngredientsCenterViewModel IngredientsCenter { get; set; } = ingredientsCenter;

    public Observable<bool> IsMaterialRetrievalEnabled { get; } =
        ClientConfig.Config.Wilds.Overlay.ActivitiesWidget.IsMaterialRetrievalEnabled;

    public Observable<bool> IsSupportShipEnabled { get; } =
        ClientConfig.Config.Wilds.Overlay.ActivitiesWidget.IsArgosyEnabled;

    public Observable<bool> IsIngredientsCenterEnabled { get; } =
        ClientConfig.Config.Wilds.Overlay.ActivitiesWidget.IsIngredientsCenterEnabled;
}