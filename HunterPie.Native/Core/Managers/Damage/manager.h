#pragma once
#include <unordered_map>
#include "../../Entities/damage.h"

using namespace Core::Entities;

namespace HunterPie
{
	namespace Core
	{
		namespace Damage
		{
			struct HuntStatistics
			{
				EntityDamageData entities[10];
			};

			class DamageTrackManager
			{
			private:
				std::unordered_map<intptr_t, HuntStatistics*> m_Trackings;
				static DamageTrackManager* m_Instance;
				DamageTrackManager();
				DamageTrackManager operator=(DamageTrackManager const&);

			public:
				static DamageTrackManager* GetInstance();

				void UpdateDamage(EntityDamageData damageData);
				HuntStatistics* GetHuntStatisticsBy(intptr_t target);
				void DeleteBy(intptr_t target);
			};
		}
	}
}