namespace DesktopCleaner.Application.Services
{
    public interface IFileSystemService
    {
        //only for unit test
        void SetDesktopDirectory(string path);

        void CopyFiles();

        void CopyDirectories();
    }
}