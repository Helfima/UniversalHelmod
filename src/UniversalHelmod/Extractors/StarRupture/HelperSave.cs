using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UniversalHelmod.Classes;

namespace UniversalHelmod.Extractors.StarRupture
{
    /// <summary>
    /// @see https://gist.github.com/oldguygamingtime/3c5dd09cec3d4b72c643bea5c563a13a(
    /// </summary>
    public class HelperSave
    {
        // Zstandard magic number: 0xFD2FB528 (little-endian)
        private static bool IsZstd(byte[] data)
        {
            return data.Length >= 4
                && data[0] == 0x28
                && data[1] == 0xB5
                && data[2] == 0x2F
                && data[3] == 0xFD;
        }

        // -----------------------------------------------------------------------
        // Decompress: détecte automatiquement zstd ou zlib/deflate.
        // -----------------------------------------------------------------------
        public static byte[] Decompress(byte[] data)
        {
            // Read the first 4 bytes as the JSON size (little-endian)
            int jsonSize = BitConverter.ToInt32(data, 0);
            Logger.Info($"JSON size from header: {jsonSize} bytes", "SaveFileService");

            // Extract the compressed data (everything after the first 4 bytes)
            byte[] compressedData = new byte[data.Length - 4];
            Array.Copy(data, 4, compressedData, 0, compressedData.Length);

            // Decompress the zlib data (raw deflate, no header)
            return DecompressZlibRaw(compressedData);
        }

        private static byte[] DecompressZlib(byte[] data)
        {
            // Skip the 2-byte zlib header (CMF + FLG).
            var skip = 4;
            using (var input = new MemoryStream(data, skip, data.Length - skip))
            using (var deflate = new DeflateStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                deflate.CopyTo(output);
                return output.ToArray();
            }
        }
        /// <summary>
        /// Decompresses zlib raw data (deflate without header)
        /// </summary>
        public static byte[] DecompressZlibRaw(byte[] compressedData)
        {
            // Check if this is zlib-wrapped data (has 2-byte header)
            // Zlib header typically starts with 0x78 (120 decimal)
            byte[] deflateData = compressedData;

            if (compressedData.Length >= 2 && compressedData[0] == 0x78)
            {
                // This is zlib-wrapped data, strip the 2-byte header and 4-byte checksum trailer
                deflateData = new byte[compressedData.Length - 6];
                Array.Copy(compressedData, 2, deflateData, 0, deflateData.Length);
            }

            using var compressedStream = new MemoryStream(deflateData);
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();

            deflateStream.CopyTo(decompressedStream);
            byte[] decompressedBytes = decompressedStream.ToArray();

            return decompressedBytes;
        }
        // -----------------------------------------------------------------------
        // Compress: deflates the data, prepends the standard zlib header (0x78
        // 0x9C = deflate, default compression), and appends a big-endian
        // Adler-32 checksum over the *original* (uncompressed) data.
        // -----------------------------------------------------------------------
        public static byte[] Compress(byte[] data)
        {
            // Deflate the raw bytes.
            byte[] deflated;
            using (var output = new MemoryStream())
            {
                using (var deflate = new DeflateStream(output, CompressionLevel.Optimal, leaveOpen: true))
                {
                    deflate.Write(data, 0, data.Length);
                }
                deflated = output.ToArray();
            }
            // Compute Adler-32 over the original uncompressed data.
            uint adler = ComputeAdler32(data);
            // Build final stream: 2-byte header + deflated payload + 4-byte checksum.
            byte[] result = new byte[2 + deflated.Length + 4];
            result[0] = 0x78; // CMF: deflate method, window size 32k
            result[1] = 0x9C; // FLG: default compression, no dict; (0x789C % 31 == 0) ✓
            Array.Copy(deflated, 0, result, 2, deflated.Length);
            // Adler-32 is big-endian.
            int tail = 2 + deflated.Length;
            result[tail + 0] = (byte)(adler >> 24);
            result[tail + 1] = (byte)(adler >> 16);
            result[tail + 2] = (byte)(adler >> 8);
            result[tail + 3] = (byte)(adler);
            return result;
        }
        // Adler-32 per RFC 1950.
        private static uint ComputeAdler32(byte[] data)
        {
            const uint MOD_ADLER = 65521;
            uint s1 = 1, s2 = 0;
            foreach (byte b in data)
            {
                s1 = (s1 + b) % MOD_ADLER;
                s2 = (s2 + s1) % MOD_ADLER;
            }
            return (s2 << 16) | s1;
        }

        public static string GetCompressionInfo(byte[] data)
        {
            if (data.Length < 16) return "Fichier trop petit";

            var sb = new StringBuilder();
            sb.AppendLine($"Taille: {data.Length} octets");
            sb.Append("Header (16 premiers octets): ");
            for (int i = 0; i < Math.Min(16, data.Length); i++)
            {
                sb.Append($"0x{data[i]:X2} ");
            }
            sb.AppendLine();

            // Détection des formats connus
            if (data[0] == 0x28 && data[1] == 0xB5 && data[2] == 0x2F && data[3] == 0xFD)
                sb.AppendLine("Format: Zstandard (zstd)");
            else if (data[0] == 0x78 && (data[1] == 0x01 || data[1] == 0x5E || data[1] == 0x9C || data[1] == 0xDA))
                sb.AppendLine("Format: zlib");
            else if (data[0] == 0x1F && data[1] == 0x8B)
                sb.AppendLine("Format: gzip");
            else if (data[0] == 0x50 && data[1] == 0x4B)
                sb.AppendLine("Format: ZIP");
            else if (data[0] == 0xFD && data[1] == 0x37 && data[2] == 0x7A && data[3] == 0x58)
                sb.AppendLine("Format: xz");
            else if (data[0] == 0x04 && data[1] == 0x22 && data[2] == 0x4D && data[3] == 0x18)
                sb.AppendLine("Format: LZ4");
            else if (data[0] == 0x7B)
                sb.AppendLine("Format: JSON non compressé (commence par '{')");
            else
                sb.AppendLine("Format: inconnu");

            return sb.ToString();
        }
    }
}
