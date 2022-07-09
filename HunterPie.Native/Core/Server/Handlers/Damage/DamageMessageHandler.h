#pragma once
#include "../../../Entities/damage.h"
#include "../MessageHandler.h"
#include "../../Models/Messages.h"
#include "../../IPCService.h"

namespace Core
{
    namespace Server
    {
        namespace Models
        {
            struct RequestHuntStatisticsMessage : IPCMessage
            {
                intptr_t target;
            };

            struct ResponseHuntStatisticsMessage : IPCMessage
            {
                Entities::EntityDamageData entities[10];
            };
        }

        namespace Handlers
        {
            class DamageMessageHandler : public MessageHandler
            {
            public:
                virtual void Initialize();
                virtual const char* GetName();
            };
        }
    }
}