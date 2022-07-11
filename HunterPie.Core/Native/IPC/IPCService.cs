using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC.Handlers;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace HunterPie.Core.Native.IPC
{
    public class IPCService
    {
        private const string IPC_DEFAULT_ADDRESS = "127.0.0.1";
        private const short IPC_DEFAULT_PORT = 22002;
        private const int IPC_DEFAULT_BUFFER_SIZE = 8192;

        private static IPCService? _instance;
        private TcpClient? _client;
        private NetworkStream? _stream => _client?.GetStream();
        private bool _isConnected;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

        public static IPCService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IPCService();

                return _instance;
            }
        }

        public static async Task<bool> Send<T>(T data) where T : struct
        {
            return await Instance.SendAsync(data);
        }

        internal static async Task<bool> Initialize()
        {
            return await Instance.Connect();
        }

        private async Task<bool> Connect()
        {
            if (_isConnected)
                return true;

            _client = new();

            try
            {
                await _client.ConnectAsync(IPC_DEFAULT_ADDRESS, IPC_DEFAULT_PORT);
            } catch (Exception err)
            {
                Log.Error(err.ToString());
                return false;
            }

            _isConnected = true;

            if (!_isConnected)
                return false;

            Log.Native("Connected to HunterPie Native IPC");

            Listen();
            
            return _isConnected;
        }

        private void HandleMessage(byte[] rawData)
        {
            IPCMessage message = MessageHelper.Deserialize<IPCMessage>(rawData);

            MessageHandlerManager.Dispatch(message.Type, rawData);
        }

        private void Listen()
        {
            Task.Factory.StartNew(async () =>
            {
                byte[] buffer = new byte[IPC_DEFAULT_BUFFER_SIZE];
             
                while (_isConnected)
                {
                    int dataSize = await _stream?.ReadAsync(buffer, 0, buffer.Length);
                    byte[] dataCopy = new byte[dataSize];

                    Buffer.BlockCopy(buffer, 0, dataCopy, 0, dataSize);
                    
                    HandleMessage(dataCopy);
                }
            });
        }

        private async Task<bool> SendAsync<T>(T message) where T : struct
        {
            byte[] raw = MessageHelper.Serialize(message);
            return await SendRawAsync(raw);
        }

        private async Task<bool> SendRawAsync(byte[] raw)
        {
            if (!_isConnected)
                return false;

            try
            {
                await _semaphoreSlim.WaitAsync();

                await _stream?.WriteAsync(raw, 0, raw.Length);
                return true;
            } catch (Exception err)
            {
                Log.Error(err.ToString());
            } finally
            {
                _semaphoreSlim.Release();
            }

            return false;
        }
    }
}
#nullable restore