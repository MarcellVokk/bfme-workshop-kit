using BfmeFoundationProject.WorkshopKit.Logic;

namespace BfmeFoundationProject.WorkshopKit.Data
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
        public List<string> Dependencies;

        public bool IsBaseGame() => Guid.StartsWith("original-") || Guid.StartsWith("exp-original-");
        public string GameName() => Game == 2 ? "RotWK" : $"BFME{Game + 1}";
        public string FullName() => $"{Name} ({Version})";
        public string ExternalInstallerUrl() => Files.Any(x => x.Name == "extinst.exe") ? Files.First(x => x.Name == "extinst.exe").Url : "";

        public BfmeWorkshopEntryPreview Preview() => new BfmeWorkshopEntryPreview() { Guid = Guid, Name = Name, Version = Version, Description = Description, ArtworkUrl = ArtworkUrl, Author = Author, Owner = Owner, Game = Game, Type = Type, CreationTime = CreationTime };
        public BfmeWorkshopEntry WithCreationTimeSetToNow() => new BfmeWorkshopEntry() { Guid = Guid, Name = Name, Version = Version, Description = Description, ArtworkUrl = ArtworkUrl, Author = Author, Owner = Owner, Game = Game, Type = Type, CreationTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), Files = Files, Dependencies = Dependencies };
        public BfmeWorkshopEntry WithThisAsMetadata(BfmeWorkshopEntry metadataSource, bool inheritVersion = false, bool inheritAuthor = false) => new BfmeWorkshopEntry() { Guid = metadataSource.Guid, Name = metadataSource.Name, Version = inheritVersion ? metadataSource.Version : Version, Description = Description, ArtworkUrl = ArtworkUrl, Author = inheritAuthor ? metadataSource.Author : Author, Owner = metadataSource.Owner, Game = Game, Type = metadataSource.Type, CreationTime = CreationTime, Files = Files, Dependencies = Dependencies };

        public static async Task<BfmeWorkshopEntry> BaseGame(int game) => await BfmeWorkshopDownloadManager.Download($"original-{(game == 2 ? "RotWK" : $"BFME{game + 1}")}");
        public static async Task<BfmeWorkshopEntry> OfficialPatch(int game) => await BfmeWorkshopDownloadManager.Download($"official-{game}");
    }
}
