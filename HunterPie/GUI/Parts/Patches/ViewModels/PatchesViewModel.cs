using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Patch;
using HunterPie.Integrations.Poogie.Patch.Models;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Patches.ViewModels;

public class PatchesViewModel : ViewModel
{
    private readonly PoogiePatchConnector _patchConnector = new();

    private bool _isFetching;

    public bool IsFetching { get => _isFetching; set => SetValue(ref _isFetching, value); }
    public ObservableCollection<PatchViewModel> Patches { get; } = new();

    public async Task FetchPatchesAsync()
    {
        IsFetching = true;
        PoogieResult<PatchResponse[]> patches = await _patchConnector.FindAll();
        IsFetching = false;

        if (patches.Response is null)
            return;

        await UIThread.InvokeAsync(() =>
        {
            PatchResponse[] patchNotes = patches.Response;

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
