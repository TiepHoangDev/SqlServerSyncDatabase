namespace SqlServerSyncDatabase.Objects.SchemaTracking
{
    public interface ISchemaTrackingMethods
    {
        List<InfoChemaTableObject> GetSchemaTable(List<string> selectedTables);
        List<InfoChemaTableDiffObject> GetDiffSchemaTable(List<InfoChemaTableObject> source, List<InfoChemaTableObject> distination);
    }
}
