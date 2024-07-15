using System;
using System.Windows.Forms;

namespace HookApp
{
    class Program
    {
        #region Private Methods

        static void Main()
        {
            var hookManager = new HookManager();
            hookManager.InitializeHooks();
            Application.Run();
            hookManager.Dispose();
        }

        #endregion Private Methods
    }
}
