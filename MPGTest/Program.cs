using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using CsvHelper;

namespace MPGTest
{
    class Program
    {
        #region teams
        public static List<long> myTeam = new List<long> {
{ 60772  },
{ 37096  },
{ 197365 },
{ 17476  },
{ 60914  },
{ 51927  },
{ 15033  },
{ 37748  },
{ 55909  },
{ 54908  },
{ 27789  },
{ 43670  },
{ 15157  },
{ 20664  },
{ 62399  },
{ 101178 },
{ 203341 },
{ 37572  },
{ 57001  },
{ 205651 },
{ 44683  },
{ 101668 },
{ 176297 }};

        static List<long> teamYann = new List<long>
        {
            {9089  },
{12086 },
{55605 },
{98745 },
{101184},
{54469 },
{19159 },
{56979 },
{153133},
{116594},
{49013 },
{49688 },
{18073 },
{50229 },
{37605 },
{59856 },
{57134 },
{37265 },
{49696 },
{49277 },
{49207 },
{45076 }
        };

        static List<long> teamPaul = new List<long> {
{6744  },
{111234},
{59949 },
{56917 },
{19272 },
{74230 },
{57531 },
{44336 },
{14295 },
{39155 },
{110979},
{55422 },
{7958  },
{18507 },
{20467 },
{90105 },
{88498 },
{80801 },
{67527 }  };

        static List<long> teamYass = new List<long> {

{33148  },
{20310  },
{82263  },
{41270  },
{54771  },
{42593  },
{77762  },
{48717  },
{38290  },
{42786  },
{26901  },
{57249  },
{18008  },
{42774  },
{47431  },
{14664  },
{41464  },
{44699  },
{40142  },
{3773   },
{78007  },
{148225 },
{113688 },
{38419 }};


        static List<long> teamGabi = new List<long> {


    { 37915},
{
39215},
{
57328},
{51507},
{38454},
{97299},
{20658},
{103192},
{88894},
{56864},
{84583},
{93264},
{41792},
{173515},
{219352},
{92217},
{103955},
{112338}};


        static List<long> teamJean = new List<long> {


{51940},
{21205},
{20695},
{41328},
{19419},
{19188},
{37642},
{61366},
{108823},
{17878},
{59846},
{86934},
{81880},
{182156},
{18987},
{94245},
{41725},
{73426},
{20452},
{152760},
{49579},
{59779},
{39487 }};

        static List<long> teamEric = new List<long> {
            {78315},
            {12745},
            {76359},
            {38411},
            {69140},
            {58893},
            {40784},
            {58621},
            {19151},
            {43250},
            {39104},
            {74208},
            {15208},
            {56872},
            {54756},
            {41733},
            {82403},
            {54861},
            {91972},
            {61548},
            {78830},
            {66749},
            {96764},
            {49464},
            {50175},
            {93464},
            {52287},
            {39167},
            {11974 }
            };


        static List<long> teamMido = new List<long> {
{ 11334},
{18656},
{58877},
{17336},
{55459},
{83312},
{57410},
{40555},
{38439},
{80607},
{103025},
{62398},
{85971},
{84450},
{44346},
{9808},
{19760},
{42892},
{149828},
{149266 }};
        #endregion

        static void Main(string[] args)
        {
            List<Player> players = new List<Player>();

            using (var client = new HttpClient())
            {
                var calendar = GetCalendar(client);

                var savedGame = ReadFile();
                players = savedGame.players.ToList();
                //GetGamesAndUpdatePlayers(calendar.matches, client, players);

                //long numberOfGameWeek = calendar.day - 1;

                if (savedGame.day != calendar.day)
                {
                    for (long i = savedGame.day - 1; i < calendar.day; i++)
                    {
                        var calendarOfTheWeek = GetCalendar(client, i);
                        GetGamesAndUpdatePlayers(calendarOfTheWeek.matches, client, players);

                        string json = JsonConvert.SerializeObject(new Game { day = calendar.day, players = players.ToArray() });
                        File.WriteAllText(@"../../game.json", json);
                    }
                }
            }

            var plays = players.OrderBy(p => p.AverageNote);
            var myTeamRates = players.Where(p => myTeam.Any(m => m == p.idplayer));
            string myTeamJson = JsonConvert.SerializeObject( myTeamRates.ToArray() );
            File.WriteAllText(@"../../myTeam.json", myTeamJson);

            var teamYannRates = players.Where(p => teamYann.Any(m => m == p.idplayer));

            var minNote = 5;
            var position = "Defender";

            var att = plays//.Where(p => p.AverageNote > minNote && p.position == position)
                .Where(a => !teamYann.Contains(a.idplayer))
                .Where(a => !teamPaul.Contains(a.idplayer))
                .Where(a => !teamMido.Contains(a.idplayer))
                .Where(a => !teamJean.Contains(a.idplayer))
                .Where(a => !teamGabi.Contains(a.idplayer))
                .Where(a => !myTeam.Contains(a.idplayer))
                .Where(a => !teamEric.Contains(a.idplayer))
                .Where(a => !teamYass.Contains(a.idplayer));

            using (var csv = new CsvWriter(new StreamWriter("allPlayer.csv")))
            {
                csv.WriteHeader<Player>();

                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.Delimiter = ",";

                foreach (var player in plays)
                    csv.WriteRecord(player);
            }

            using (var csv = new CsvWriter(new StreamWriter("myTeam.csv")))
            {
                csv.WriteHeader<Player>();

                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.Delimiter = ",";

                foreach (var player in myTeamRates)
                    csv.WriteRecord(player);
            }

            using (var csv = new CsvWriter(new StreamWriter("allAvailablePlayer.csv")))
            {
                csv.WriteHeader<Player>();

                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.Delimiter = ",";

                foreach (var player in att)
                    csv.WriteRecord(player);
            }
        }

        private static Game ReadFile()
        {
            StreamReader sr = new StreamReader(@"../../game.json");
            string jsonString = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<Game>(jsonString);
        }

        private static void GetGamesAndUpdatePlayers(Match[] enumerable, HttpClient client, List<Player> players)
        {
            foreach (var match in enumerable)
            {
                HttpResponseMessage response =
                    client.GetAsync(string.Format("https://api.monpetitgazon.com/championship/match/{0}",
                        match.id)).Result;
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    UpdatePlayersWithNotes(jsonString, players);
                }
            }
        }

        private static MPGCalendar GetCalendar(HttpClient client, long numberOfWeek)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("https://api.monpetitgazon.com/championship/2/calendar/{0}", numberOfWeek)).Result;
            response.EnsureSuccessStatusCode();

            MPGCalendar calendar = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                calendar = JsonConvert.DeserializeObject<MPGCalendar>(jsonString);
            }

            return calendar;
        }

        private static MPGCalendar GetCalendar(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("https://api.monpetitgazon.com/championship/2/calendar/").Result;
            response.EnsureSuccessStatusCode();

            MPGCalendar calendar = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                calendar = JsonConvert.DeserializeObject<MPGCalendar>(jsonString);
            }

            return calendar;
        }

        private static void UpdatePlayersWithNotes(string jsonString, List<Player> players)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);

            ParseItems(jsonObject.Home, players);
            ParseItems(jsonObject.Away, players);
        }

        private static void ParseItems(dynamic club, List<Player> players)
        {
            foreach (var item in club.players)
            {
                var idplayer = item.Value.info.idplayer.Value;
                var lastName = item.Value.info.lastname.Value;
                var noteFinal2015 = item.Value.info.note_final_2015.Value;

                if (players.Any(p => p.idplayer == idplayer))
                {
                    var player = players.First(p => p.idplayer == idplayer);
                    player.notes.Add(noteFinal2015);
                }
                else
                {
                    players.Add(new Player()
                    {
                        idplayer = idplayer,
                        lastName = lastName,
                        position = item.Value.info.position,
                        team = club.club,
                        notes = new List<double>() { Convert.ToDouble(noteFinal2015) }
                    });
                }
            }
        }
    }

    internal class MPGCalendar
    {
        public long day { get; set; }
        public Match[] matches { get; set; }
    }

    internal class Match
    {
        public long id { get; set; }
    }
}

public class Game
{
    public long day { get; set; }
    public Player[] players { get; set; }
}

public class Player
{
    public long idplayer { get; set; }
    public string lastName { get; set; }
    public List<double> notes { get; set; }

    public string position { get; set; }

    public string team { get; set; }

    public double AverageNote { get { return notes.Average(); } }
    public int NumberOfGames { get { return notes.Count; } }

    public double tendanceLastFiveGames { get { return notes.Skip(Math.Max(0, notes.Count() - 5)).Average(); } }

    public double tendanceLastThreeGames { get { return notes.Skip(Math.Max(0, notes.Count() - 3)).Average(); } }

    public double lastGame { get { return notes.LastOrDefault(); } }
}

