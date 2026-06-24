using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using OtchetClient.Models;
namespace OtchetClient
{
    public class Connection
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Data Source=teacherpc;Initial Catalog=Деканат;User ID=user13;Password=Aa_111111;Encrypt=False";
            string sqlExpression = "SELECT * FROM Все_Студенты";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {

                    while (await reader.ReadAsync()) // построчно считываем данные
                    {

                    }
                }

                await reader.CloseAsync();
            }
            Console.Read();
        }
    }
}   