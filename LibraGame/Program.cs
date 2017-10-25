using System;

namespace LibraCore
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var core = new MyCore())
            {
                core.Run();
            }
        }
    }
}
