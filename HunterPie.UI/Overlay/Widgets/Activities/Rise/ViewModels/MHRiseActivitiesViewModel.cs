using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MHRiseActivitiesViewModel : ViewModel, IActivitiesViewModel
{
    public CohootNestsViewModel CohootNests { get; }
    public MeowcenariesViewModel Meowcenaries { get; }
    public SubmarinesViewModel Submarines { get; }
    public TrainingDojoViewModel TrainingDojo { get; }

    public Observable<bool> IsCohootEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsCohootEnabled;

    public Observable<bool> IsSubmarinesEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsArgosyEnabled;

    public Observable<bool> IsMeowcenariesEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsMeowmastersEnabled;

    public Observable<bool> IsTrainingDojoEnabled { get; } =
        ClientConfig.Config.Rise.Overlay.ActivitiesWidget.IsTrainingDojoEnabled;

    public MHRiseActivitiesViewModel(
        CohootNestsViewModel cohootNests,
        MeowcenariesViewModel meowcenaries,
        SubmarinesViewModel submarines,
        TrainingDojoViewModel trainingDojo)
    {
        CohootNests = cohootNests;
        Meowcenaries = meowcenaries;
        Submarines = submarines;
        TrainingDojo = trainingDojo;
    }
}