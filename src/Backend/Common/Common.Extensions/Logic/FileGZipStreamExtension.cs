using System.IO.Compression;
using System.Text;

namespace Common.Extensions.Logic
{
    public static class FileGZipStreamExtension
    {
        public static async Task<byte[]> Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            var stream = new MemoryStream();

            using (GZipStream zip = new(stream, CompressionMode.Compress, true))
                await zip.WriteAsync(buffer);

            stream.Position = 0;

            byte[] compressed = new byte[stream.Length];

            await stream.ReadAsync(compressed);

            byte[] gzBuffer = new byte[compressed.Length + 4];

            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return gzBuffer;
        }

        public static async Task<string> DeCompress(byte[] bytes)
        {
            var stream = new MemoryStream();
            int length = BitConverter.ToInt32(bytes, 0);

            await stream.WriteAsync(bytes.AsMemory(4, bytes.Length - 4));

            byte[] buffer = new byte[length];

            stream.Position = 0;

            using (GZipStream zip = new(stream, CompressionMode.Decompress))
                await zip.ReadAsync(buffer);

            var text = Encoding.UTF8.GetString(buffer);

            return text;
        }

    }
}
