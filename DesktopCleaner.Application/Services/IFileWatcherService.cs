namespace DesktopCleaner.Application.Services
{
    public interface IFileWatcherService
    {
        bool IsRunning();

        void StartWatching(string pathToDirectory);

        void StopWatching();
    }
}