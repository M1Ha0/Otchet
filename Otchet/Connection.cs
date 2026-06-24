using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
namespace Otchet
{
    public class Connection
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Server=teacherpc;Database=Деканат;User Id=user13;Password=Aa_111111;Trusted_Connection=True;";
            string sqlExpression = "SELECT * FROM Все_Студенты";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string columnName1 = reader.GetName(0);
                    string columnName2 = reader.GetName(1);
                    string columnName3 = reader.GetName(2);

                    Console.WriteLine($"{columnName1}\t{columnName3}\t{columnName2}");

                    while (await reader.ReadAsync()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(2);
                        object age = reader.GetValue(1);

                        Console.WriteLine($"{id} \t{name} \t{age}");
                    }
                }

                await reader.CloseAsync();
            }
            Console.Read();
        }
    }
}   