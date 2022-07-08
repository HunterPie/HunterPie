#pragma once
#include "../../pch.h"

namespace Core
{
    class IInitializable
    {
    public:
        virtual bool Init(intptr_t* pointers) { return false; };
    };
}