using System.IO;
using System.Linq;
using Newtonsoft.Json;
using _01.EF_Mappings_Db_First;

namespace _02.Export_Characters_Players_As_JSON
{
    class ExportCharactersPlayersAsJson
    {
        static void Main()
        {
            var context = new DiabloEntities();
            string fileName = "../../characters.json";

            var characters = context.Characters
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    name = c.Name,
                    playedBy = c.UsersGames
                                .Select(ug => ug.User.Username)
                });

            var json = JsonConvert.SerializeObject(characters, 
                                            Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
    }
}
