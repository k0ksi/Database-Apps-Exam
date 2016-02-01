using System.Linq;
using System.Text;
using System.Xml;
using _01.EF_Mappings_Db_First;

namespace _03.Export_Finished_Games_As_XML
{
    class ExportFinishedGamesAsXml
    {
        static void Main()
        {
            var context = new DiabloEntities();

            var finishedGames = context.Games
                .Where(g => g.IsFinished)
                .OrderBy(g => g.Name)
                .ThenBy(g => g.Duration)
                .Select(g => new
                {
                    GameName = g.Name,
                    Duration = g.Duration,
                    Users = g.UsersGames
                        .Select(ug => new
                        {   
                            UserName = ug.User.Username,
                            IpAddress = ug.User.IpAddress
                        })
                });

            string file = "../../finished-games.xml";
            Encoding encoding = Encoding.GetEncoding("utf-8");

            using (XmlTextWriter writer = new XmlTextWriter(file, encoding))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("games");
                foreach (var game in finishedGames)
                {
                    WriteGame(writer, game.GameName, game.Duration);
                    writer.WriteStartElement("users");
                    foreach (var user in game.Users)
                    {
                        WriteUser(writer, user.UserName, user.IpAddress);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
        }

        private static void WriteUser(XmlTextWriter writer, string userName, string ipAddress)
        {
            writer.WriteStartElement("user");
            writer.WriteAttributeString("username", userName);
            writer.WriteAttributeString("ip-address", ipAddress);
        }

        private static void WriteGame(XmlWriter writer, string name, int? duration)
        {
            writer.WriteStartElement("game");
            writer.WriteAttributeString("name", name);
            if (duration != null)
            {
                writer.WriteAttributeString("duration", duration.ToString());
            }
        }
    }
}
