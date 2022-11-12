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
        public static string SearchInfo = "";


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
            CBCommunicationSP.SelectedIndex = 2;
            CBDecisionMakingSP.SelectedIndex = 2;
            CBProblemSolvingSP.SelectedIndex = 2;
            CBListeningSP.SelectedIndex = 2;
            CBLeadershipSP.SelectedIndex = 2;
            CBAccuracySP.SelectedIndex = 2;
        }
        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (SearchInfo == "")
            {
                MessageBox.Show("You must select ", "Warning", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
