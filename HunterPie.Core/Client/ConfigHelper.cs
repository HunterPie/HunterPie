using HunterPie.Core.Logger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace HunterPie.Core.Client
{
    public static class ConfigHelper
    {

        public static void WriteObject(string path, object obj)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                string serialized = JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings);
                ReadOnlySpan<byte> buffer = Encoding.UTF8.GetBytes(serialized);
                using (FileStream stream = File.OpenWrite(path))
                {
                    stream.SetLength(0);
                    stream.Write(buffer);
                }
            } catch (Exception err) { Log.Error(err.ToString()); }
        }

    }
}
