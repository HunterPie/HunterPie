#include "../../../pch.h"
#include <iterator>
#include "manager.h"

using namespace HunterPie::Core::Damage;

DamageTrackManager* DamageTrackManager::m_Instance;
DamageTrackManager::DamageTrackManager() {}
DamageTrackManager DamageTrackManager::operator=(DamageTrackManager const &)
{
    return *this;
}

DamageTrackManager* DamageTrackManager::GetInstance()
{
    if (m_Instance == nullptr)
        m_Instance = new DamageTrackManager();

    return m_Instance;
}

void DamageTrackManager::UpdateDamage(EntityDamageData damageData)
{
    intptr_t& target = damageData.target;

    if (m_Trackings.find(target) == m_Trackings.end())
        m_Trackings.insert({ target, new HuntStatistics() });

    HuntStatistics*& statistics = m_Trackings.at(target);

    if (damageData.source.index >= std::size(statistics->entities) || damageData.source.index < 0)
        return;

    EntityDamageData& entityData = statistics->entities[damageData.source.index];

    entityData.target = damageData.target;
    memcpy(&entityData.source, &damageData.source, sizeof(Entity));

    entityData.rawDamage += damageData.rawDamage;
    entityData.elementalDamage += damageData.elementalDamage;

    LOG("[DEBUG] Entity %d -> %08X : %f damage", entityData.source.index, target, entityData.rawDamage + entityData.elementalDamage);
}

HuntStatistics* DamageTrackManager::GetHuntStatisticsBy(intptr_t target)
{
    if (m_Trackings.find(target) == m_Trackings.end())
        return nullptr;

    return m_Trackings.at(target);
}

void DamageTrackManager::DeleteBy(intptr_t target)
{
    if (m_Trackings.find(target) == m_Trackings.end())
        return;

    LOG("Clearing monster at %08X", target);

    delete m_Trackings.at(target);
    m_Trackings.erase(target);
}

void DamageTrackManager::ClearAllExcept(intptr_t* targets, size_t length)
{
    for (auto track : m_Trackings)
    {
        intptr_t* size = targets + length * sizeof(intptr_t);
        bool found = std::find(targets, size, track.first) != size;

        if (found)
            DeleteBy(track.first);
    }
}