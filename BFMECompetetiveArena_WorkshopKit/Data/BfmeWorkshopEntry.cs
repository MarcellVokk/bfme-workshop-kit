using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
