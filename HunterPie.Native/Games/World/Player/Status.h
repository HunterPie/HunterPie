#pragma once
#include "Core/Interfaces/IInitializable.h"

namespace Games::World::Player::Status {
    typedef float (*GetCurrentTrueRawDamageFn)(void* equipment, void* unk0, int unk1, char unk2);

    float GetCurrentTrueRawDamage(void* equipment, void* unk0, int unk1, char unk2);

    class Equipment : Core::IInitializable {
    public:
        HRESULT Init(uintptr_t* pointers) override;
    };
}