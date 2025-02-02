using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class CohootNestViewModel : ViewModel
{
    private int _kamuraCount;
    public int KamuraCount { get => _kamuraCount; set => SetValue(ref _kamuraCount, value); }

    private int _kamuraMaxCount;
    public int KamuraMaxCount { get => _kamuraMaxCount; set => SetValue(ref _kamuraMaxCount, value); }

    private int _elgadoCount;
    public int ElgadoCount { get => _elgadoCount; set => SetValue(ref _elgadoCount, value); }

    private int _elgadoMaxCount;
    public int ElgadoMaxCount { get => _elgadoMaxCount; set => SetValue(ref _elgadoMaxCount, value); }

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
}