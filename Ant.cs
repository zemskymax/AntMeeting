using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AntMeeting
{
    public enum Algorithm
    {
        Noa, 
        Noa_Extended, 
        Cross
    }

    class Ant 
    {
        private const int VISIBLE_AREA = 1;

        private int m_id = -1;

        private int m_startX = -1, m_startY = -1;
        private int m_currX = 0, m_currY = 0;
        private int dx = 0, dy = -1;

        private SolidColorBrush m_color;

        public event EventHandler StepCompleted;
        public bool Found = false;
        public bool StopOnDetect = false;

        enum Action
        {
            UpOnY,
            DownOnY,
            YtoXtransaction,
            UpOnX,
            DownOnX,
            XtoYtransaction
        }

        private Action m_currAction = Action.DownOnY;
        
        public Ant(int id, string startPos, SolidColorBrush color)
        {
            char[] delimiterChars = {' ', '(', ',', ')', '\t' };

            string[] coordinates = startPos.Split(delimiterChars);

            Int32.TryParse(coordinates[1], out m_startX);
            Int32.TryParse(coordinates[2], out m_startY);

            m_id = id;

            m_color = color;
        }

        public SolidColorBrush GetColor()
        {
            return m_color;
        }
        
        #region NOA
        public void Search_NOA()
        {
            if (Found)
            {
                return;
            }

            if ((m_currX == m_currY) || ((m_currX < 0) && (m_currX == -m_currY)) || ((m_currX > 0) && (m_currX == 1 - m_currY)))
            {
                int temp = dx;
                dx = -dy;
                dy = temp;
            }

            int prev_global_pos_x = m_startX + m_currX;
            int prev_global_pos_y = m_startY + m_currY;

            m_currX += dx;
            m_currY += dy;

            //Console.WriteLine("x: {0}. y: {1}." , m_currX, m_currY);

            int curr_global_pos_x = m_startX + m_currX;
            int curr_global_pos_y = m_startY + m_currY;


            if (LandManager.The.isBlankCell(curr_global_pos_x, curr_global_pos_y) || !StopOnDetect)
            {
                LandManager.The.visitCell(curr_global_pos_x, curr_global_pos_y, m_id);

                if (StepCompleted != null)
                {
                    StepCompleted(this, new AntStepEventArgs(curr_global_pos_x, curr_global_pos_y, m_color));
                }
            }
            else
            {
                Found = true;
            }

            if (ScanExtendedArea(curr_global_pos_x, curr_global_pos_y))
            {
                Found = true;
            }
        }
        #endregion

        #region Cross
        private int m_runningDistance = 10;
        public void Search_Cross()
        {
            if (Found)
            {
                return;
            }

            //going down on y
            if (m_currAction == Action.DownOnY && m_currY == -m_runningDistance)
            {
                m_currAction = Action.UpOnY;
                dy = -dy; //turn up    
            }
            //going up on y
            else if (m_currAction == Action.UpOnY && m_currY == m_runningDistance)
            {
                m_currAction = Action.YtoXtransaction;
                dy = -dy; //turn down
            }
            else if (m_currAction == Action.YtoXtransaction && m_currX == 0 && m_currY == 0)
            {
                m_currAction = Action.DownOnX;
                int temp = dy;
                dy = dx;
                dx = temp;
            }
            //going down on x
            else if (m_currAction == Action.DownOnX && m_currX == -m_runningDistance)
            {
                m_currAction = Action.UpOnX;
                dx = -dx; //turn up  
            }
            //going up on x
            else if (m_currAction == Action.UpOnX && m_currX == m_runningDistance)
            {
                m_currAction = Action.XtoYtransaction;
                dx = -dx; //turn down  
            }
            else if (m_currAction == Action.XtoYtransaction && m_currX == 0 && m_currY == 0)
            {
                m_currAction = Action.DownOnY;

                int temp = dx;
                dx = dy;
                dy = temp;

                m_runningDistance *= 2;
            }

            m_currX += dx;
            m_currY += dy;

            int curr_global_pos_x = m_startX + m_currX;
            int curr_global_pos_y = m_startY + m_currY;


            if (LandManager.The.isMarkededByOher(curr_global_pos_x, curr_global_pos_y, m_id) || !StopOnDetect)
            {
                LandManager.The.markCellWithPheromon(curr_global_pos_x, curr_global_pos_y, m_id);

                if (StepCompleted != null)
                {
                    StepCompleted(this, new AntStepEventArgs(curr_global_pos_x, curr_global_pos_y, m_color));
                }
            }
            else
            {
                Found = true;
            }

            if (ScanExtendedArea(curr_global_pos_x, curr_global_pos_y))
            {
                Found = true;
            }
        }
        #endregion

        #region Extendede NOA
        public void Search_Extended_NOA()
        {
            if(Found)
            {
                return;
            }

            if ((m_currX == m_currY) || ((m_currX < 0) && (m_currX == -(2 * m_currY))) || ((m_currX > 0) && (m_currX == 2 - m_currY)))
            {
                int temp = dx;
                dx = -dy;
                dy = temp;
            }

            int prev_global_pos_x = m_startX + m_currX;
            int prev_global_pos_y = m_startY + m_currY;

            m_currX += dx;
            m_currY += dy;

            //Console.WriteLine("x: {0}. y: {1}." , m_currX, m_currY);

            int curr_global_pos_x = m_startX + m_currX;
            int curr_global_pos_y = m_startY + m_currY;


            if (LandManager.The.isBlankCell(curr_global_pos_x, curr_global_pos_y) || !StopOnDetect)
            {
                LandManager.The.visitCell(curr_global_pos_x, curr_global_pos_y, m_id);

                if (StepCompleted != null)
                {
                    StepCompleted(this, new AntStepEventArgs(curr_global_pos_x, curr_global_pos_y, m_color));
                }
            }
            else
            {
                Found = true;
            }

            if (CheckExtendedAreaForPheromone(curr_global_pos_x, curr_global_pos_y))
            {
                Found = true;
            }
        }
        #endregion


        private bool ScanExtendedArea(int currX, int currY/*, int prevX, int prevY*/)
        {
            for (int i = (currY + VISIBLE_AREA); i >= (currY - VISIBLE_AREA); i--)
            {
                for (int j = (currX - VISIBLE_AREA); j <= (currX + VISIBLE_AREA); j++)
                {
                    //bool myCurrLoc = (j == currX && i == currY);
                    //bool myPrevLoc = (j == prevX && i == prevY);
                    if (!LandManager.The.isCellFree(j, i, m_id))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckExtendedAreaForPheromone(int currX, int currY)
        {
            for (int i = (currY + VISIBLE_AREA); i >= (currY - VISIBLE_AREA); i--)
            {
                for (int j = (currX - VISIBLE_AREA); j <= (currX + VISIBLE_AREA); j++)
                {
                    if (!LandManager.The.isCellFree(j, i, m_id))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
