namespace EzEvade_Port
{
    using System;
    using Core;

    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                new Evade();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}