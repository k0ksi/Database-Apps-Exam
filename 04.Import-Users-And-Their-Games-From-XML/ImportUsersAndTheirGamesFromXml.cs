using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using _01.EF_Mappings_Db_First;

namespace _04.Import_Users_And_Their_Games_From_XML
{
    class ImportUsersAndTheirGamesFromXml
    {
        static void Main()
        {
            string fileName = "../../users-and-games.xml";

            var context = new DiabloEntities();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNode rootNode = xmlDocument.DocumentElement;

            foreach (XmlNode user in rootNode.ChildNodes)
            {
                try
                {
                    if (user.Attributes.Count > 0)
                    {
                        string firstName = null, lastName = null, email = null, username = null, ip = null;
                        DateTime regDate = DateTime.MinValue;
                        bool isDeleted = false;
                        User userFromDb = new User();
                        foreach (XmlAttribute attribute in user.Attributes)
                        {
                            if (attribute.Name == "first-name")
                            {
                                firstName = attribute.Value;
                            }

                            if (attribute.Name == "last-name")
                            {
                                lastName = attribute.Value;
                            }

                            if (attribute.Name == "username")
                            {
                                username = attribute.Value;
                            }

                            if (attribute.Name == "email")
                            {
                                email = attribute.Value;
                            }

                            if (attribute.Name == "ip-address")
                            {
                                ip = attribute.Value;
                            }

                            if (attribute.Name == "is-deleted")
                            {
                                isDeleted = attribute.Value == "1";
                            }

                            if (attribute.Name == "registration-date")
                            {
                                regDate = DateTime.ParseExact(attribute.Value, "dd/mm/yyyy",
                                    CultureInfo.InvariantCulture);
                            }
                        }

                        userFromDb = context.Users
                            .FirstOrDefault(u => u.Username == username);
                        if (userFromDb == null)
                        {
                            userFromDb = new User()
                            {
                                Username = username,
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                RegistrationDate = regDate,
                                IsDeleted = isDeleted,
                                IpAddress = ip
                            };
                            context.Users.Add(userFromDb);
                            Console.WriteLine("Successfully added user {0}", username);
                        }
                        else
                        {
                            Console.WriteLine("User {0} already exists", username);
                            throw new ArgumentException("User already exists");
                        }

                        foreach (XmlNode game in user.FirstChild.ChildNodes)
                        {
                            string gameName = "", characterName = "";
                            int? gameId, userId, characterId, level = null;
                            DateTime? joinedOn = null;
                            decimal? cash = null;
                            Game gameFromDb = new Game();
                            if (game["game-name"] != null)
                            {
                                gameName = game["game-name"].InnerText;
                            }
                            if (game["character"] != null)
                            {
                                foreach (XmlAttribute attribute in game["character"].Attributes)
                                {
                                    if (attribute.Name == "name")
                                    {
                                        characterName = attribute.Value;
                                    }

                                    if (attribute.Name == "cash")
                                    {
                                        cash = decimal.Parse(attribute.Value);
                                    }

                                    if (attribute.Name == "level")
                                    {
                                        level = int.Parse(attribute.Value);
                                    }
                                }
                            }

                            if (game["joined-on"] != null)
                            {
                                joinedOn = DateTime.ParseExact(game["joined-on"].InnerText, "dd/mm/yyyy",
                                    CultureInfo.InvariantCulture);
                            }

                            gameFromDb = context.Games
                                .FirstOrDefault(g => g.Name == gameName);
                            
                            if (gameFromDb == null)
                            {
                                throw new ArgumentException("No such game exists");
                            }
                            gameId = gameFromDb.Id;

                            if (userFromDb == null)
                            {
                                throw new ArgumentException("User already exists");
                            }
                            userId = userFromDb.Id;

                            Character character = context.Characters
                                .FirstOrDefault(c => c.Name == characterName);
                            characterId = character.Id;

                            context.UsersGames.Add(new UsersGame()
                            {
                                GameId = gameId.Value,
                                User = userFromDb,
                                CharacterId = characterId.Value,
                                Level = level.Value,
                                JoinedOn = joinedOn.Value,
                                Cash = cash.Value
                            });
                            Console.WriteLine("User {0} successfully added to game {1}", username, gameName);
                        }
                    }
                    context.SaveChanges();
                }
                catch (ArgumentException ex)
                {

                }
            }
        }
    }
}
