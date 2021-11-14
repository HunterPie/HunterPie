#include "Socket.h"

using namespace Connection;

bool Connection::NativeServer::initialize()
{
	if (this->b_isInitialized)
		return true;

	const wchar_t* addr = L"127.0.0.1";
	const unsigned short port = 0x6976;

	WSADATA wsaData;
	WORD ver = MAKEWORD(2, 2);

	if (WSAStartup(ver, &wsaData) != 0)
	{
		LOG("ERROR: Failed to start WSA - Error code: %d", WSAGetLastError());
		return false;
	}

	SOCKET listenSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (listenSocket == INVALID_SOCKET)
	{
		LOG("ERROR: Failed to create listenSocket - Error code: %d", WSAGetLastError());
		WSACleanup();
		return false;
	}

	SOCKADDR_IN addressServer;
	addressServer.sin_family = AF_INET;
	addressServer.sin_port = htons(port);
	InetPton(AF_INET, addr, &addressServer.sin_addr.s_addr);

	ZeroMemory(&addressServer.sin_zero, 8);

	if (bind())

	return false;
}

void Connection::NativeServer::ReceivePacket()
{}

void Connection::NativeServer::EnableHooks()
{}

void Connection::NativeServer::DisableHooks()
{}

void Connection::NativeServer::Disconnect()
{}
