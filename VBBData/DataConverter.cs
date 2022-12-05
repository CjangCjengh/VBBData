﻿using CoreSystem.AdvSystem;
using Ionic.Zlib;
using MsgPack.Serialization;
using System.IO;
using System.Web.Script.Serialization;

namespace VBBData
{
    internal static class DataConverter
    {
        public static string BytesToJson(string path)
        {
            ViewControlRoot result;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (DeflateStream deflateStream = new DeflateStream(fileStream, CompressionMode.Decompress))
                {
                    MessagePackSerializer<ViewControlRoot> messagePackSerializer = MessagePackSerializer.Get<ViewControlRoot>();
                    result = messagePackSerializer.Unpack(deflateStream);
                }
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        public static void JsonToBytes(string path)
        {
            ViewControlRoot viewControlRoot = new JavaScriptSerializer().Deserialize<ViewControlRoot>(File.ReadAllText(path));
            using (FileStream fileStream = new FileStream(Path.ChangeExtension(path, "bytes"), FileMode.Create, FileAccess.Write))
            {
                using (DeflateStream deflateStream = new DeflateStream(fileStream, CompressionMode.Compress))
                {
                    MessagePackSerializer<ViewControlRoot> messagePackSerializer = MessagePackSerializer.Get<ViewControlRoot>();
                    messagePackSerializer.Pack(deflateStream, viewControlRoot);
                }
            }
        }
    }
}
