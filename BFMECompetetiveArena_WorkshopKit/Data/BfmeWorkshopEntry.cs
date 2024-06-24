using BfmeWorkshopKit.Logic;

namespace BfmeWorkshopKit.Data
{
    public struct BfmeWorkshopEntry
    {
        public string Guid;
        public string Name;
        public string Version;
        public string Description;
        public string ArtworkUrl;
        public string Author;
        public string Owner;
        public int Game;
        public int Type;
        public long CreationTime;
        public List<BfmeWorkshopFile> Files;

        public static async Task<BfmeWorkshopEntry> BaseGame(int game) => (await BfmeWorkshopQueryManager.Get($"original-{(game < 2 ? $"BFME{game + 1}" : "RotWK")}")).entry;
    }
}
