using SqlServerSyncDatabase.Objects.DataTracking;

namespace SqlServerSyncDatabase.Tests
{
    public class IDataTrackingMethodsTest : IDataTrackingMethods
    {
        private IDataTrackingMethods _IDataTrackingMethods;

        [SetUp]
        public void Init()
        {
            _IDataTrackingMethods = new DataTrackingMethods();
        }

        public List<InfoTableChangedObject> GetCHANGETABLE(string table, long fromVersion)
        {
            var result = _IDataTrackingMethods.GetCHANGETABLE(table, fromVersion);
            Assert.IsNotNull(result);
            return result;
        }

        public long? GetCURRENT_VERSION()
        {
            var result = _IDataTrackingMethods.GetCURRENT_VERSION();
            Assert.IsNotNull(result);
            return result;
        }

        public long? GetMIN_VALID_VERSION(string table)
        {
            var result = _IDataTrackingMethods.GetMIN_VALID_VERSION(table);
            Assert.IsNotNull(result);
            return result;
        }

        public InfoTrackingDatabaseObject GetTrackingDatabase(string? databaseName)
        {
            var result = _IDataTrackingMethods.GetTrackingDatabase(databaseName);
            Assert.IsNotNull(result);
            return result;
        }

        public bool SetTrackingDatabase(InfoTrackingDatabaseObject trackingDatabase)
        {
            var result = _IDataTrackingMethods.SetTrackingDatabase(trackingDatabase);
            Assert.IsNotNull(result);
            return result;
        }
    }
}
