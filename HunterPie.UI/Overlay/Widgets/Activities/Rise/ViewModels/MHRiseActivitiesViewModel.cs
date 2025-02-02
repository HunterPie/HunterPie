using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MHRiseActivitiesViewModel : ViewModel, IActivitiesViewModel
{
    public CohootNestViewModel CohootNest { get; }
    public MeowcenariesViewModel Meowcenaries { get; }
    public SubmarinesViewModel Submarines { get; }
    public TrainingDojoViewModel TrainingDojo { get; }

    public MHRiseActivitiesViewModel(
        CohootNestViewModel cohootNest,
        MeowcenariesViewModel meowcenaries,
        SubmarinesViewModel submarines,
        TrainingDojoViewModel trainingDojo)
    {
        CohootNest = cohootNest;
        Meowcenaries = meowcenaries;
        Submarines = submarines;
        TrainingDojo = trainingDojo;
    }
}