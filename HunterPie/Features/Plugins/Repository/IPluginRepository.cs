using HunterPie.Features.Plugins.Entity;
using System.Collections.Generic;

namespace HunterPie.Features.Plugins.Repository;

internal interface IPluginRepository
{
    public IReadOnlyList<PluginContext> FindAll();
}