using BfmeFoundationProject.WorkshopKit.Logic;

namespace BfmeFoundationProject.WorkshopKit.Data
{
    public struct BfmeWorkshopEntry
    {
        public string Guid;
        public string Name;
        public string Version;
        public string Description;
        public string Changelog;
        public string Language;
        public string ArtworkUrl;
        public List<string> ScreenshotUrls;
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
        public string ModFolderRedirect() => Files.Any(x => x.Guid == "mod_folder_redirect") ? Files.First(x => x.Guid == "mod_folder_redirect").Url : "";
        public long PackageSize() => Files.Sum(x => x.Size);

        public BfmeWorkshopEntryPreview Preview(BfmeWorkshopEntryMetadata metadata) => new BfmeWorkshopEntryPreview() { Guid = Guid, Name = Name, Version = Version, Description = Description, Changelog = Changelog, Language = Language, ArtworkUrl = ArtworkUrl, ScreenshotUrls = ScreenshotUrls, Author = Author, Owner = Owner, Game = Game, Type = Type, Size = PackageSize(), CreationTime = CreationTime, Metadata = metadata };
        public BfmeWorkshopEntry WithCreationTimeSetToNow() => new BfmeWorkshopEntry() { Guid = Guid, Name = Name, Version = Version, Description = Description, Changelog = Changelog, Language = Language, ArtworkUrl = ArtworkUrl, ScreenshotUrls = ScreenshotUrls, Author = Author, Owner = Owner, Game = Game, Type = Type, CreationTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), Files = Files, Dependencies = Dependencies };
        public BfmeWorkshopEntry WithThisAsBaseInfo(BfmeWorkshopEntry baseInfoSource, bool inheritVersion = false, bool inheritAuthor = false) => new BfmeWorkshopEntry() { Guid = baseInfoSource.Guid, Name = baseInfoSource.Name, Version = inheritVersion ? baseInfoSource.Version : Version, Description = Description, Changelog = Changelog, Language = Language, ArtworkUrl = ArtworkUrl, ScreenshotUrls = ScreenshotUrls, Author = inheritAuthor ? baseInfoSource.Author : Author, Owner = baseInfoSource.Owner, Game = Game, Type = baseInfoSource.Type, CreationTime = CreationTime, Files = Files, Dependencies = Dependencies };

        public static async Task<BfmeWorkshopEntry> BaseGame(int game) => await BfmeWorkshopDownloadManager.Download($"original-{(game == 2 ? "RotWK" : $"BFME{game + 1}")}");
        public static async Task<BfmeWorkshopEntry> OfficialPatch(int game) => await BfmeWorkshopDownloadManager.Download($"official-{game}");
    }
}
