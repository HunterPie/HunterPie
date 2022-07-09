// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>
#include <thread>
#include "libs/MinHook/MinHook.h"
#include "Games/Rise/Damage/hooks.h"
#include "Core/Server/IPCService.h"
#include "Core/Server/Handlers/MessageHandler.h"

void SetupHandlers()
{
    auto handlers = Core::Server::Handlers::MessageHandler::GetAvailableHandlers();

    for (auto handler : handlers)
    {
        handler->Initialize();
        LOG("Setup handler: %s", handler->GetName());
    }
}

void Initialize()
{
    intptr_t pointers[] = { 0x141149370 };

    MH_Initialize();
    
    auto damageHooks = Game::Damage::Hook::DamageHooks();
    bool success = damageHooks.Init(pointers);

    if (!success)
        LOG("[DEBUG] Something went wrong, failed to hook");
    
    auto ipcService = Core::Server::IPCService::GetInstance();
    
    SetupHandlers();
    
    MH_EnableHook(MH_ALL_HOOKS);

    success = ipcService->Initialize();
    
    if (!success)
        LOG("Something went wrong");
}

void LoadNativeDll()
{
    std::thread([]()
    {
        AllocConsole();
        FILE* stdoutForward;
        freopen_s(&stdoutForward, "CONOUT$", "w", stdout);

        std::cout << "Alloced console" << std::endl;

        Initialize();
    }).detach();
}

BOOL APIENTRY DllMain(
    HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved)
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