#pragma once
#include "damage.h"
#include "../../../Core/Interfaces/IInitializable.h"

using namespace Core;

namespace Game
{
	namespace Damage
	{
		namespace Hook
		{
			MHREntityData* CalculateEntityDamage(
				intptr_t arg1,
				intptr_t target,
				intptr_t arg3,
				intptr_t arg4,
				intptr_t arg5,
				void* arg6
			);

			class DamageHooks : public IInitializable
			{
			public:
				virtual bool Init(intptr_t* pointers);
			};
		}
	}
}