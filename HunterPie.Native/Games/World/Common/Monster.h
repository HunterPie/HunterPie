#pragma once

namespace Games::World::Common
{
    struct Monster
    {
        [[maybe_unused]] uint8_t padding1[0x12280];
        uint32_t id;
    };
}
