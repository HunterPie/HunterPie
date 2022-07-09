#include "MessageHandler.h"
#include "Damage/DamageMessageHandler.h"

using namespace Core::Server::Handlers;

static std::vector<MessageHandler*> g_Handlers{
    {new DamageMessageHandler() },
};

std::vector<MessageHandler*> Core::Server::Handlers::MessageHandler::GetAvailableHandlers()
{
    return g_Handlers;
}
