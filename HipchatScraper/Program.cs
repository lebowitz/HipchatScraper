using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace HipchatScraper
{
    public class Program
    {
        // get this from hipchat UI
        public static void Main()
        {
            using (var conn = new SqlConnection("Server=(local); Integrated Security=true"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = File.ReadAllText("db.sql");                    
                    cmd.ExecuteNonQuery();
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HipchatScraper());            
        }
    }
}
