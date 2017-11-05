namespace EzEvade_Port.Utils
{
    using System;

    public static class ConsolePrinter
    {
        private static float _lastPrintTime;

        public static void Print(string str)
        {
            var timeDiff = Environment.TickCount - _lastPrintTime;

            var finalStr = "[" + timeDiff + "] " + str;

            Console.WriteLine(finalStr);

            _lastPrintTime = Environment.TickCount;
        }
    }
}