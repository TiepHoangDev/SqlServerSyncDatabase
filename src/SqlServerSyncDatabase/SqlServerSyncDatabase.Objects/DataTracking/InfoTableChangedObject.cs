namespace SqlServerSyncDatabase.Objects.DataTracking
{
    public class InfoTableChangedObject
    {
        public long? SYS_CHANGE_VERSION { get; set; }
        public long? SYS_CHANGE_CREATION_VERSION { get; set; }
        public long? SYS_CHANGE_OPERATION { get; set; }
        public long? SYS_CHANGE_COLUMNS { get; set; }
        public long? SYS_CHANGE_CONTEXT { get; set; }
        public Guid? ID { get; set; }
    }
}
