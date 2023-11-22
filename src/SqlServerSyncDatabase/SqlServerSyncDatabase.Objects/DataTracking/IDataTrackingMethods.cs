using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerSyncDatabase.Objects.DataTracking
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/sql/relational-databases/track-changes/manage-change-tracking-sql-server?view=sql-server-ver16
    /// </summary>
    public interface IDataTrackingMethods
    {
        bool SetTrackingDatabase(InfoTrackingDatabaseObject trackingDatabase);
        InfoTrackingDatabaseObject GetTrackingDatabase(string? databaseName);
        long? GetCURRENT_VERSION();
        long? GetMIN_VALID_VERSION(string table);
        List<InfoTableChangedObject> GetCHANGETABLE(string table, long fromVersion);
    }
}
