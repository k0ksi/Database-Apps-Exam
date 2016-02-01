using System;
using System.Linq;

namespace _01.EF_Mappings_Db_First
{
    class EfMappingsDbFirst
    {
        static void Main()
        {
            var context = new DiabloEntities();
            var characters = context.Characters
                .Select(c => c.Name);

            foreach (var character in characters)
            {
                Console.WriteLine(character);
            }
        }
    }
}
