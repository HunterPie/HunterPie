using System;
using System.Collections.Generic;

namespace HunterPie.Features.Statistics.Models;

internal record HuntStatisticsModel(
    List<PartyMemberModel> Players,
    List<MonsterModel> Monsters,
    DateTime StartedAt,
    DateTime FinishedAt,
    string Hash
);
