﻿using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BfmeFoundationProject.WorkshopKit.Utils
{
    internal static class FileUtils
    {
        internal static async Task<string> Hash(string path)
        {
            return await Task.Run(() =>
            {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(path);
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            });
        }

        internal static T ReadJson<T>(string path, T def)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path)) ?? def;
            }
            catch { }
            return def;
        }

        internal static void WriteJson(string path, object obj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read)))
                    sw.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
            catch { }
        }

        internal static bool Contains(string path, string text)
        {
            try
            {
                return File.ReadAllText(path).Contains(text);
            }
            catch { }
            return false;
        }
    }
}
