using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using OtchetClient.Models;
using OtchetClient.ViewModels;
using System.Reflection.Metadata;
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
        string _connectionString = "Data Source=teacherpc;Initial Catalog=Деканат;User ID=user13;Password=Aa_111111;Encrypt=False";
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
       
        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (GroupComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите группу из списка!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedGroup = (All_Groups)GroupComboBox.SelectedItem;

            DateTime? startDate = DpStart.SelectedDate;
            DateTime? endDate = DpEnd.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Заполните диапазон дат", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                List<AbsenceReportRow> reportData = GetAbsenceReport(selectedGroup.IdGroup, startDate.Value, endDate.Value);
                DgReport.ItemsSource = reportData;

                if (reportData.Count == 0)
                {
                    MessageBox.Show("Пропусков за указанный период не найдено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<AbsenceReportRow> GetAbsenceReport(int groupId, DateTime start, DateTime end)
        {
            var result = new List<AbsenceReportRow>();

            string query = @"
                SELECT 
                    dbo.Все_Студенты.Имя AS StudentName,
                    dbo.Все_Студенты.Фамилия AS StudentSurname,
                    dbo.ЖурналДаты.Дата AS AbsenceDate,
                    dbo.ЖурналДаты.НомерЧаса AS NumPara,
                    dbo.ЖурналЗначения.Значение AS StatusName
                FROM dbo.ЖурналДанные
                JOIN dbo.Все_Студенты ON dbo.ЖурналДанные.КодСтудента = dbo.Все_Студенты.Код
                JOIN dbo.ЖурналДаты ON dbo.ЖурналДанные.КодДаты = dbo.ЖурналДаты.Код
                JOIN dbo.ЖурналЗначения ON dbo.ЖурналДанные.КодЗначения = dbo.ЖурналЗначения.Код
                WHERE Все_Студенты.Код_Группы = @GroupId
                  AND ЖурналДаты.Дата BETWEEN @StartDate AND @EndDate
                  AND ЖурналЗначения.Значение = 'Н'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", groupId);
                    command.Parameters.AddWithValue("@StartDate", start);
                    command.Parameters.AddWithValue("@EndDate", end);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new AbsenceReportRow
                            {
                                StudentName = reader["StudentName"].ToString()!,
                                StudentSurname = reader["StudentSurname"].ToString()!,
                                Date = Convert.ToDateTime(reader["AbsenceDate"]),
                                NumPara = (int)reader["NumPara"],
                                Status = reader["StatusName"].ToString()!
                            });
                        }
                    }
                }
            }

            return result;
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