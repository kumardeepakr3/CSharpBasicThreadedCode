using System;
using System.Collections.Generic;
using System.Text;

namespace myVisualStudioTestProject
{
    interface ITaskLock 
    {
        bool getLockValue();

        void mergeLockValue(bool value);
    }
}
