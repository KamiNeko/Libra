using System;

namespace LibraGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var core = new Game())
            {
                core.Run();
            }
        }
    }
}
