using System;
using System.IO;

namespace LoodsmanPdf
{
    class CRC32
    {
        static uint[] crc_table = new uint[256];

        public static void BuildTable()
        {
            uint crc;

            for (uint i = 0; i < 256; i++)
            {
                crc = i;
                for (int j = 0; j < 8; j++)
                    crc = ((crc & 1) == 1) ? (crc >> 1) ^ 0xEDB88320 : crc >> 1;

                crc_table[i] = crc;
            }
        }

        public static uint Crc(byte[] array)
        {
            uint result = 0xFFFFFFFF;

            for (int i = 0; i < array.Length; i++)
            {
                byte last_byte = (byte)(result & 0xFF);
                result >>= 8;
                result = result ^ crc_table[last_byte ^ array[i]];
            }

            return result;
        } 
    }
}