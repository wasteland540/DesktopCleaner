using System;

namespace DesktopCleaner.Model
{
    public class BlackListedDirectory : IDbEntry
    {
        public BlackListedDirectory()
        {
            UId = Guid.NewGuid();
        }

        public string Name { get; set; }
        public Guid UId { get; set; }
    }
}