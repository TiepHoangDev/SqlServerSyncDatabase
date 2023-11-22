using SqlServerSyncDatabase.Objects.SchemaTracking;

namespace SqlServerSyncDatabase.Tests
{
    public class ISchemaTrackingMethodsTest : ISchemaTrackingMethods
    {
        private ISchemaTrackingMethods _ISchemaTrackingMethods;

        public List<InfoChemaTableDiffObject> GetDiffSchemaTable(List<InfoChemaTableObject> source, List<InfoChemaTableObject> distination)
        {
            var result = _ISchemaTrackingMethods.GetDiffSchemaTable(source, distination);
            Assert.IsNotNull(result);
            return result;
        }

        public List<InfoChemaTableObject> GetSchemaTable(List<string> selectedTables)
        {
            var result = _ISchemaTrackingMethods.GetSchemaTable(selectedTables);
            Assert.IsNotNull(result);
            return result;
        }

        [SetUp]
        public void Init()
        {
            _ISchemaTrackingMethods = new SchemaTrackingMethods();
        }

    }
}
