using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Windows;

namespace HotelReservations.Repository
{
    internal class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {
            try
            {
                var users = new List<User>();
                using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
                {
                    conn.Open();
                    var command = new SqlCommand("SELECT * FROM users", conn);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User()
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                JMBG = reader["jmbg"].ToString(),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                UserType = (UserType)Enum.Parse(typeof(UserType), reader["user_type"].ToString(), true),
                                IsActive = (bool)reader["is_active"]
                            };
                            users.Add(user);
                        }
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new CouldntLoadResourceException(ex.Message);
            }
        }

        public int Insert(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
                {
                    conn.Open();

                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO users (name, surname, jmbg, username, password, user_type, is_active)
                            OUTPUT inserted.id
                            VALUES (@name, @surname, @jmbg, @username, @password, @user_type, @is_active)
                        ";

                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@surname", user.Surname);
                        command.Parameters.AddWithValue("@jmbg", user.JMBG);
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@user_type", user.UserType.ToString());
                        command.Parameters.AddWithValue("@is_active", user.IsActive);

                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }
        public User Login(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
                {
                    conn.Open();

                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = @"
                    SELECT * FROM users WHERE username = @username AND password = @password";

                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User user = new User
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Surname = reader["surname"].ToString(),
                                    JMBG = reader["jmbg"].ToString(),
                                    Username = reader["username"].ToString(),
                                    Password = reader["password"].ToString(),
                                    UserType = (UserType)Enum.Parse(typeof(UserType), reader["user_type"].ToString(), true),
                                    IsActive = (bool)reader["is_active"]
                                };

                                return user;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }


        public void Update(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
                {
                    conn.Open();

                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE users 
                            SET name=@name, surname=@surname, jmbg=@jmbg, username=@username, password=@password, user_type=@user_type, is_active=@is_active
                            WHERE id=@id
                        ";

                        command.Parameters.AddWithValue("@id", user.Id);
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@surname", user.Surname);
                        command.Parameters.AddWithValue("@jmbg", user.JMBG);
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@user_type", user.UserType.ToString());
                        command.Parameters.AddWithValue("@is_active", user.IsActive);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public void Save(List<User> userList)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
                {
                    conn.Open();

                    foreach (var user in userList)
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.CommandText = @"
                                INSERT INTO users (name, surname, jmbg, username, password, user_type, is_active)
                                VALUES (@name, @surname, @jmbg, @username, @password, @user_type, @is_active)
                            ";
                            command.Parameters.AddWithValue("@name", user.Name);
                            command.Parameters.AddWithValue("@surname", user.Surname);
                            command.Parameters.AddWithValue("@jmbg", user.JMBG);
                            command.Parameters.AddWithValue("@username", user.Username);
                            command.Parameters.AddWithValue("@password", user.Password);
                            command.Parameters.AddWithValue("@user_type", user.UserType.ToString());
                            command.Parameters.AddWithValue("@is_active", user.IsActive);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }
    }
}
