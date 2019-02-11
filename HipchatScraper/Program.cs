using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
