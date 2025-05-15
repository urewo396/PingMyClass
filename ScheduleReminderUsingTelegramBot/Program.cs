using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualBasic;


class Lesson
{
    public string Subject { get; set; }
    public TimeSpan StartTime { get; set; }
}

class Program
{
    static async Task SendTelegramMessage(string msg)
    {
        string botToken = "YOUR_TELEGRAM_BOT_TOKEN_HERE";
        string chatId = "YOUR_CHAT_ID_HERE";
        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(msg)}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage res = await client.GetAsync(url);

        if (res.IsSuccessStatusCode)
        {
            Console.WriteLine("Message was sent successfully!");
        }
        else
        {
            Console.WriteLine("Failed to send message.");
        }
    }

    static async Task Main()
    {

        Dictionary<string, List<Lesson>> weeklySchedule = new Dictionary<string, List<Lesson>>();

        string path = @"PATH_TO_YOUR_CSV_SCHEDULE_RIGHT_HERE";
        string[] csv = File.ReadAllLines(path);
        foreach (string line in csv.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');
            if (parts.Length < 3) continue;
            string day = parts[0].Trim();
            string subject = parts[1].Trim();
            TimeSpan time;

            if (!TimeSpan.TryParse(parts[2].Trim(), out time))
            {
                Console.WriteLine($"Invalid time format in line: {line}");
                continue;
            }

            if (!weeklySchedule.ContainsKey(day))
            {
                weeklySchedule[day] = new List<Lesson>();
            }

            weeklySchedule[day].Add(new Lesson { Subject = subject, StartTime = time });
        }


        HashSet<string> sentToday = new HashSet<string>();

        while (true)
        {
            var now = DateTime.Now;
            string today = now.DayOfWeek.ToString();
            TimeSpan CurrentTime = now.TimeOfDay;

            if (weeklySchedule.ContainsKey(today))
            {
                foreach (var lesson in weeklySchedule[today])
                {
                    TimeSpan alertTime = lesson.StartTime - TimeSpan.FromMinutes(5);
                    string id = $"{lesson.Subject}-{lesson.StartTime}";

                    if (!sentToday.Contains(id) && CurrentTime >= alertTime && CurrentTime < alertTime + TimeSpan.FromMinutes(1))
                    {
                        string msg = $"Yo Max, {lesson.Subject} is in 5 mins ({lesson.StartTime.ToString(@"hh\:mm")})!";
                        await SendTelegramMessage(msg);
                        sentToday.Add(id);
                    }
                }
            }

            if (now.Hour == 0 && now.Minute == 0)
            {
                sentToday.Clear();
            }
        }
        await Task.Delay(60000);
    }
}
