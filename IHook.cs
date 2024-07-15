using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookApp
{
    public interface IHook
    {
        #region Public Methods

        void Init();
        void Stop();

        #endregion Public Methods
    }
}
