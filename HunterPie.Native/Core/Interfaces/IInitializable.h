#pragma once
#include "../../pch.h"

namespace Core
{
    class IInitializable abstract
    {
    public:
        virtual bool Init(uintptr_t* pointers) = 0;
        virtual ~IInitializable() = default;
    };
}