namespace SqlServerSyncDatabase.Objects.DataTracking
{
    public class DataTrackingMethods : IDataTrackingMethods
    {
        public List<InfoTableChangedObject> GetCHANGETABLE(string table, long fromVersion)
        {
            throw new NotImplementedException();
        }

        public long? GetCURRENT_VERSION()
        {
            throw new NotImplementedException();
        }

        public long? GetMIN_VALID_VERSION(string table)
        {
            throw new NotImplementedException();
        }

        public InfoTrackingDatabaseObject GetTrackingDatabase(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public bool SetTrackingDatabase(InfoTrackingDatabaseObject trackingDatabase)
        {
            throw new NotImplementedException();
        }
    }
}
