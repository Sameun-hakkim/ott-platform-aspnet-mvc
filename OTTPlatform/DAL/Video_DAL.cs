using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using OTTPlatform.Models;
using System.Data;
namespace OTTPlatform.DAL
{
    public class Video_DAL
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

        public bool InsertVideo(Videos video)
        {
            int id = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "INSERT");
                _cmd.Parameters.AddWithValue("@CategoryID", video.CategoryID);
                _cmd.Parameters.AddWithValue("@LanguageID", video.LanguageID);
                _cmd.Parameters.AddWithValue("@VideoName", video.VideoName);
                _cmd.Parameters.AddWithValue("@VideoDescription", video.VideoDescription);
                _cmd.Parameters.AddWithValue("@VideoURL", video.VideoURL);
                _cmd.Parameters.AddWithValue("@TemplateURL", video.TemplateURL);
                _conn.Open();
                id = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return id > 0 ? true : false;
        }
        public bool UpdateVideo(Videos video)
        {
            int id = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "UPDATE");
                _cmd.Parameters.AddWithValue("@Id", video.Id);
                _cmd.Parameters.AddWithValue("@CategoryID", video.CategoryID);
                _cmd.Parameters.AddWithValue("@LanguageID", video.LanguageID);
                _cmd.Parameters.AddWithValue("@VideoName", video.VideoName);
                _cmd.Parameters.AddWithValue("@VideoDescription", video.VideoDescription);
                _cmd.Parameters.AddWithValue("@VideoURL", video.VideoURL);
                _cmd.Parameters.AddWithValue("@TemplateURL", video.TemplateURL);
                _conn.Open();
                id = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return id > 0 ? true : false;
        }
        public bool DeleteVideo(int id)
        {
            int ResultID = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "DELETE");
                _cmd.Parameters.AddWithValue("@Id", id);
                _conn.Open();
                ResultID = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return ResultID > 0 ? true : false;
        }

        public bool UndoVideo(int id)
        {
            int ResultID = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "UNDO");
                _cmd.Parameters.AddWithValue("@Id", id);
                _conn.Open();
                ResultID = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return ResultID > 0 ? true : false;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _conn.Open();
                string query = "select Id,CategoryName from Category";

                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CategoryName = reader["CategoryName"].ToString()
                            });
                        }
                    }
                }

                return categories;
            }

        }

        public Category GetCategoryName(int id)
        {
            Category categories = null;

            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _conn.Open();
                string query = "select Id,CategoryName from Category where Id =" + id;

                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories = new Category
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CategoryName = reader["CategoryName"].ToString()
                            };
                        }
                    }
                }

                return categories;
            }

        }

        public Login GetLoginDetails(int id)
        {
            Login user = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            {

                string query = "SELECT Id, UserType, UserName, Password FROM LoginTable WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Login
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
            return user;

        }

        public List<Language> GetLanguage()
        {
            List<Language> Languages = new List<Language>();

            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _conn.Open();
                string query = "select Id,LanguageName from language";

                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Languages.Add(new Language
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                LanguageName = reader["LanguageName"].ToString()
                            });
                        }
                    }
                }

                return Languages;
            }

        }
        public List<Login> GetUserList()
        {
            List<Login> logins = new List<Login>();

            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _conn.Open();
                string query = "select Id,UserType,UserName,Password,UserStatus from LoginTable";

                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logins.Add(new Login
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserType = reader["UserType"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                password = reader["Password"].ToString(),
                                UserStatus = Convert.ToBoolean(reader["UserStatus"])
                            });
                        }
                    }
                }

                return logins;
            }

        }
        public List<Videos> GetVideoList(string categoryName = null, string searchQuery = null,string IsUserPage = null)
        {
            List<Videos> Video = new List<Videos>();

            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _conn.Open();

                
                string query = @"
                        SELECT v.Id, v.VideoName, v.VideoDescription, v.VideoURL, v.TemplateURL, 
                               c.CategoryName, l.LanguageName, v.VideoStatus 
                        FROM Videos v
                        JOIN Category c ON v.CategoryID = c.Id
                        JOIN Language l ON v.LanguageID = l.Id";

               
                if (!string.IsNullOrEmpty(categoryName))
                {
                    query += " WHERE c.CategoryName = @CategoryName";
                }

               
                if (!string.IsNullOrEmpty(searchQuery))
                {
                   
                    if (query.Contains("WHERE"))
                    {
                        query += " AND v.VideoName LIKE @SearchQuery";
                    }
                    else
                    {
                        query += " WHERE v.VideoName LIKE @SearchQuery";
                    }
                }
                if (!string.IsNullOrEmpty(IsUserPage))
                {
                    query += " AND v.VideoStatus = 1";
                }
               
                query += " ORDER BY v.Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    }
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%"); 
                    }

                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Video.Add(new Videos
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                VideoName = reader["VideoName"].ToString(),
                                VideoDescription = reader["VideoDescription"].ToString(),
                                VideoURL = reader["VideoURL"].ToString(),
                                TemplateURL = reader["TemplateURL"].ToString(),
                                LanguageName = reader["LanguageName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                VideoStatus = Convert.ToBoolean(reader["VideoStatus"])

                            });
                        }
                    }
                }

                return Video;
            }
        }

        public Videos GetVideoById(int id)
        {
            Videos video = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("[dbo].[SP_Videos]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Option", "SELECT");
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            video = new Videos
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                VideoName = reader["VideoName"].ToString(),
                                VideoDescription = reader["VideoDescription"].ToString(),
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                LanguageID = Convert.ToInt32(reader["LanguageID"]),
                                VideoURL = reader["VideoURL"].ToString(),
                                TemplateURL = reader["TemplateURL"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                LanguageName = reader["LanguageName"].ToString(),
                                Ratings = reader["Ratings"] != DBNull.Value ? Convert.ToInt32(reader["Ratings"]) : 0,
                                RatingsCount = reader["RatingsCount"] != DBNull.Value ? Convert.ToInt32(reader["RatingsCount"]) : 0,

                            };
                        }
                    }
                }
            }

            return video;
        }
        public bool CommentVideo(Comments comments)
        {
            int id = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_VideoComments]";
                _cmd.Parameters.AddWithValue("@Option", "INSERT_COMMENTS");
                _cmd.Parameters.AddWithValue("@UserID", comments.UserID);
                _cmd.Parameters.AddWithValue("@VideoID", comments.VideoID);
                _cmd.Parameters.AddWithValue("@CommentsName", comments.CommentsName);
                _cmd.Parameters.AddWithValue("@Ratings", comments.Ratings);


                _conn.Open();
                id = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return id > 0 ? true : false;
        }
        public List<Comments> GetComments(int id)
        {
            List<Comments> comment = new List<Comments>();


            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("[dbo].[SP_VideoComments]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Option", "SELECT_COMMENTS");
                    command.Parameters.AddWithValue("@VideoID", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                   
                        while (reader.Read())
                        {
                            comment.Add(new Comments
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                VideoID = Convert.ToInt32(reader["VideoID"]),
                                UserName = reader["UserName"].ToString(),
                                CommentsName = reader["CommentsName"].ToString(),
                                Ratings = reader["Ratings"] != DBNull.Value ? Convert.ToInt32(reader["Ratings"]) : (int?)null


                            });
                          
                        }
                    }
                }
                return comment;

            }



        }
        public bool InactiveUser(int id)
        {
            int ResultID = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "USER_INACTIVE");
                _cmd.Parameters.AddWithValue("@Id", id);
                _conn.Open();
                ResultID = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return ResultID > 0 ? true : false;
        }

        public bool ActiveUser(int id)
        {
            int ResultID = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_Videos]";
                _cmd.Parameters.AddWithValue("@Option", "USER_ACTIVE");
                _cmd.Parameters.AddWithValue("@Id", id);
                _conn.Open();
                ResultID = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return ResultID > 0 ? true : false;
        }

        public List<VideoMessage> GetVideoMessage(int CategoryId)
        {
            if (CategoryId == null || CategoryId == 0)
            {
                CategoryId = 1;
            }
            List<VideoMessage> videoMessages = new List<VideoMessage>();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("[dbo].[SP_VideoMsg]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Option", "SELECT_MESSAGE");
                    command.Parameters.AddWithValue("@CategoryId", CategoryId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            videoMessages.Add(new VideoMessage
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                UserMessage = reader["UserMessage"].ToString(),
                                MsgStatus = Convert.ToBoolean(reader["MsgStatus"])

                            });

                        }
                    }
                }
                return videoMessages;

            }



        }


        
        public bool InsertChat(VideoMessage message)
        {
            int id = 0;
            using (_conn = new SqlConnection(GetConnectionString()))
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[DBO].[SP_VideoMsg]";
                _cmd.Parameters.AddWithValue("@Option", "INSERT_MSG");
                _cmd.Parameters.AddWithValue("@UserId", message.UserId);
                _cmd.Parameters.AddWithValue("@CategoryId", message.CategoryId);
                _cmd.Parameters.AddWithValue("@UserMessage", message.UserMessage);

                _conn.Open();
                id = _cmd.ExecuteNonQuery();
                _conn.Close();


            }

            return id > 0 ? true : false;
        }

    }
}
