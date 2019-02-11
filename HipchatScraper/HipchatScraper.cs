using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HipchatScraper
{
    public partial class HipchatScraper : Form
    {
        private Timer _statusTimer;
        private MyTraceListener _textBoxListener;

        public HipchatScraper()
        {
            InitializeComponent();
        }

        private void buttonScrape_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                LoadUsers(this.textBoxHipchatPersonalAccessToken.Text);
                LoadRooms(this.textBoxHipchatPersonalAccessToken.Text);
            });            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateCounts();
            _textBoxListener = new MyTraceListener(textBoxOutput);
            Trace.Listeners.Add(_textBoxListener);
            _statusTimer = new Timer();
            _statusTimer.Interval = 30000;            
            _statusTimer.Tick += _statusTimer_Tick;
            _statusTimer.Start();
        }

        private void UpdateCounts()
        {
            int countRooms = 0, countUsers = 0, countMessages = 0;
            using (var conn = new SqlConnection("Server=(local); Integrated Security=true; Database=HipChat"))
            {

                conn.Open();
                using (var cmd = conn.CreateCommand())
                {

                    cmd.CommandText =
                        "SELECT COUNT(1) as Count FROM Rooms (NOLOCK); SELECT COUNT(1) as Count  FROM Users (NOLOCK); SELECT COUNT(1) as Count FROM Messages (NOLOCK)";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        countRooms = reader.GetInt32(0);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        countUsers = reader.GetInt32(0);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        countMessages = reader.GetInt32(0);
                    }
                }
            }

            labelCountsOutput.Text = $@"{countRooms} Rooms, {countUsers} Users, {countMessages} Messages";
        }

        private void _statusTimer_Tick(object sender, EventArgs e)
        {
            UpdateCounts();
        }

        public static void LoadUsers(string personalAccessToken, string url = "https://api.hipchat.com/v2/user?max-results=1000&startIndex=0&include-guests=false&include-deleted=false")
        {
            var client = new HipchatClient(personalAccessToken);
            Task<HttpResponseMessage> response = client.GetAsync(url);
            response.Wait();
            Task<string> y = response.Result.Content.ReadAsStringAsync();
            y.Wait();
            var resObj = JObject.Parse(y.Result);
            JObject links = resObj.Value<JObject>("links");
            string next = links?.Value<string>("next");
            if (next != null)
            {
                LoadUsers(next);
            }

            JArray items = resObj.Value<JArray>("items");

            using (var ctx = new HipchatEntities())
            {
                foreach (var i in items)
                {
                    long userId = i.Value<long>("id");
                    string name = i.Value<string>("name");
                    string mentionName = i.Value<string>("mention_name");

                    Trace.WriteLine($"Loading user '{name}'...");

                    var user = ctx.Users.FirstOrDefault(r => r.id == userId);
                    if (user == null)
                    {
                        user = new User() { name = name, id = userId, mention_name = mentionName };
                        ctx.Users.Add(user);
                    }

                    ctx.SaveChanges();

                    Trace.AutoFlush = true;
                    LoadUserMessages(personalAccessToken, userId);
                }
            }
        }

        public static void LoadUserMessages(string personalAccessToken, long userId)
        {
            var client = new HipchatClient(personalAccessToken);
            DateTimeOffset until = DateTimeOffset.Now;

            bool hitExistingMessage = false;

            while (true)
            {
                using (var ctx = new HipchatEntities())
                {
                    Task<HttpResponseMessage> response = client.GetAsync(
                        $"/v2/user/{userId}/history?reverse=false&max-results=1000&date={until.ToUnixTimeSeconds()}");
                    response.Wait();
                    Task<string> content = response.Result.Content.ReadAsStringAsync();
                    content.Wait();
                    var obj = JsonConvert.DeserializeObject<JObject>(content.Result);
                    JArray items = (JArray)obj["items"];

                    if (items == null || items.Count == 0 || hitExistingMessage)
                    {
                        Trace.WriteLine("Done");
                        break;
                    }

                    foreach (var item in items)
                    {
                        DateTimeOffset d = DateTimeOffset.Parse(item.Value<string>("date"));
                        if (d < until)
                        {
                            until = d;
                        }

                        if (item.Value<string>("type") == "message")
                        {
                            Message msg = ConvertToMessage(item);
                            if (ctx.Messages.Any(rm => rm.id == msg.id))
                            {
                                hitExistingMessage = true;
                                continue;
                            }

                            ctx.Messages.Add(msg);
                        }

                        // This fixes the condition where one message keeps getting returned at end
                        until = until.AddSeconds(-1);
                    }

                    Trace.WriteLine($"Loaded history since {until.ToString()}");
                    ctx.SaveChanges();
                }
            }
        }


        public static void LoadRooms(string personalAccessToken, string url = "https://api.hipchat.com/v2/room?max-results=1000&startIndex=0")
        {
            var client = new HipchatClient(personalAccessToken);
            Task<HttpResponseMessage> response = client.GetAsync(url);
            response.Wait();
            Task<string> y = response.Result.Content.ReadAsStringAsync();
            y.Wait();
            var resObj = JObject.Parse(y.Result);

            JObject links = resObj.Value<JObject>("links");
            string next = links?.Value<string>("next");
            if (next != null)
            {
                LoadRooms(next);
            }

            JArray items = resObj.Value<JArray>("items");

            using (var ctx = new HipchatEntities())
            {
                foreach (var i in items)
                {
                    long roomId = i.Value<long>("id");
                    string name = i.Value<string>("name");
                    string privacy = i.Value<string>("privacy");

                    Trace.WriteLine($"Loading room '{name}'...");

                    var room = ctx.Rooms.FirstOrDefault(r => r.room_id == roomId);
                    if (room == null)
                    {
                        room = new Room() { name = name, room_id = roomId, privacy = privacy };
                        ctx.Rooms.Add(room);
                    }

                    response = client.GetAsync($"/v2/room/{room.room_id}/statistics");
                    response.Wait();

                    var content = response.Result.Content.ReadAsStringAsync();
                    content.Wait();
                    resObj = JObject.Parse(content.Result);
                    room.messages_sent = resObj.Value<long>("messages_sent");
                    if (resObj["last_active"] != null && resObj.Value<string>("last_active") != null)
                    {
                        room.last_active = DateTimeOffset.Parse(resObj.Value<string>("last_active"));
                    }

                    ctx.SaveChanges();

                    if (room.messages_sent > 0)
                    {
                        bool anyMessages = ctx.Messages.Any(rm => rm.room_id == room.room_id);

                        if (!anyMessages)
                        {
                            LoadRoomMessages(personalAccessToken, roomId);
                        }
                        else
                        {
                            DateTimeOffset latestMessage = ctx.Messages.Where(rm => rm.room_id == room.room_id).Max(m => m.date);
                            if (latestMessage < room.last_active)
                            {
                                LoadRoomMessages(personalAccessToken, roomId);
                            }
                            else
                            {
                                Trace.WriteLine($"No new messages since '{room.last_active}'");
                            }
                        }
                    }
                }
            }
        }


        public static void LoadRoomMessages(string personalAccessToken, long roomId)
        {
            var client = new HipchatClient(personalAccessToken);
            DateTimeOffset until = DateTimeOffset.Now;

            bool hitExistingMessage = false;

            while (true)
            {
                using (var ctx = new HipchatEntities())
                {
                    Task<HttpResponseMessage> response = client.GetAsync(
                        $"/v2/room/{roomId}/history?reverse=false&max-results=1000&date={until.ToUnixTimeSeconds()}");
                    response.Wait();
                    Task<string> content = response.Result.Content.ReadAsStringAsync();
                    content.Wait();
                    var obj = JsonConvert.DeserializeObject<JObject>(content.Result);
                    JArray items = (JArray)obj["items"];

                    if (items == null || items.Count == 0 || hitExistingMessage)
                    {
                        Trace.WriteLine("Done");
                        break;
                    }

                    foreach (var item in items)
                    {
                        DateTimeOffset d = DateTimeOffset.Parse(item.Value<string>("date"));
                        if (d < until)
                        {
                            until = d;
                        }

                        if (item.Value<string>("type") == "message")
                        {
                            Message msg = ConvertToMessage(item);
                            if (ctx.Messages.Any(rm => rm.id == msg.id))
                            {
                                hitExistingMessage = true;
                                continue;
                            }

                            ctx.Messages.Add(msg);
                        }
                    }

                    Trace.WriteLine($"Loaded history since {until.ToString()}");


                    // This fixes the condition where one message keeps getting returned at end
                    until = until.AddSeconds(-1);
                    ctx.SaveChanges();
                }
            }
        }

        private static Message ConvertToMessage(JToken item)
        {
            var msg = new Message()
            {
                id = item.Value<string>("id"),
                date = DateTimeOffset.Parse(item.Value<string>("date")),
                message1 = item.Value<string>("message")
            };

            if (item["room_id"] != null)
            {
                msg.room_id = item.Value<long>("room_id");
            }

            JObject file = (JObject)item["file"];
            if (file != null)
            {
                msg.file_name = file.Value<string>("name");
                msg.file_size = file.Value<long>("size");
                msg.file_url = file.Value<string>("url");
            }

            if (item["from"] is JObject)
            {
                JObject fromUser = (JObject)item["from"];
                if (fromUser != null)
                {
                    msg.from_id = fromUser.Value<long>("id");
                    msg.from_name = fromUser.Value<string>("name");
                }
            }

            /* Older objects in Hipchat's API used a string here*/
            if (item["from"] is JValue)
            {
                string from = item.Value<string>("from");
                msg.from_name = from;
            }

            msg.raw = item.ToString();

            return msg;
        }
    }
}
