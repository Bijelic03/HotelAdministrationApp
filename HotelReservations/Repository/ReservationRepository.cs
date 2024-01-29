using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelReservations.Exceptions;
using HotelReservations.Model;
using System.IO;
using System.Linq;
using HotelReservations.Service;

namespace HotelReservations.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private GuestService guestService;
        private RoomService roomService;
        public ReservationRepository()
        {
            roomService = new RoomService();
            guestService = new GuestService();
        }

        public List<Reservation> GetAll()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM reservation", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Reservation> reservations = new List<Reservation>();

                            while (reader.Read())
                            {
                                var reservation = new Reservation()
                                {
                                    Id = (int)reader["id"],
                                    Room = roomService.getRoomById((int)reader["room_id"]),
                                    ReservationType = (ReservationType)Enum.Parse(typeof(ReservationType), reader["reservation_type"].ToString(), true),
                                    StartDateTime = (DateTime)reader["start_date_time"],
                                    EndDateTime = (DateTime)reader["end_date_time"],
                                    TotalPrice = (double)reader["total_price"],
                                    IsActive = (bool)reader["is_active"]
                                };

                                string guestsCSV = reader["guests"].ToString();
                                List<int> guestsId = guestsCSV.Split(';')
                                                               .Select(s => int.Parse(s))
                                                               .ToList();

                                List<Guest> guests = new List<Guest>();

                                foreach (int id in guestsId)
                                {
                                    guests.Add(guestService.GetGuestById(id));
                                }

                                reservation.Guests = guests;

                                reservations.Add(reservation);
                            }

                            return reservations;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new CouldntLoadResourceException(ex.Message);
            }
        }

        public int Insert(Reservation reservation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();
                    reservation.IsActive = true;

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO reservation (room_id, reservation_type, guests, start_date_time, end_date_time, total_price, is_active)
                        OUTPUT inserted.id
                        VALUES (@room_id, @reservation_type, @guests, @start_date_time, @end_date_time, @total_price, @is_active)
                    ";

                        command.Parameters.AddWithValue("@room_id", reservation.Room.Id);
                        command.Parameters.AddWithValue("@reservation_type", reservation.ReservationType.ToString());
                        command.Parameters.AddWithValue("@guests", string.Join(";", reservation.Guests?.Select(g => g.Id) ?? Enumerable.Empty<int>()));
                        command.Parameters.AddWithValue("@start_date_time", reservation.StartDateTime);
                        command.Parameters.AddWithValue("@end_date_time", reservation.EndDateTime);
                        command.Parameters.AddWithValue("@total_price", reservation.TotalPrice);
                        command.Parameters.AddWithValue("@is_active", reservation.IsActive);

                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }





        public void Update(Reservation reservation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTION_STRING))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        UPDATE dbo.reservation 
                        SET room_id=@room_id, reservation_type=@reservation_type, guests=@guests, start_date_time=@start_date_time, 
                            end_date_time=@end_date_time, total_price=@total_price, is_active=@is_active
                        WHERE id=@id
                    ";

                        command.Parameters.AddWithValue("@id", reservation.Id);
                        command.Parameters.AddWithValue("@room_id", reservation.Room.Id);
                        command.Parameters.AddWithValue("@reservation_type", reservation.ReservationType.ToString());
                        command.Parameters.AddWithValue("@guests", string.Join(";", reservation.Guests?.Select(g => g.Id) ?? Enumerable.Empty<int>()));
                        command.Parameters.AddWithValue("@start_date_time", reservation.StartDateTime);
                        command.Parameters.AddWithValue("@end_date_time", reservation.EndDateTime);
                        command.Parameters.AddWithValue("@total_price", reservation.TotalPrice);
                        command.Parameters.AddWithValue("@is_active", reservation.IsActive);

                        command.ExecuteNonQuery();
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
