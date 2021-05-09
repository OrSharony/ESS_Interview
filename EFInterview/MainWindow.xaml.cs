using System;
using System.Windows;
using EFInterview.Controllers;


namespace EFInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GetDbDataController getData;
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                 getData = new GetDbDataController();
                this.DataContext = getData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void dgCustomers_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            getData.Update(103);
            getData.ManageCustomersAtRisk(60);
        }
    }
}
