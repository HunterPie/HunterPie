#pragma once
#include "../../../pch.h"

namespace Core
{
    namespace Server
    {
        namespace Models
        {
            typedef uint32_t IPCMessageVersion;

            enum IPCMessageType
            {
                HUNTERPIE,
                HUNT_STATISTICS,
                UNKNOWN
            };

            struct IPCMessage
            {
                IPCMessageType type;
                IPCMessageVersion version;
            };
        }
    }
}