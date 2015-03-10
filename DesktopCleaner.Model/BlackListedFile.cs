using System;

namespace DesktopCleaner.Model
{
    public class BlackListedFile : IDbEntry
    {
        public BlackListedFile()
        {
            UId = Guid.NewGuid();
        }

        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public Guid UId { get; set; }
    }
}