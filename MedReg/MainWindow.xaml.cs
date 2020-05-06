using MedReg.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
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

namespace MedReg
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //all requests in code make sense in an external config to change with changing situations
        private string cs = ConfigurationManager.ConnectionStrings["MedRegContext"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
            CustomInitialize();
        }

        /// <summary>
        /// custom binding dg to table initialize and refresh
        /// </summary>
        private void CustomInitialize()
        {
            try
            {
                BindDataGrid(dgP, "SELECT * FROM Patients ORDER BY Family ASC");
                BindDataGrid(dgV, "SELECT * FROM Visits");
            }
            catch (Exception ex)
            {
                MessageBox.Show("database not exist , please select Card  tab and create new card patient and card visit for working later");
            }
        }
        
        /// <summary>
        /// binding data grid 
        /// </summary>
        /// <param name="dgName">data grid name</param>
        /// <param name="query">query to db</param>
        private void BindDataGrid(DataGrid dgName, string query)
        {
            var da = new SqlDataAdapter($"{query}", cs);
            var dt = new DataTable();
            da.Fill(dt);
            dgName.ItemsSource = dt.DefaultView;
        }

        /// <summary>
        /// method adds patients
        /// </summary>
        public void PatientAdd()
        {
            using (var context = new MedRegContext())
            {
                var patient = new Patient
                {
                    Family = Family.Text,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Gender = Gender.Text,
                    Birthday = Birthday.Text,
                    Adress = Adress.Text,
                    PhoneNumber = PhoneNumber.Text,
                    Snils = Snils.Text
                };
                if (Snils.Text.Equals("Snils - common element"))
                {
                    MessageBox.Show("Enter native snils");
                }
                else
                {
                    context.Patients.Add(patient);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// method adds patients
        /// </summary>
        public void VisitAdd()
        {
            using (var context = new MedRegContext())
            {
                var visit = new Visit
                {
                    VisitDate = VisitDate.Text,
                    VisitType = VisitType.Text,
                    Diagnosis = Diagnosis.Text,
                    Snils = Snils.Text
                };
                if (Snils.Equals("Snils - common element"))
                {
                    MessageBox.Show("Enter native snils");
                }
                else
                {
                    context.Visits.Add(visit);
                    context.SaveChanges();
                }

            }
        }

        /// <summary>
        /// Create card button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (CardItem.IsSelected == true)
            {
                if (cardPatient.IsChecked == true)
                {
                    PatientAdd();
                }
                else if (cardVisit.IsChecked == true)
                {
                    VisitAdd();
                }
            }
            else
            {
                MessageBox.Show("Select tab Card for created new cards");
            }
            CustomInitialize();
            MainItem.IsSelected = true;
        }

        /// <summary>
        /// Edit Button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Delete.IsEnabled = true;
            Save.IsEnabled = true;
            if (CardItem.IsSelected == true)
            {
                MessageBox.Show("Select tab Main for select record");
            }
            else
            {
                
            }
        }

       

        /// <summary>
        /// Delete button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MainItem.IsSelected == true)
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {                    
                    SqlCommand CPD = new SqlCommand($"delete from Patients where Snils ='{Snils.Text}'", connection);
                    CPD.Connection.Open();
                    CPD.ExecuteNonQuery();
                    CPD.Connection.Close();
                    CustomInitialize();

                    SqlCommand CVD = new SqlCommand($"delete from Visits where Snils ='{Snils.Text}'", connection);
                    CVD.Connection.Open();
                    CVD.ExecuteNonQuery();
                    CVD.Connection.Close();
                    CustomInitialize();
                }
            }
            else if(CardItem.IsSelected == true)
            {
                //visit without patient - can not be, so it makes no sense to remove the patient without deleting visits
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand CVD = new SqlCommand($"delete from Visits where VisitDate ='{VisitDate.Text}'", connection);
                    CVD.ExecuteNonQuery();
                    CVD.Connection.Close();
                    CustomInitialize();
                }
            }
            
        }

        /// <summary>
        /// Save button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(cardPatient.IsChecked == true)
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand CVD = new SqlCommand($"update Patient set Family = '{Family.Text}', FirstName = '{FirstName.Text}', " +
                        $"LastName = '{LastName.Text}', Gender = '{Gender.Text}', Birthday = '{Birthday.Text}', Adress = '{Adress.Text}', " +
                        $"PhoneNumber = '{PhoneNumber.Text}' where snils ='{Snils.Text}'", connection);
                    CVD.ExecuteNonQuery();
                    CVD.Connection.Close();
                    CustomInitialize();
                }
            }
            else if(cardVisit.IsChecked == true)
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand CVD = new SqlCommand($"update Visits set VisitType = '{VisitType.Text}', Diagnosis = '{Diagnosis.Text}' where VisitDate ='{VisitDate.Text}'", connection);
                    CVD.ExecuteNonQuery();
                    CVD.Connection.Close();
                    CustomInitialize();
                }
            }
        }

        /// <summary>
        /// view visits history by datarowsview.selecteditem
        /// and add to text box for editing later data about patient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView rs = dg.SelectedItem as DataRowView;
            if (rs != null)
            {
                Family.Text = rs["Family"].ToString();
                FirstName.Text = rs["FirstName"].ToString();
                LastName.Text = rs["LastName"].ToString();
                Gender.Text = rs["Gender"].ToString();
                Adress.Text = rs["Adress"].ToString();
                PhoneNumber.Text = rs["PhoneNumber"].ToString();
                Snils.Text = rs["Snils"].ToString();
                cardPatient.IsChecked = true;
                BindDataGrid(dgV, $"Select * from Visits where Snils = '{rs["Snils"]}'");
            }
        }
        
        /// <summary>
        /// add to text box for editing later data about visit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView rs = dg.SelectedItem as DataRowView;
            if (rs != null)
            {
                Snils.Text = rs["Snils"].ToString();
                VisitDate.Text = rs["VisitDate"].ToString();
                VisitType.Text = rs["VisitType"].ToString();
                Diagnosis.Text = rs["Diagnosis"].ToString();
                cardVisit.IsChecked = true;
            }
        }


        //handlers halper for button and other elements

        private void Delete_MouseEnter(object sender, MouseEventArgs e)
        {
            Label.Content = "If you click on delete button on this tab  \n- patient and all linked visit will \nbe deleted. \nFor deleting one to one select Card tab.";
        }

        private void Delete_MouseLeave(object sender, MouseEventArgs e)
        {
            Label.Content = string.Empty;
        }

        private void Create_MouseEnter(object sender, MouseEventArgs e)
        {
            Label.Content = "For Create new patient or visit - \nopen Card tab.";
        }

        private void Create_MouseLeave(object sender, MouseEventArgs e)
        {
            Label.Content = string.Empty;
        }

        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            Label.Content = "For create, edit or delete row \npatient or visit select in datagrid and \nopen Card tab for editing.\n For edit row in table select in datagrid\n open Card tab , make change and click\n on save button.";

        }

        private void Edit_MouseLeave(object sender, MouseEventArgs e)
        {
            Label.Content = string.Empty;
        }

    
    }
}

