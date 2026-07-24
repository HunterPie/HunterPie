using HunterPie.Core.Architecture;

namespace HunterPie.Playground.Dogma.ViewModels;

internal class UnitViewModel(bool value) : Observable<bool>(value);