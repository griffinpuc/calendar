using System;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Calendar.Models
{

    /*
     * Class dbContext:
     * Holds all of database functionality
     */

    public class dbContext
    {
        /* Database connection strings */
        private string connectionString = "";

        /* Add a new event to the database */
        public void addEvent(Event newevent, string sessionId)
        {

            string formattedStart = newevent.startHour.ToString("yyyy-MM-dd H:mm:ss");
            string formattedEnd = newevent.endHour.ToString("yyyy-MM-dd H:mm:ss");

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "INSERT INTO events (event_name, event_desc, event_start, event_end, event_user) " +
                             "VALUES (@Name, @Desc, @Start, @End, @SessionId); ";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", newevent.eventName);
                cmd.Parameters.AddWithValue("@Desc", newevent.eventDesc);
                cmd.Parameters.AddWithValue("@Start", formattedStart);
                cmd.Parameters.AddWithValue("@End", formattedEnd);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        /* Remove an entry from the database */
        public void removeEvent(int eventId)
        {
            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "DELETE FROM events WHERE events.event_id = @EventId;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@EventId", eventId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        /* Update an event in the database */
        public void updateEvent(Event newevent)
        {

            string formattedStart = newevent.startHour.ToString("yyyy-MM-dd H:mm:ss");
            string formattedEnd = newevent.endHour.ToString("yyyy-MM-dd H:mm:ss");

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "UPDATE events SET events.event_name = @Name, events.event_desc = @Desc WHERE events.event_id = @EventId";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", newevent.eventName);
                cmd.Parameters.AddWithValue("@Desc", newevent.eventDesc);
                cmd.Parameters.AddWithValue("@EventId", newevent.eventId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        /* Get all events for a given session id and time range */
        public List<Event> getEvents(string sessionId, DateTime fromTime, DateTime toTime)
        {
            List<Event> retval = new List<Event>();

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM events WHERE events.event_start BETWEEN @fromTime AND @toTime AND events.event_user = @sessionId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@sessionId", sessionId);
                cmd.Parameters.AddWithValue("@fromTime", fromTime);
                cmd.Parameters.AddWithValue("@toTime", toTime);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Event newevent = new Event()
                    {
                        eventId = (int)rdr["event_id"],
                        eventName = (string)rdr["event_name"],
                        eventDesc = (string)rdr["event_desc"],
                        startHour = (DateTime)rdr["event_start"],
                        endHour = (DateTime)rdr["event_end"]
                    };
                    
                    newevent.totalHours = (newevent.endHour.Hour - newevent.startHour.Hour) + 1;
                    retval.Add(newevent);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return retval;
        }

        /* Get an event given an event it */
        public Event getEvent(int eventId)
        {
            Event retval = new Event();

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM events WHERE @eventId = events.event_id;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@eventId", eventId);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    retval = new Event()
                    {
                        eventId = (int)rdr["event_id"],
                        eventName = (string)rdr["event_name"],
                        eventDesc = (string)rdr["event_desc"],
                        startHour = (DateTime)rdr["event_start"],
                        endHour = (DateTime)rdr["event_end"],
                    };

                    retval.totalHours = (retval.endHour.Hour - retval.startHour.Hour)+1;

                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return retval;
        }

        /* Get all taken hours from a day */
        public List<int> getTakenHoursDay(string sessionId, DateTime datetime)
        {
            List<int> retval = new List<int>();

            string formattedStart = datetime.ToString("yyyy-MM-dd H:mm:ss");
            string formattedEnd = datetime.ToString("yyyy-MM-dd H:mm:ss");

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM events WHERE DATE_FORMAT(@dateTime, '%y-%m-%d') = DATE_FORMAT(events.event_start, '%y-%m-%d') AND events.event_user = @sessionId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@sessionId", sessionId);
                cmd.Parameters.AddWithValue("@dateTime", formattedStart);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DateTime startDate = (DateTime)rdr["event_start"];
                    DateTime endDate = (DateTime)rdr["event_end"];

                    retval.Add(startDate.Hour);
                    retval.Add(endDate.Hour);

                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return retval;
        }

    }
}
