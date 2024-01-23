using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelReservations.Exceptions;
using HotelReservations.Model;
using System.IO;
using System.Diagnostics;

namespace HotelReservations.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        public List<RoomType> GetAll()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM room_type", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<RoomType> roomTypes = new List<RoomType>();

                            while (reader.Read())
                            {
                                var roomType = new RoomType()
                                {
                                    Id = (int)reader["room_type_id"],
                                    Name = reader["room_type_name"].ToString(),
                                    Value = (int)reader["room_type_value"],
                                    NightPrice = (double)reader["night_price"],
                                    DayPrice = (double)reader["day_price"],
                                    IsActive = (bool)reader["room_type_is_active"]
                                };

                                roomTypes.Add(roomType);
                            }

                            return roomTypes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntLoadResourceException(ex.Message);
            }
        }

        public int Insert(RoomType roomType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO dbo.room_type (room_type_name, room_type_value, night_price, day_price, room_type_is_active)
                            OUTPUT inserted.room_type_id
                            VALUES (@room_type_name, @room_type_value, @night_price, @day_price, @room_type_is_active)
                        ";

                        command.Parameters.AddWithValue("@room_type_name", roomType.Name);
                        command.Parameters.AddWithValue("@room_type_value", roomType.Value);
                        command.Parameters.AddWithValue("@night_price", roomType.NightPrice);
                        command.Parameters.AddWithValue("@day_price", roomType.DayPrice);
                        command.Parameters.AddWithValue("@room_type_is_active", roomType.IsActive);

                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public void Update(RoomType roomType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE dbo.room_type 
                            SET room_type_name=@room_type_name, room_type_value=@room_type_value, night_price=@night_price, day_price=@day_price, room_type_is_active=@room_type_is_active
                            WHERE room_type_id=@room_type_id
                        ";

                        command.Parameters.AddWithValue("@room_type_id", roomType.Id);
                        command.Parameters.AddWithValue("@room_type_name", roomType.Name);
                        command.Parameters.AddWithValue("@room_type_value", roomType.Value);
                        command.Parameters.AddWithValue("@night_price", roomType.NightPrice);
                        command.Parameters.AddWithValue("@day_price", roomType.DayPrice);
                        command.Parameters.AddWithValue("@room_type_is_active", roomType.IsActive);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public void Save(List<RoomType> roomTypeList)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    foreach (var roomType in roomTypeList)
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @"
                                INSERT INTO dbo.room_type (room_type_name, room_type_value, night_price, day_price, room_type_is_active)
                                VALUES (@room_type_name, @room_type_value, @night_price, @day_price, @room_type_is_active)
                            ";
                            command.Parameters.AddWithValue("@room_type_name", roomType.Name);
                            command.Parameters.AddWithValue("@room_type_value", roomType.Value);
                            command.Parameters.AddWithValue("@night_price", roomType.NightPrice);
                            command.Parameters.AddWithValue("@day_price", roomType.DayPrice);
                            command.Parameters.AddWithValue("@room_type_is_active", roomType.IsActive);

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
