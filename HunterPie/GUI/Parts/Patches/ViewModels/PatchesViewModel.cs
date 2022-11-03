using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Patches.ViewModels;

#nullable enable
public class PatchesViewModel : ViewModel
{
    private bool _isFetching;

    public bool IsFetching { get => _isFetching; set => SetValue(ref _isFetching, value); }
    public ObservableCollection<PatchViewModel> Patches { get; } = new();

    public async Task FetchPatchesAsync()
    {
        IsFetching = true;
        PoogieApiResult<PatchResponse[]>? patches = await PoogieApi.GetPatchNotes();
        IsFetching = false;

        if (patches is null || !patches.Success || patches.Response is null)
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
