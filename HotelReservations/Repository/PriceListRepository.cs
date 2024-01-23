using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelReservations.Exceptions;
using HotelReservations.Model;

namespace HotelReservations.Repository
{
    public class PriceListRepository : IPriceListRepository
    {
        public List<Price> GetAll()
        {
            var prices = new List<Price>();
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();
                var command = new SqlCommand(@"
                    SELECT pl.*, rt.room_type_name 
                    FROM price_list pl
                    JOIN room_type rt ON pl.room_type_id = rt.room_type_id", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var price = new Price()
                        {
                            Id = (int)reader["price_id"],
                            RoomType = new RoomType
                            {
                                Id = (int)reader["room_type_id"],
                                Name = reader["room_type_name"].ToString()
                            },
                            ReservationType = (ReservationType)Enum.Parse(typeof(ReservationType), reader["reservation_type"].ToString()),
                            PriceValue = Convert.ToDouble(reader["price_value"]),
                            IsActive = (bool)reader["is_active"]
                        };
                        prices.Add(price);
                    }
                }
            }

            return prices;
        }

        public int Insert(Price price)
        {
            using (var conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = new SqlCommand(@"
                    INSERT INTO price_list (room_type_id, reservation_type, price_value, is_active)
                    OUTPUT INSERTED.price_id
                    VALUES (@RoomTypeId, @ReservationType, @PriceValue, @IsActive)", conn);

                command.Parameters.AddWithValue("@RoomTypeId", price.RoomType.Id);
                command.Parameters.AddWithValue("@ReservationType", price.ReservationType.ToString());
                command.Parameters.AddWithValue("@PriceValue", price.PriceValue);
                command.Parameters.AddWithValue("@IsActive", price.IsActive);

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Price price)
        {
            using (var conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();

                var command = new SqlCommand(@"
                    UPDATE price_list
                    SET room_type_id = @RoomTypeId, reservation_type = @ReservationType, 
                        price_value = @PriceValue, is_active = @IsActive
                    WHERE price_id = @PriceId", conn);

                command.Parameters.AddWithValue("@PriceId", price.Id);
                command.Parameters.AddWithValue("@RoomTypeId", price.RoomType.Id);
                command.Parameters.AddWithValue("@ReservationType", price.ReservationType.ToString());
                command.Parameters.AddWithValue("@PriceValue", price.PriceValue);
                command.Parameters.AddWithValue("@IsActive", price.IsActive);

                command.ExecuteNonQuery();
            }
        }

        public void Save(List<Price> priceList)
        {
            using (SqlConnection conn = new SqlConnection(Config.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    foreach (var price in priceList)
                    {
                        SqlCommand cmd;
                        if (price.Id <= 0)
                        {
                            cmd = new SqlCommand(@"
                                INSERT INTO price_list (room_type_id, reservation_type, price_value, is_active)
                                VALUES (@RoomTypeId, @ReservationType, @PriceValue, @IsActive)", conn, transaction);

                            cmd.Parameters.AddWithValue("@RoomTypeId", price.RoomType.Id);
                            cmd.Parameters.AddWithValue("@ReservationType", price.ReservationType.ToString());
                            cmd.Parameters.AddWithValue("@PriceValue", price.PriceValue);
                            cmd.Parameters.AddWithValue("@IsActive", price.IsActive);

                            price.Id = (int)cmd.ExecuteScalar();
                        }
                        else
                        {
                            cmd = new SqlCommand(@"
                                UPDATE price_list
                                SET room_type_id = @RoomTypeId, reservation_type = @ReservationType, 
                                    price_value = @PriceValue, is_active = @IsActive
                                WHERE price_id = @PriceId", conn, transaction);

                            cmd.Parameters.AddWithValue("@PriceId", price.Id);
                            cmd.Parameters.AddWithValue("@RoomTypeId", price.RoomType.Id);
                            cmd.Parameters.AddWithValue("@ReservationType", price.ReservationType.ToString());
                            cmd.Parameters.AddWithValue("@PriceValue", price.PriceValue);
                            cmd.Parameters.AddWithValue("@IsActive", price.IsActive);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
