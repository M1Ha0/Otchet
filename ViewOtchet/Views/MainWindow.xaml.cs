using Microsoft.Data.SqlClient;
using OtchetClient.Models;
using OtchetClient.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace OtchetClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            var groups = await GetGroupsAsync();
            GroupComboBox.ItemsSource = groups;
        }
        public static async Task<List<All_Groups>> GetGroupsAsync()
        {
            List<All_Groups> groups = new List<All_Groups>();

            string connectionString = "Data Source=teacherpc;Initial Catalog=Деканат;User ID=user13;Password=Aa_111111;Encrypt=False";
            string sqlExpression = "SELECT * FROM Все_Группы";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        All_Groups group = new All_Groups
                        {
                            IdGroup = reader.GetInt32(0),
                            NameGroup = reader.GetString(1)
                        };

                        groups.Add(group);
                    }
                }

                await reader.CloseAsync();
            }

            return groups;
        }
    }
}