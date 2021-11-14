#define DEFAULT_BUFFER_SIZE 16384
#define WIN32_LEAN_NO_MEAN
#pragma once
#include <WinSock2.h>
#include <mutex>
#include <queue>
#include <WS2tcpip.h>
#include <vector>
#include "../Logger/Logger.h"

namespace Connection
{
	class NativeServer
	{
	public:
		static NativeServer* instance;
		bool initialize();

	private:
		NativeServer();
		NativeServer(NativeServer const&);
		NativeServer& operator=(NativeServer const&);

		void ReceivePacket();

		void EnableHooks();
		void DisableHooks();
		void Disconnect();

		bool b_isInitialized = false;
		bool b_hooksEnabled = false;

		static NativeServer* _instance;
		// Sockets pool
		std::vector<SOCKET> pool;
	};
}