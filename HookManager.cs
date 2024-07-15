using HookApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HookApp
{
    public class HookManager
    {
        #region Private Fields

        private readonly List<IHook> _hooks = new List<IHook>();

        #endregion Private Fields

        #region Public Methods

        public void Dispose()
        {
            foreach (var hook in _hooks)
            {
                hook.Stop();
            }
        }

        public void InitializeHooks()
        {
            var mouseHook = new MouseHook();
            _hooks.Add(mouseHook);
            mouseHook.Init();
        }

        #endregion Public Methods
    }
}
