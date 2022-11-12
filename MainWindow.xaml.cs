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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Staff_Registry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StaffManager staffManager;
        private string[] gender = Enum.GetNames(typeof(GenderType));
        private string[] rating = Enum.GetNames(typeof(RatingType));
        private int selectedDGIndex;

        private const string file = @"C:\Users\hazem\Desktop\C# III\assignment_3\SR_DLL\DB.csv";

        public MainWindow()
        {
            InitializeComponent();
            staffManager = new();
            InitalizeCBComponents();
            InitalizeComponentIndex();
            GetDB();
            selectedDGIndex = -1;
        }

        private void InitalizeCBComponents()
        {
            CBGender.ItemsSource = gender;
            CBCommunication.ItemsSource = rating;
            CBDecisionMaking.ItemsSource = rating;
            CBProblemSolving.ItemsSource = rating;
            CBListening.ItemsSource = rating;
            CBLeadership.ItemsSource = rating;
            CBAccuracy.ItemsSource = rating;
        }

        private void InitalizeComponentIndex()
        {
            TBFirstName.Text = "";
            TBLastName.Text = "";
            CBGender.SelectedIndex = 0;
            CBCommunication.SelectedIndex = 2;
            CBDecisionMaking.SelectedIndex = 2;
            CBProblemSolving.SelectedIndex = 2;
            CBListening.SelectedIndex = 2;
            CBLeadership.SelectedIndex = 2;
            CBAccuracy.SelectedIndex = 2;
        }

        private void GenerateID()
        {
            Employee employee = staffManager.StaffList.Last();
            int newID = employee.ID + 1;
            TBID.Text = "" + newID;
        }

        private void GetDB()
        {
            staffManager.StaffList.Clear();
            var lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                Employee employee = new Employee(int.Parse(line[0]), line[1], line[2],
                  (GenderType)(int.Parse(line[3])), (RatingType)int.Parse(line[4]), (RatingType)int.Parse(line[5]),
                   (RatingType)int.Parse(line[6]), (RatingType)int.Parse(line[7]), (RatingType)int.Parse(line[8]),
                    (RatingType)int.Parse(line[9]));
                staffManager.StaffList.Add(employee);
            }
            UppdateDG();
        }

        private void DGSelectedChange(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedDGIndex = DG.SelectedIndex;
            if (selectedDGIndex >= 0)
            {
                TBID.Text = "" + staffManager.StaffList[selectedDGIndex].ID;
                TBFirstName.Text = staffManager.StaffList[selectedDGIndex].FirstName;
                TBLastName.Text = staffManager.StaffList[selectedDGIndex].LastName;
                CBGender.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Gender;
                CBCommunication.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Communication;
                CBDecisionMaking.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Decision_Making;
                CBProblemSolving.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Problem_Solving;
                CBListening.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Listening;
                CBLeadership.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Leadership;
                CBAccuracy.SelectedIndex = (int)staffManager.StaffList[selectedDGIndex].Accuracy;
            }
        }
        private void NewClick(object sender, RoutedEventArgs e)
        {
            selectedDGIndex = -1;
            InitalizeComponentIndex();
            GenerateID();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            selectedDGIndex = -1;
            TBFirstName.Text.Trim();
            TBLastName.Text.Trim();
            if (TBFirstName.Text == "" || TBLastName.Text == "")
            {
                MessageBox.Show("You must write first- and lastname!");
            }
            else
            {
                GenerateID();
                Employee employee = new Employee(
                    int.Parse(TBID.Text),
                    TBFirstName.Text,
                    TBLastName.Text,
                    (GenderType)CBGender.SelectedIndex,
                    (RatingType)CBCommunication.SelectedIndex,
                    (RatingType)CBDecisionMaking.SelectedIndex,
                    (RatingType)CBProblemSolving.SelectedIndex,
                    (RatingType)CBListening.SelectedIndex,
                    (RatingType)CBLeadership.SelectedIndex,
                    (RatingType)CBAccuracy.SelectedIndex
            );
                staffManager.StaffList.Add(employee);
                UppdateDG();
                AddEmployeeToDB(employee);
            }
        }

        private void ChangeClick(object sender, RoutedEventArgs e)
        {
            if (selectedDGIndex >= 0)
            {
                Employee employee = new Employee(
                           int.Parse(TBID.Text),
                           TBFirstName.Text,
                           TBLastName.Text,
                           (GenderType)CBGender.SelectedIndex,
                           (RatingType)CBCommunication.SelectedIndex,
                           (RatingType)CBDecisionMaking.SelectedIndex,
                           (RatingType)CBProblemSolving.SelectedIndex,
                           (RatingType)CBListening.SelectedIndex,
                           (RatingType)CBLeadership.SelectedIndex,
                           (RatingType)CBAccuracy.SelectedIndex);
                //staffManager.StaffList.RemoveAt(DG.SelectedIndex);
                staffManager.StaffList[DG.SelectedIndex] = employee;
                UppdateDG();
                UpdateDB();
            }
            else
            {
                MessageBox.Show("You must select an employee!");
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex >= 0)
            {
                staffManager.StaffList.RemoveAt(DG.SelectedIndex);
                UppdateDG();
                UpdateDB();
            }
        }
        private void SearchClick(object sender, RoutedEventArgs e)
        {

        }

        private void UppdateDG()
        {
            DG.ItemsSource = null;
            DG.ItemsSource = staffManager.StaffList;
        }

        private void AddEmployeeToDB(Employee employee)
        {
            File.AppendAllText(file, $"{employee.ID},{employee.FirstName}," +
                                     $"{employee.LastName},{(int)employee.Gender},{(int)employee.Communication}," +
                                     $"{(int)employee.Decision_Making},{(int)employee.Problem_Solving}," +
                                     $"{(int)employee.Listening},{(int)employee.Leadership},{(int)employee.Accuracy}\n");
        }

        private void UpdateDB()
        {
            string lines = "";
            foreach (var s in staffManager.StaffList)
            {
                lines += $"{s.ID},{s.FirstName},{s.LastName}," +
                    $"{(int)s.Gender},{(int)s.Communication}," +
                    $"{(int)s.Decision_Making},{(int)s.Problem_Solving}," +
                    $"{(int)s.Listening},{(int)s.Leadership},{(int)s.Accuracy}\n";
            }
            File.WriteAllText(file, lines);
        }
    }
}
