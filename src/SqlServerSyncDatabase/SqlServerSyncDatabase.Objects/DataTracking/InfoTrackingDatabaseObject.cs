namespace SqlServerSyncDatabase.Objects.DataTracking
{
    public class InfoTrackingDatabaseObject
    {
        public List<InfoTrackingTableObject> TrackingTables { get; set; } = new();
        public string? DatabaseName { get; set; }
        public bool? EnableTracking { get; set; }
        public long? database_id { get; set; }
        public bool? is_auto_cleanup_on { get; set; }
        public long? retention_period { get; set; }
        public long? retention_period_units { get; set; }
        public long? retention_period_units_desc { get; set; }
        public long? max_cleanup_version { get; set; }
    }

}
