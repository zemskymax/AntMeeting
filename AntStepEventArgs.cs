using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AntMeeting
{
    class AntStepEventArgs: EventArgs 
    {
       private readonly int m_x = 0;
       private readonly int m_y = 0;
       private readonly SolidColorBrush m_color = Brushes.BlanchedAlmond;

       // Constructor
       public AntStepEventArgs(int x, int y, SolidColorBrush color) 
       {
           this.m_x = x;
           this.m_y = y;
           this.m_color = color;
       }

       // Properties
       public int ValueX 
       {
          get { return m_x; }
       }
       public int ValueY
       {
           get { return m_y; }
       }

        public SolidColorBrush Color
       {
           get { return m_color; }
       }
    }
}
