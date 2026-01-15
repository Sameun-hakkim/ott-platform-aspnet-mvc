using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using OTTPlatform.Models;
using System.Data;
namespace OTTPlatform.DAL
{
    public class Login_DAL
    {
        SqlConnection _conn = null;
        SqlCommand _cmd = null;

        public static IConfiguration Configuration
        {
            get; set;
        }
        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }
  
        public Login ValidateLogin(string UserName, string Password)
        {
            Login login = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("[dbo].[SP_Login]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Option", "LOGIN_CHECK");
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                   

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            login = new Login
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserType = reader["UserType"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                password = reader["Password"].ToString()

                            };
                        }
                    }
                }
            }

            return login;
        }

        public Login ValidateSignup(string UserName)
        {
            Login login = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("[dbo].[SP_Login]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Option", "VALIDATE_SIGNUP");
                    command.Parameters.AddWithValue("@UserName", UserName);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())  
                        {
                            login = new Login
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserType = reader["UserType"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                password = reader["Password"].ToString()
                            };
                        }
                    }
                }
            }

            return login; 
        }

        public bool InsertSignUp(Login login)
        {
            int id = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Login]";
                _cmd.Parameters.AddWithValue("@Option", "INSERT_SIGNUP");
                _cmd.Parameters.AddWithValue("@UserName", login.UserName);
                _cmd.Parameters.AddWithValue("@Password", login.password);
                _conn.Open();
                id = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return id > 0 ? true : false;
        }

    }
}
