namespace BfmeFoundationProject.WorkshopKit.Data
{
    public struct BfmeWorkshopFile
    {
        public string Guid;
        public string Name;
        public string Url;
        public string Md5;
        public string Language;

        public static BfmeWorkshopFile MakeModFolderRedirect(string actualModFolder) => new BfmeWorkshopFile() { Guid = "mod_folder_redirect", Name = "mod_folder_redirect", Language = "", Md5 = "", Url = actualModFolder };
    }
}
