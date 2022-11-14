using SR_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Staff_Registry
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        // Using EventHandler
        public event EventHandler<EmployeeArgs>? EmployeeProcessorEvent;
        
        // Using Event
        public delegate void CancelClickedDelegate();
        public event CancelClickedDelegate? CancelClicked;

        private string[] gender = Enum.GetNames(typeof(GenderType));
        private string[] rating = Enum.GetNames(typeof(RatingType));
        public SearchWindow()
        {
            InitializeComponent();
            InitalizeCBComponents();
            InitalizeComponentIndex();
        }


        private void InitalizeCBComponents()
        {
            CBGenderSP.ItemsSource = gender;
            CBCommunicationSP.ItemsSource = rating;
            CBDecisionMakingSP.ItemsSource = rating;
            CBProblemSolvingSP.ItemsSource = rating;
            CBListeningSP.ItemsSource = rating;
            CBLeadershipSP.ItemsSource = rating;
            CBAccuracySP.ItemsSource = rating;
        }

        private void InitalizeComponentIndex()
        {
            TBFirstNameSP.Text = "";
            TBLastNameSP.Text = "";
            CBGenderSP.SelectedIndex = 0;
            CBCommunicationSP.SelectedIndex = 0;
            CBDecisionMakingSP.SelectedIndex = 0;
            CBProblemSolvingSP.SelectedIndex = 0;
            CBListeningSP.SelectedIndex = 0;
            CBLeadershipSP.SelectedIndex = 0;
            CBAccuracySP.SelectedIndex = 0;
        }
        private void SearchClick(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee(
                                        0,
                                        TBFirstNameSP.Text,
                                        TBLastNameSP.Text,
                                        (GenderType)CBGenderSP.SelectedIndex,
                                        (RatingType)CBCommunicationSP.SelectedIndex,
                                        (RatingType)CBDecisionMakingSP.SelectedIndex,
                                        (RatingType)CBProblemSolvingSP.SelectedIndex,
                                        (RatingType)CBListeningSP.SelectedIndex,
                                        (RatingType)CBLeadershipSP.SelectedIndex,
                                        (RatingType)CBAccuracySP.SelectedIndex
                                        );

            EmployeeArgs args = new EmployeeArgs(employee);
            EmployeeSearchEventInfoProcessor(args);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        public void CloseWindow()
        {
            this.Close();
            if (CancelClicked!= null)
            {
                CancelClicked.Invoke();    
            }
        }

        public void EmployeeSearchEventInfoProcessor(EmployeeArgs args)
        {
            if (EmployeeProcessorEvent != null)
            {
                EmployeeProcessorEvent(this, args);
            }
        }
    }
}
