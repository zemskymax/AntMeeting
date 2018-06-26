using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AntMeeting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Button> m_buttonCollection = new Dictionary<string, Button>();
        private string m_startingPosition = "", m_targetPosition = "";

        private AntManager m_manager = null;
        private Algorithm al = Algorithm.Noa;

        public MainWindow()
        {
            InitializeComponent();

            BuildGrid(20, 20);

            _Stop.IsEnabled = false;

            m_manager = new AntManager();

            m_manager.OnStepCompleted += OnStepCompleted;
            m_manager.OnSearchCompleted += OnSearchCompleted;
        }

        public void BuildGrid(int rows, int columns)
        {
            for (int i = rows; i >= -rows; i--)
            {
                StackPanel row = new StackPanel();
                row.Orientation = Orientation.Horizontal;

                for (int j = -columns; j <= columns; j++)
                {
                    Button area = new Button();
                    area.MinWidth = 20;
                    area.MinHeight = 20;
                    string areaLocation = string.Format("({0},{1})", j, i);
                    area.ToolTip = areaLocation;
                    area.Click += new RoutedEventHandler(btnSetInitialPosigions_Click);
                    row.Children.Add(area);
                    m_buttonCollection.Add(areaLocation, area);
                }

                _Container.Children.Add(row);
            }
        }

        private void btnSetInitialPosigions_Click(object sender, RoutedEventArgs e)
        {
            Button area = sender as Button;

            area.Background = area.Background == Brushes.Blue ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Blue;

            if (m_startingPosition.Equals(""))
            {
                m_startingPosition = area.ToolTip.ToString();
            }
            else if (m_startingPosition.Equals(area.ToolTip.ToString()))
            {
                m_startingPosition = "";
                area.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
            }
            else if (m_targetPosition.Equals(""))
            {
                m_targetPosition = area.ToolTip.ToString();
            }
            else if (m_targetPosition.Equals(area.ToolTip.ToString()))
            {
                m_targetPosition = "";
                area.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
            }
        }

        private void OnStepCompleted(object sender, EventArgs e)
        {
            AntStepEventArgs args = e as AntStepEventArgs;

            string stepLocation = string.Format("({0},{1})", args.ValueX, args.ValueY);

            Button buttonInLocation;
            if (m_buttonCollection.TryGetValue(stepLocation, out buttonInLocation))
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (buttonInLocation.Background != Brushes.Blue)
                    {
                        buttonInLocation.Background = args.Color;
                    }
                });                
            }
        }

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                _Reset.IsEnabled = true;
                _Stop.IsEnabled = false;
            });
            
            AntSearchEventArgs args = e as AntSearchEventArgs;

            string msg = string.Format("Game Ended in {0} steps.", args.Steps);
            MessageBox.Show(msg, "AntMeeting");
        }

        private void OnClickStart(object sender, RoutedEventArgs e)
        {
            if (m_startingPosition.Equals("") && m_targetPosition.Equals(""))
            {
                string msg = string.Format("Please provice ant initial positions.");
                MessageBox.Show(msg, "AntMeeting");
                return;
            }

            _Start.IsEnabled = false;
            _Reset.IsEnabled = false;
            _Stop.IsEnabled = true;

            m_manager.InitializeProcess(m_startingPosition, m_targetPosition, al);
            m_manager.BeginProcess();
        }

        private void OnClickStop(object sender, RoutedEventArgs e)
        {
            m_manager.EndProcess();

            _Reset.IsEnabled = true;
        }

        private void OnClickReset(object sender, RoutedEventArgs e)
        {
            m_startingPosition = "";
            m_targetPosition = "";

            foreach (var btn in m_buttonCollection)
            {
                btn.Value.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
            }

            _Start.IsEnabled = true;
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton ck = sender as RadioButton;
            if (ck != null && ck.IsChecked.Value)
            {
                switch (ck.Name)
                {
                    case "_noa":
                        al = Algorithm.Noa;
                        break;

                    case "_noa_extended":
                        al = Algorithm.Noa_Extended;
                        break;

                    case "_cross":
                        al = Algorithm.Cross;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
