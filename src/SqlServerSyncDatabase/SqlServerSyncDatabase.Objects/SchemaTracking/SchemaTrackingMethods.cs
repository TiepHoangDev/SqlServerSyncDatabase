namespace SqlServerSyncDatabase.Objects.SchemaTracking
{
    public class SchemaTrackingMethods : ISchemaTrackingMethods
    {
        public List<InfoChemaTableDiffObject> GetDiffSchemaTable(List<InfoChemaTableObject> source, List<InfoChemaTableObject> distination)
        {
            throw new NotImplementedException();
        }

        public List<InfoChemaTableObject> GetSchemaTable(List<string> selectedTables)
        {
            throw new NotImplementedException();
        }
    }
}
