using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Console2HTML
{
    public struct CharWithColor
    {
        public ConsoleColor Color;
        public char Char;
    }

    public class ConsoleBufferReader
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CHAR_INFO
        {
            public UnionUShort CharData;  // Union between WCHAR and ASCII char
            public short Attributes;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        internal struct UnionUShort
        {
            [FieldOffset(0)] public byte High;
            [FieldOffset(1)] public byte Low;
            [FieldOffset(0)] public ushort UShort;
            [FieldOffset(0)] public unsafe fixed byte Bytes[2];
        }

        internal struct Chars2Struct
        {
            public unsafe fixed char Chars[2];
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        public static CharWithColor[,] ReadConsoleBuffer()
        {
            // Получаем дескриптор выходного буфера консоли
            IntPtr consoleHandle = GetStdHandle(-11); // STD_OUTPUT_HANDLE = -11

            // Получаем информацию о буфере
            CONSOLE_SCREEN_BUFFER_INFO bufferInfo;
            if (!GetConsoleScreenBufferInfo(consoleHandle, out bufferInfo))
            {
                throw new Exception("Не удалось получить информацию о буфере консоли");
            }

            // Создаем область чтения, соответствующую размеру буфера
            SMALL_RECT readRegion = new SMALL_RECT
            {
                Left = 0,
                Top = 0,
                Right = (short)(bufferInfo.dwSize.X - 1),
                Bottom = (short)(bufferInfo.dwSize.Y - 1)
            };

            // Создаем буфер для чтения
            int bufferSize = readRegion.Right * readRegion.Bottom + 1;
            CHAR_INFO[] buffer = new CHAR_INFO[bufferSize];

            // Читаем содержимое буфера
            ReadConsoleOutputImpl(consoleHandle, buffer, readRegion);

            // Конвертируем результат в нужный формат
            var outputEncoding = Console.OutputEncoding;

            var result = new CharWithColor[bufferInfo.dwSize.Y, bufferInfo.dwSize.X];
            for (int y = 0; y < readRegion.Bottom; y++)
            {
                for (int x = 0; x < readRegion.Right; x++)
                {
                    var index = x + y * readRegion.Right;
                    var charInfo = buffer[index];

                    unsafe
                    {
                        Chars2Struct chars2;
                        outputEncoding.GetChars(charInfo.CharData.Bytes, 2, chars2.Chars, 2);
                        result[y, x].Char = chars2.Chars[0];
                    }

                    result[y, x].Color = (ConsoleColor)charInfo.Attributes;
                }
            }

            return result;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpBufferInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern unsafe bool ReadConsoleOutput(IntPtr hConsoleOutput, CHAR_INFO* pBuffer, COORD bufferSize, COORD bufferCoord, ref SMALL_RECT readRegion);


        internal static unsafe void ReadConsoleOutputImpl(IntPtr hConsoleOutput, CHAR_INFO[] buffer, SMALL_RECT readRegion)
        {
            bool readResult;
            fixed (CHAR_INFO* pCharInfo = buffer)
            {
                readResult = ReadConsoleOutput(
                    hConsoleOutput,
                    pCharInfo,
                    new COORD { X = readRegion.Right, Y = readRegion.Bottom },
                    new COORD { X = 0, Y = 0 },
                    ref readRegion);
            }

            if (!readResult)
            {
                //int errorCode = Marshal.GetLastWin32Error();
                var lastErrorString = GetLastErrorString();
                throw new Exception("Не удалось прочитать буфер консоли: " + lastErrorString);
            }
        }

        enum FORMAT_MESSAGE : uint
        {
            ALLOCATE_BUFFER = 0x00000100,
            IGNORE_INSERTS = 0x00000200,
            FROM_SYSTEM = 0x00001000,
            ARGUMENT_ARRAY = 0x00002000,
            FROM_HMODULE = 0x00000800,
            FROM_STRING = 0x00000400
        }

        [DllImport("kernel32.dll")]
        static extern int FormatMessage(FORMAT_MESSAGE dwFlags, IntPtr lpSource, int dwMessageId, uint dwLanguageId, out StringBuilder msgOut, int nSize, IntPtr Arguments);

        //

        public static int GetLastError()
        {
            return (Marshal.GetLastWin32Error());
        }

        public static string GetLastErrorString()
        {
            int lastError = GetLastError();
            if (0 == lastError) return ("");
            else
            {
                StringBuilder msgOut = new StringBuilder(256);
                int size = FormatMessage(FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                    IntPtr.Zero, lastError, 0, out msgOut, msgOut.Capacity, IntPtr.Zero);
                return (msgOut.ToString().Trim());
            }
        }
    }
}
