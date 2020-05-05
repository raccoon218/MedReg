﻿using MedReg.Models;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            CustomInitialize();
        }

        /// <summary>
        /// custom methods bindings and handlers initializer
        /// </summary>
        private void CustomInitialize()
        {
            try
            {
                BindDataGrid(dgP, "SELECT Family, FirstName, LastName, Birthday, Snils FROM Patients ORDER BY Family ASC");
            }
            catch (Exception ex){}
        }

        /// <summary>
        /// binding data grid 
        /// </summary>
        /// <param name="dgName">data grid name</param>
        /// <param name="query">query to db</param>
        private void BindDataGrid(DataGrid dgName, string query)
        {
            var da = new SqlDataAdapter($"{query}", $@"data source =.\SQLEXPRESS; initial catalog = MedReg.Models.MedRegContext; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework");
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

        }

        /// <summary>
        /// view visits history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView rs = dg.SelectedItem as DataRowView;
            if(rs != null)
            {
                BindDataGrid(dgV, $"Select * from Visits where Snils = '{rs["Snils"]}'");
                
            }
        }
    }
}
