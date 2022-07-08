// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>
#include <thread>
#include "libs/MinHook/MinHook.h"

struct PlayerDamageUnk
{
    intptr_t unk0;
    int32_t unk1;
    int32_t unk2;
    int32_t unk3;
    int32_t unk4;
    int32_t partyIndex;
};

struct DamageReturn
{
    intptr_t unk0;
    int32_t unk1;
    int32_t unk2;
    int32_t unk3;
    float rawDamage;
    float elementalDamage;
    int unk4;
    float unk5;
    float unk6;
    float unk7;
    float unk8;
    float unk9;
    float unk10;
    float unk11;
    float unk12;
    uint8_t unk13[112];
    int attackerDamageType;
    int unk14;
    int unk15;
    int partyId;
};

struct PlayerStructureUnk
{
    intptr_t unk0;
    int32_t unk1;
    int32_t unk2;
    int32_t id; // maybe?
};

static DamageReturn*(*fnUnknown)(
    int64_t arg1,
    int64_t* arg2,
    PlayerDamageUnk* arg3,
    void* arg4,
    void* arg5,
    void* arg6
    ) = (DamageReturn*(*)(int64_t, int64_t*, PlayerDamageUnk*, void*, void*, void*))0x141149370;

static DamageReturn* (*fnUnknown2)(
    int64_t arg1,
    int64_t* arg2,
    void* arg3
    ) = (DamageReturn*(*)(int64_t, int64_t*, void*))0x141148640; // ends at 0x141148e34

void format_data(char data[200])
{
    for (int i = 0; i < 200; i++)
        printf_s("%02X ", data[i]);
    sizeof(DamageReturn);
    printf_s("\n");
}

DamageReturn* hook(int64_t arg1,
                   int64_t* arg2,
                   PlayerDamageUnk* arg3,
                   PlayerStructureUnk* arg4,
                   void* arg5,
                   void* arg6)
{
    printf_s("Unk1(%08X, %08X, %08X, %08X, %08X, %08X)", arg1, arg2, arg3, arg4, arg5, arg6);
    
    DamageReturn* rax = fnUnknown(arg1, arg2, arg3, arg4, arg5, arg6);

    printf_s("-> %08X\n", rax);
    int partIdx = ((int*)rax)[0x19];
    printf_s("Player %d -> %d damage | (Raw: %f, Elemental: %f)\n", rax->partyId, (int)(rax->rawDamage + rax->elementalDamage), rax->rawDamage, rax->elementalDamage);
    
    return rax;
}

DamageReturn* hook2(int64_t arg1,
            int64_t* arg2,
            PlayerStructureUnk* arg3)
{
    DamageReturn* rax = fnUnknown2(arg1, arg2, arg3);
    
    //printf_s("Unk2(%08X, %08X, %08X) -> %08X\n", arg1, arg2, arg3, rax);
    printf_s("dealt %d damage | (Raw: %f, Elemental: %f)\n", (int)(rax->rawDamage + rax->elementalDamage), rax->rawDamage, rax->elementalDamage);

    //format_data((char*)rax);
    return rax;
}

void Hook()
{
    MH_Initialize();
    MH_STATUS s = MH_CreateHook(fnUnknown, &hook, reinterpret_cast<LPVOID*>(&fnUnknown));
    //MH_STATUS e = MH_CreateHook(fnUnknown2, &hook2, reinterpret_cast<LPVOID*>(&fnUnknown2));
    if ((s) == MH_OK)
        std::cout << "Hook successful" << std::endl;
    else
        std::cout << s << std::endl;

    MH_EnableHook(MH_ALL_HOOKS);
}

void LoadNativeDll()
{
    std::thread(
        []()
        {
            AllocConsole();
            FILE* stdoutForward;
            freopen_s(&stdoutForward, "CONOUT$", "w", stdout);

            std::cout << "Alloced console" << std::endl;

            Hook();
        }
    ).detach();
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        LoadNativeDll();
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}