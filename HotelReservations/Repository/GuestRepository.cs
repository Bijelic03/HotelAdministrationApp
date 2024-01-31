using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Windows;

namespace HotelReservations.Repository
{
    public class GuestRepository : IGuestRepository
    {

        
        public List<Guest> GetAll()
        {
            try
            {
                var guests = new List<Guest>();
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.guest", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var guest = new Guest()
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Surname = reader["surname"].ToString(),
                                    IDNumber = reader["id_number"].ToString(),
                                    ReservationId = (int)reader["id_reservation"]
                                };

                                guests.Add(guest);
                            }
                        }
                    }
                }

                return guests;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new CouldntLoadResourceException(ex.Message);
            }
        }

        public int Insert(Guest guest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO dbo.guest (name, surname, id_number)
                            OUTPUT inserted.id
                            VALUES (@name, @surname, @id_number)
                        ";

                        command.Parameters.AddWithValue("@name", guest.Name);
                        command.Parameters.AddWithValue("@surname", guest.Surname);
                        command.Parameters.AddWithValue("@id_number", guest.IDNumber);

                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public void Update(Guest guest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE dbo.guest
                            SET name = @name, surname = @surname, id_number = @id_number
                            WHERE id = @id
                        ";

                        command.Parameters.AddWithValue("@id", guest.Id);
                        command.Parameters.AddWithValue("@name", guest.Name);
                        command.Parameters.AddWithValue("@surname", guest.Surname);
                        command.Parameters.AddWithValue("@id_number", guest.IDNumber);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public void Save(List<Guest> guestList)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    foreach (var guest in guestList)
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @"
                                INSERT INTO dbo.guest (name, surname, id_number)
                                VALUES (@name, @surname, @id_number)
                            ";

                            command.Parameters.AddWithValue("@name", guest.Name);
                            command.Parameters.AddWithValue("@surname", guest.Surname);
                            command.Parameters.AddWithValue("@id_number", guest.IDNumber);

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
