using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Paginating.ViewModels;

public class PaginationItemViewModel : ViewModel
{
    public int Page { get; set => SetValue(ref field, value); }
    public bool IsActive { get; set => SetValue(ref field, value); }
    public bool IsVisible { get; set => SetValue(ref field, value); }
}