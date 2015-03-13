namespace DesktopCleaner.Application.Services
{
    public interface IFileSystemService
    {
        void CopyFiles();

        void CopyDirectories();
    }
}