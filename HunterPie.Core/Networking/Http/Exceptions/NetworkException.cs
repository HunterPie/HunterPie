using System;

namespace HunterPie.Core.Networking.Http.Exceptions;

public class NetworkException(string message) : Exception(message);