#pragma once

namespace Core
{
    class IInitializable
    {
    public:
        virtual bool Init(uintptr_t* pointers) = 0;
        virtual ~IInitializable() = default;
    };
}
