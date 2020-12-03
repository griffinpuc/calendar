using System;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class dbContext
    {

        private string connectionString = "server=localhost;user=pluginconnection;password=Fall2020!;database=calendar";

        public void addEvent(Event newevent)
        {

            MySqlDateTime eventDT = newevent.startHour.ToString("yyyy-MM-dd HH:mm:ss");

            using var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();

                string sql = "INSERT INTO events (event_name, event_desc, event_start, event_end) " +
                             "VALUES (@Name, @Desc, @Start, @End); ";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", newevent.eventName);
                cmd.Parameters.AddWithValue("@Desc", newevent.eventDesc);
                cmd.Parameters.AddWithValue("@ImgUrl", newevent.startHour);
                cmd.Parameters.AddWithValue("@Series", newevent.endHour);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        //public List<Event> 

    }
}
