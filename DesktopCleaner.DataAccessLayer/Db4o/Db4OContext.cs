using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using DesktopCleaner.Model;

namespace DesktopCleaner.DataAccessLayer.Db4o
{
    public class Db4OContext : IDataAccessLayer
    {
        private IEmbeddedConfiguration _configuration;
        private IObjectContainer _container;

        /// <summary>
        ///     Have to be called, if the application is shutdowning, to persist all data.
        /// </summary>
        public void Close()
        {
            _container.Close();
        }

        //only for unittests
        public void ClearDb(string databaseName)
        {
            //TODO: generisch machen für andere db einträge
            
            IList<BlackListedFile> resultList = _container.Query<BlackListedFile>();

            foreach (BlackListedFile entry in resultList)
            {
                _container.Delete(entry);
            }

            IList<BlackListedDirectory> resultList2 = _container.Query<BlackListedDirectory>();

            foreach (BlackListedDirectory entry in resultList2)
            {
                _container.Delete(entry);
            }
        }

        public void Setup(string databaseName)
        {
            _configuration = Db4oEmbedded.NewConfiguration();
            //NOTE: needed for update/delete, if references have to update/deltet to!!!
            // the cascadeOnUpdate() call must be done while the ObjectContainer
            // isn't open, so close() it, setCascadeOnUpdate, then open() it again
            //_configuration.GetType(PaymentEntry).cascadeOnUpdate(true);

            _container = Db4oFactory.OpenFile(databaseName);
        }

        public void Delete<T>(string databaseName, T entry) where T : IDbEntry
        {
            IList<T> resultList = _container.Query<T>(e => (e.UId == entry.UId));
            T result = resultList.FirstOrDefault();

            _container.Delete(result);
        }

        public void Delete<T>(string databaseName, List<T> entries) where T : IDbEntry
        {
            foreach (T entry in entries)
            {
                Delete(databaseName, entry);
            }
        }

        public void Update<T>(string databaseName, T entry) where T : IDbEntry
        {
            _container.Store(entry);
        }

        public void Update<T>(string databaseName, List<T> entries) where T : IDbEntry
        {
            foreach (T entry in entries)
            {
                Update(databaseName, entry);
            }
        }

        public string Insert<T>(string databaseName, T entry) where T : IDbEntry
        {
            _container.Store(entry);

            return entry.UId.ToString();
        }

        public List<string> Insert<T>(string databaseName, List<T> entries)
            where T : IDbEntry
        {
            var idList = new List<string>();

            foreach (T entry in entries)
            {
                _container.Store(entry);
                idList.Add(entry.UId.ToString());
            }

            return idList;
        }

        public T GetEntry<T>(string databaseName, string id) where T : IDbEntry
        {
            T result;

            IList<T> resultList = _container.Query<T>(entry => (entry.UId == new Guid(id)));
            result = resultList.FirstOrDefault();

            return result;
        }

        public List<T> GetEntries<T>(string databaseName) where T : IDbEntry
        {
            IList<T> results = _container.Query<T>();
            List<T> resultList = results.ToList();

            return resultList;
        }

        public List<T> GetEntries<T>(string databaseName, IEnumerable<string> idList)
            where T : IDbEntry
        {
            IList<T> results = _container.Query<T>(entry => idList.Contains(entry.UId.ToString()));
            List<T> resultList = results.ToList();

            return resultList;
        }
    }
}