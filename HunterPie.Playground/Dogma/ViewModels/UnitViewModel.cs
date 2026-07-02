using HunterPie.Core.Architecture;

namespace HunterPie.Playground.Dogma.Views;

internal class UnitViewModel(bool value) : Observable<bool>(value);