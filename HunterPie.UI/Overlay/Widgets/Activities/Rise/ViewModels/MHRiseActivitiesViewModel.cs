using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MHRiseActivitiesViewModel(
    CohootNestsViewModel cohootNests,
    MeowcenariesViewModel meowcenaries,
    SubmarinesViewModel submarines,
    TrainingDojoViewModel trainingDojo) : ViewModel, IActivitiesViewModel
{
    public CohootNestsViewModel CohootNests { get; } = cohootNests;
    public MeowcenariesViewModel Meowcenaries { get; } = meowcenaries;
    public SubmarinesViewModel Submarines { get; } = submarines;
    public TrainingDojoViewModel TrainingDojo { get; } = trainingDojo;

    public Observable<bool> IsCohootEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsCohootEnabled;

    public Observable<bool> IsSubmarinesEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsArgosyEnabled;

    public Observable<bool> IsMeowcenariesEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsMeowmastersEnabled;

    public Observable<bool> IsTrainingDojoEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsTrainingDojoEnabled;
}