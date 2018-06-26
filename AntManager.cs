using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AntMeeting
{
    class AntManager
    {
        private Ant ant_a = null, ant_b = null;
        private bool m_keepSearching;
        private int m_stepCounter = 0;
        private Algorithm m_algorithm = Algorithm.Noa;

        public event EventHandler OnStepCompleted;
        public event EventHandler OnSearchCompleted;

        public void InitializeProcess(string startingPosition, string targetPosition, Algorithm al)
        {
            ant_a = new Ant(1, startingPosition, Brushes.GreenYellow);
            ant_b = new Ant(2, targetPosition, Brushes.Ivory);

            ant_b.StopOnDetect = true;

            ant_a.StepCompleted += OnStepCompleted;
            ant_b.StepCompleted += OnStepCompleted;
            
            m_keepSearching = true;

            m_algorithm = al;
        }

        public void BeginProcess()
        {
            Thread process = new Thread(new ThreadStart(Process));
            process.Start();
        }

        public void EndProcess()
        {
            m_keepSearching = false;

            m_stepCounter = 0;
        }

        private void Process()
        {
            while (m_keepSearching)
            {
                switch (m_algorithm)
                {
                    case Algorithm.Noa:
                        ant_a.Search_NOA();
                        ant_b.Search_NOA();
                        break;

                    case Algorithm.Noa_Extended:
                        ant_a.Search_Extended_NOA();
                        ant_b.Search_Extended_NOA();
                        break;

                    case Algorithm.Cross:
                        ant_a.Search_Cross();
                        ant_b.Search_Cross();
                        break;
                 }

                ++m_stepCounter;

                if (ant_a.Found && ant_b.Found)
                {
                    m_keepSearching = false;
                    break;
                }

                Thread.Sleep(100);
            }

            if (OnSearchCompleted != null)
            {
                OnSearchCompleted(this, new AntSearchEventArgs(m_stepCounter));

                m_stepCounter = 0;
            }
        }
    }
}
