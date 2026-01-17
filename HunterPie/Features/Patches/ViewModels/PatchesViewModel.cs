using HunterPie.Integrations.Poogie.Patch;
using HunterPie.Integrations.Poogie.Patch.Models;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.Features.Patches.ViewModels;

internal class PatchesViewModel(PoogiePatchConnector patchConnector) : ViewModel
{
    private readonly PoogiePatchConnector _patchConnector = patchConnector;

    public bool IsFetching { get; set => SetValue(ref field, value); }

    public ObservableCollection<PatchViewModel> Patches { get; } = new();

    public async Task FetchPatchesAsync()
    {
        IsFetching = true;
        (PatchResponse[]? patchNotes, _) = await _patchConnector.FindAll();
        IsFetching = false;

        if (patchNotes is null)
            return;

        await UIThread.InvokeAsync(() =>
        {
            foreach (PatchResponse note in patchNotes)
                Patches.Add(new PatchViewModel
                {
                    Title = note.Title,
                    Description = note.Description,
                    Banner = note.Banner,
                    Link = note.Link
                });
        });
    }
}