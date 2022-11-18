// Hazem Kudaimi November 2022

//A Subscriber class  (a listerner)
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

namespace Staff_Registry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This class creates a search window for a search auction
    /// but does not save the info on the search 
    /// Receives search information and show it in the datagrid.
    /// This class is mainly a subscriber of the events fired by 
    /// the search window class.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StaffManager staffManager;
        private readonly string[] gender = Enum.GetNames(typeof(GenderType));
        private readonly string[] rating = Enum.GetNames(typeof(RatingType));
        private int selectedDGIndex;
        private const string file = @"DB.csv";
        //private const string file = @"C:\Users\hazem\Desktop\C# III\assignment_3\SR_PL\DB.csv";


        // Using Action delegate with lambda expression               
        private readonly Action<string> WriteToMessageBox = str =>
        {
            MessageBox.Show("" + str);
        };

        public MainWindow()
        {
            InitializeComponent();
            staffManager = new StaffManager(file);
            InitalizeCBComponents();
            InitalizeComponentIndex();
            GetStaffListFromDB();
            selectedDGIndex = -1;
        }

        #region Initalizing GUI
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
            TBID.Text = "";
            TBFirstName.Text = "";
            TBLastName.Text = "";
            CBGender.SelectedIndex = 1;
            CBCommunication.SelectedIndex = 3;
            CBDecisionMaking.SelectedIndex = 3;
            CBProblemSolving.SelectedIndex = 3;
            CBListening.SelectedIndex = 3;
            CBLeadership.SelectedIndex = 3;
            CBAccuracy.SelectedIndex = 3;
        }
        #endregion

        #region New, Add, Change, Delete, Search Clicked
        private void NewClick(object sender, RoutedEventArgs e)
        {
            selectedDGIndex = -1;
            InitalizeComponentIndex();
            GenerateID();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            selectedDGIndex = -1;
            if (TBFirstName.Text.Trim() == "" || TBLastName.Text.Trim() == "")
            {
                WriteToMessageBox("You must write first- and lastname!");
            }
            else
            {
                GenerateID();
                Employee employee = new (
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
                Employee employee = new (
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
                staffManager.StaffList[DG.SelectedIndex] = employee;
                UppdateDG();
                SaveStaffToDB();
                InitalizeComponentIndex();
            }
            else
            {
                WriteToMessageBox("You must select an employee!");
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (selectedDGIndex >= 0)
            {
                var result = MessageBox.Show("Are you sure?", "Delet employee",
                                                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    staffManager.StaffList.RemoveAt(selectedDGIndex);
                    UppdateDG();
                    SaveStaffToDB();
                    InitalizeComponentIndex();
                }
            }
            else
            {
                WriteToMessageBox("You must select an employee!");
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new ();
            searchWindow.Show();
            InputPanel.Visibility = Visibility.Hidden;
            MainPanel.Visibility = Visibility.Hidden;
            searchWindow.EmployeeProcessorEvent += StaffFilter;
            searchWindow.CancelClicked += SearchWindowCancelClicked;
        }

        private void SearchWindowCancelClicked()
        {
            InputPanel.Visibility = Visibility.Visible;
            MainPanel.Visibility = Visibility.Visible;
            UppdateDG();
        }

        private void StaffFilter(object? sender, EmployeeArgs e)
        {
            Employee employee = e.Employee;
            List<Employee> staffLis = new();
            // Using lambda expressions with LINQ
            if (employee.FirstName != "")
            {
                var staffList = staffManager.StaffList.Where(x => x.FirstName.ToLower().Contains(employee.FirstName.ToLower()));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.LastName != "")
            {
                var staffList = staffManager.StaffList.Where(x => x.LastName.ToLower().Contains(employee.LastName.ToLower()));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Gender != GenderType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Gender.Equals(employee.Gender));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Communication != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Communication.Equals(employee.Communication));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Decision_Making != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Decision_Making.Equals(employee.Decision_Making));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }

            if (employee.Problem_Solving != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Problem_Solving.Equals(employee.Problem_Solving));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Listening != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Listening.Equals(employee.Listening));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Leadership != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Leadership.Equals(employee.Leadership));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (employee.Accuracy != RatingType.Empty)
            {
                var staffList = staffManager.StaffList.Where(x => x.Accuracy.Equals(employee.Accuracy));
                foreach (Employee em in staffList)
                {
                    staffLis.Add(em);
                }
            }
            if (staffLis.Count > 0)
            {
                var staffList = staffLis.Distinct();
                DG.ItemsSource = null;
                DG.ItemsSource = staffList;
            }
            else
            {
                MessageBox.Show("No result matched!");
                UppdateDG();
            }
        }
        #endregion

        #region Help Methods
        private void GenerateID()
        {
            Employee employee = staffManager.StaffList.Last();
            int newID = employee.ID + 1;
            TBID.Text = "" + newID;
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

                // Count of the rating score of the empolyee competence:
                //string str = string.Format("{0:0.##}", staffManager.StaffList[selectedDGIndex].CompetenceDegree());

                // Count of the rating score of the empolyee competence: But with another way, by using lambda expressions with LINQ
                List<int> competenceListAll = new ()
                {
                    (int)staffManager.StaffList[selectedDGIndex].Communication,
                    (int)staffManager.StaffList[selectedDGIndex].Decision_Making,
                    (int)staffManager.StaffList[selectedDGIndex].Problem_Solving,
                    (int)staffManager.StaffList[selectedDGIndex].Listening,
                    (int)staffManager.StaffList[selectedDGIndex].Leadership,
                    (int)staffManager.StaffList[selectedDGIndex].Accuracy
                };
                var competenceList = competenceListAll.Where(x => x > 0).ToList();
                double CompetenceSum = competenceList.Sum(x => x);

                // Using Func delegate with lambda statement
                Func<double, double, double> average = (num1, num2) =>
                {
                    double result = num1 / num2;
                    return result;
                };
                double result = average(CompetenceSum, competenceList.Count);
                TBRating.Text = string.Format("{0:0.##}", result);
            }
        }

        private void UppdateDG()
        {
            DG.ItemsSource = null;
            DG.ItemsSource = staffManager.StaffList;
        }
        #endregion

        #region Data Base 
        private void GetStaffListFromDB()
        {
            staffManager.StaffList.Clear();
            bool result = staffManager.GetStaffListFromDatabase();
            UppdateDG();
            if (!result)
            {
                MessageBox.Show("There is with getting the data from the file, check the file path!");
            }
        }

        private void SaveStaffToDB()
        {
            staffManager.SaveStaffListToDatabase();
        }

        private void AddEmployeeToDB(Employee employee)
        {
            if (staffManager.AddEmployeeToDatabase(employee))
            {
                MessageBox.Show("Add Employee to data base is succeed");
            }
            else
                MessageBox.Show("There is a problem with the data base, the employee is not added!");
        }
        #endregion
    }
}
