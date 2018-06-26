using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMeeting
{
    class AntSearchEventArgs : EventArgs 
    {
        private readonly int m_stepCount = 0;

        public AntSearchEventArgs(int stepCount) 
        {
            this.m_stepCount = stepCount;
        }

        public int Steps
        {
            get { return m_stepCount; }
        }
    }
}
