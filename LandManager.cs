using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMeeting
{

    public class LandManager
    {
        private Hashtable m_map = new Hashtable();
        private Dictionary<int, string> m_antLocation = new Dictionary<int, string>();
        private Dictionary<string, int> m_pheromonMap = new Dictionary<string, int>();


        private static LandManager m_instance = null;

        private LandManager()
        {
        }

        public static LandManager The
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new LandManager();
                }

                return m_instance;
            }
        }

        public void visitCell(int x, int y, int args)
        {
            string location = string.Format("{0},{1}", x, y);

            try
            {
                m_map.Add(location, args);
            }
            catch
            {

            }

            if (m_antLocation.ContainsKey(args))
            {
                m_antLocation.Remove(args);
            }
            m_antLocation.Add(args, location); 
        }

        public bool isCellFree(int x, int y, int args)
        {
            string location = string.Format("{0},{1}", x, y);

            if (m_antLocation.ContainsValue(location))
            {
                if (m_antLocation.FirstOrDefault(i => i.Value == location).Key != args)
                { 
                    return false;
                }
            }

            return true;
        }

        public bool isBlankCell(int x, int y)
        {
            string location = string.Format("{0},{1}", x, y);

            if (m_map.ContainsKey(location))
            {
                return false;
            }

            return true;
        }


        #region vector functions
        public bool isMarkededByOher(int x, int y, int myId)
        {
            string location = string.Format("{0},{1}", x, y);

            if (m_pheromonMap.ContainsKey(location))
            {
                if (m_pheromonMap[location] != myId)
                {
                    return false;                    
                }
            }

            return true;
        }

        public void markCellWithPheromon(int x, int y, int myId)
        {
            string location = string.Format("{0},{1}", x, y);

            if (!m_pheromonMap.ContainsKey(location))
            {
                m_pheromonMap.Add(location, myId);
            }

            if (m_antLocation.ContainsKey(myId))
            {
                m_antLocation.Remove(myId);
            }
            m_antLocation.Add(myId, location);
        }
        #endregion
    }
}
