using System.Text.Json;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly Random rnd;
        public RolesService()
        {
            rnd = new Random();
        }
        public async Task<IEnumerable<string>> GetRandomRolesFromPoolAsync(int count = 5)
        {
            try
            {
                return await Task.Run(() =>
            {
                string jsonFilePath = "Static/roles-pool.json";
                string jsonString = File.ReadAllText(jsonFilePath);
                var allRoles = JsonSerializer.Deserialize<List<string>>(jsonString);

                if (allRoles == null || allRoles.Count < 1)
                    throw new FileLoadException("File with roles is not correct");

                HashSet<int> pickedIndices = new HashSet<int>();
                List<string> randomElements = new List<string>();

                while (randomElements.Count < count)
                {
                    int index = rnd.Next(allRoles.Count);

                    if (pickedIndices.Add(index))
                        randomElements.Add(allRoles.ElementAt(index));
                }
                return randomElements;
            });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
