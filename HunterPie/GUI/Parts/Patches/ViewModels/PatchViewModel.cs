using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Patches.ViewModels;

public class PatchViewModel : ViewModel
{
    public string Link { get; init; }
    public string Banner { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
