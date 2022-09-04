#include "pch.h"
#include "Hooks.h"
#include "Core/Debug/logger.h"
#include "Core/Utils/addresses.h"
#include "libs/MinHook/MinHook.h"

namespace Games::World::Damage::Hooks
{
    DealDamageFunction ogDealDamage = nullptr;
    
    void DealDamage(
        Common::Monster* target,
        int damage,
        void* position,
        BOOL isTenderized,
        BOOL isCrit,
        int unk0,
        int unk1,
        char unk2,
        int attackId)
    {
        if (!ogDealDamage) {
            LOG("Unexpected call before hook initialization.");
            return;
        }
        ogDealDamage(target, damage, position, isTenderized, isCrit, unk0, unk1, unk2, attackId);
        LOG("Hit monster id: %d; damage: %d; atkId: %d", target->id, damage, attackId);
    }

    bool DamageHooks::Init(uintptr_t* pointers)
    {
        ogDealDamage = nullptr;
        auto originalDealDamagePtr = reinterpret_cast<DealDamageFunction>(pointers[FUN_CALCULATE_ENTITY_DAMAGE]);

        LOG("Hook DealDamage function at %p", originalDealDamagePtr);
        auto status = MH_CreateHook(
            originalDealDamagePtr,
            static_cast<DealDamageFunction>(&DealDamage),       // cast for type-check
            reinterpret_cast<LPVOID*>(&ogDealDamage)
        );

        LOG("Hook DealDamage function status: %s", MH_StatusToString(status));
        LOG("DealDamage: %p", &DealDamage);
        LOG("&ogDealDamage: %p,ogDealDamage: %p", &ogDealDamage, ogDealDamage);

        return status == MH_OK;
    }
}
