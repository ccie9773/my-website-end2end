using System.Collections.Generic;
using MyCompany.Website.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyCompany.Website.Services
{
    public interface IMyService
    {
        List<My> GetMys();
    }

    public class MyService : IMyService
    {
        private readonly IConfiguration _configuration;

        public MyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<My> GetMys()
        {
            var Mys = new List<My>();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration["SqlDatabaseConnectionString"]))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand("SELECT [Id], [Name], [Description], [Price], [ImageId] FROM [Mys]", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var My = new My()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetSqlMoney(3).ToDecimal(),
                            ImageUrl = GetImageUrl(reader.GetString(4))

                        };
                        Mys.Add(My);

                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return Mys;
        }

        private string GetImageUrl(string imageId)
        {
            var storageAccountBlobEndpoint = _configuration["StorageAccountBlobEndpoint"];
            var storageAccountImagesContainerName = _configuration["StorageAccountImagesContainerName"];           

            return $"{storageAccountBlobEndpoint}{storageAccountImagesContainerName}/{imageId}.png";
        }
    }
}
