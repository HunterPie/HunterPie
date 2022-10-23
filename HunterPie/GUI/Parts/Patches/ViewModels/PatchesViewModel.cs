using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Patches.ViewModels;

public class PatchesViewModel : ViewModel
{
    public ObservableCollection<PatchViewModel> Patches { get; } = new()
    {
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.5.0-banner.png",
            Title = "HunterPie - Patch Notes [v2.5.0]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.4.0-banner.png",
            Title = "HunterPie - Patch Notes [v2.4.0]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.3.0-banner.png",
            Title = "HunterPie - Patch Notes [v2.3.0]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.2.0-banner.png",
            Title = "HunterPie - Patch Notes [v2.2.0]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.1.0-banner.png",
            Title = "HunterPie - Patch Notes [v2.1.0]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.0.23-banner.png",
            Title = "HunterPie - Patch Notes [v2.0.23]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.0.22-banner.png",
            Title = "HunterPie - Patch Notes [v2.0.22]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.0.21-banner.png",
            Title = "HunterPie - Patch Notes [v2.0.21]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
        new PatchViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.0.20-banner.png",
            Title = "HunterPie - Patch Notes [v2.0.20]",
            Description = "This is a very long poggers description just to test the patch UI components very poggers LETS GOOOOOOOOOOOOO",
        },
    };
}
