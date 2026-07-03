using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using OtchetClient.Models;
namespace OtchetClient.ViewModels
{
    public class Connection
    {
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