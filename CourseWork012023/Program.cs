using System.Windows.Forms;

namespace CourseWork012023
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            GraphData G = new GraphData();
            Application.Run(new Form1(ref G));


        }
    }
}