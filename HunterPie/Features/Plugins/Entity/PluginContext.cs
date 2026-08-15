using HunterPie.Core.Plugins.DI;
using HunterPie.Core.Plugins.Entity;
using System.Runtime.Loader;

namespace HunterPie.Features.Plugins.Entity;

internal record struct PluginContext(
    Plugin Plugin,
    IPluginModule Module,
    AssemblyLoadContext Context
);