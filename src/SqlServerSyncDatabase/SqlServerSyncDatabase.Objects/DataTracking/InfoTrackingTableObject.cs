namespace SqlServerSyncDatabase.Objects.DataTracking
{
    public class InfoTrackingTableObject
    {
        public string? TableName { get; set; }
        public bool? EnableTracking { get; set; }
        public bool? is_track_columns_updated_on { get; set; }
        public long? object_id { get; set; }
        public long? min_valid_version { get; set; }
        public long? begin_version { get; set; }
        public long? cleanup_version { get; set; }
    }

}
