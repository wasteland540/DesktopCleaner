using System.Collections.Generic;
using DesktopCleaner.Model;

namespace DesktopCleaner.DataAccessLayer
{
    public interface IDataAccessLayer
    {
        //NOTE: Parameter databaseName is for future implementation of mongoDb.
        //we also need to extend the parameters with collection name!

        void ClearDb(string databaseName);

        void Close();

        void Setup(string databaseName);

        void Delete<T>(string databaseName, T entry) where T : IDbEntry;

        void Delete<T>(string databaseName, List<T> entries) where T : IDbEntry;

        void Update<T>(string databaseName, T entry) where T : IDbEntry;

        void Update<T>(string databaseName, List<T> entries) where T : IDbEntry;

        string Insert<T>(string databaseName, T entry) where T : IDbEntry;

        List<string> Insert<T>(string databaseName, List<T> entries) where T : IDbEntry;

        T GetEntry<T>(string databaseName, string id) where T : IDbEntry;

        List<T> GetEntries<T>(string databaseName) where T : IDbEntry;

        List<T> GetEntries<T>(string databaseName, IEnumerable<string> idList) where T : IDbEntry;
    }
}