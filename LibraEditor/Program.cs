using System;

namespace LibraEditor
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
