namespace BfmeFoundationProject.WorkshopKit.Data
{
    public struct BfmeWorkshopEntryPreview
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
        public BfmeWorkshopEntryMetadata Metadata;

        public string GameName() => Game == -1 ? "Unknown" : (Game == 2 ? "RotWK" : $"BFME{Game + 1}");
        public string FullName() => Version == "" ? Name : $"{Name} ({Version})";

        public BfmeWorkshopEntryPreview WithEmptyMetadata() => new BfmeWorkshopEntryPreview() { Guid = Guid, Name = Name, Version = Version, Description = Description, ArtworkUrl = ArtworkUrl, Author = Author, Owner = Owner, Game = Game, Type = Type, CreationTime = CreationTime, Metadata = new BfmeWorkshopEntryMetadata() };

        public BfmeWorkshopEntry WorkshopEntry() => new BfmeWorkshopEntry() { Guid = Guid, Name = Name, Version = Version, Description = Description, ArtworkUrl = ArtworkUrl, Author = Author, Owner = Owner, Game = Game, Type = Type, CreationTime = CreationTime, Files = [], Dependencies = [] };
    }
}
