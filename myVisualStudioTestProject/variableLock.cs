using System;
using System.Collections.Generic;
using System.Text;

namespace myVisualStudioTestProject
{
    class variableLock : ITaskLock
    {
        private bool lockValue;

        private readonly object successLock;

        public variableLock()
        {
            this.successLock = new object();
            this.lockValue = true;
        }

        public bool getLockValue()
        {
            return this.lockValue;
        }

        public void mergeLockValue(bool lockValue)
        {
            lock(this.successLock)
            {
                this.lockValue = this.lockValue && lockValue;
            }
        }
    }
}
