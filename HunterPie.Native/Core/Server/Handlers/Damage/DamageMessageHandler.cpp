#include "DamageMessageHandler.h"
#include "../../../Managers/Damage/manager.h"
#include <iterator>

using namespace Core::Server;
using namespace Core::Server::Models;
using namespace Core::Server::Handlers;
using namespace Core::Entities;

void OnRequestHuntingStatistics(RequestHuntStatisticsMessage* message)
{
    IPCService* ipcService = IPCService::GetInstance();
    auto damageTracker = HunterPie::Core::Damage::DamageTrackManager::GetInstance();
    
    auto statistics = damageTracker->GetHuntStatisticsBy(message->target);
    
    ResponseHuntStatisticsMessage response{};
    response.type = HUNT_STATISTICS;
    response.version = 1;

    if (statistics == nullptr)
    {    
        LOG("No monsters found with address %08X", message->target);
        ipcService->SendIPCMessage(&response, sizeof(ResponseHuntStatisticsMessage));
        return;
    }

    for (int i = 0; i < std::size(response.entities); i++)
    {
        std::memcpy(&response.entities[i], &statistics->entities[i], sizeof(EntityDamageData));
    }

    ipcService->SendIPCMessage(&response, sizeof(ResponseHuntStatisticsMessage));
}

void DamageMessageHandler::Initialize()
{
    IPCService::GetInstance()->RegisterMessageHandler(HUNT_STATISTICS, &OnRequestHuntingStatistics);
}

const char* DamageMessageHandler::GetName()
{
    return NAMEOF(DamageMessageHandler);
}