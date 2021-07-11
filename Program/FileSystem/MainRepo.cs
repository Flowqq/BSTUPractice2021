using System.IO;
using Program.Controller;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class MainRepo
    {
        protected ICollectionDefDataSource CollectionDefDataSource;
        protected IDataUnitDataSource DataUnitDataSource;
        protected IDataUnitIndexDataSource DataUnitIndexDataSource;
        protected IndexRepository IndexRepository;
        public  CollectionDefinitionRepository CollectionDefinitionRepository { get; }
        public  DataUnitRepository DataUnitRepository { get; }

        public MainRepo()
        {
            Directory.CreateDirectory(FileSystemConfig.COLLECTIONS_FILEPATH);
            DirUtils.CreateDirsForFile(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            
            DataUnitDataSource = new DataUnitDataSource();
            DataUnitIndexDataSource = new DataUnitIndexDataSource();
            CollectionDefDataSource = new CollectionDefinitionDataSource();

            var colDefs = CollectionDefDataSource.LoadCollectionDefinitions();

            IndexRepository = new IndexRepository(colDefs, DataUnitDataSource, DataUnitIndexDataSource);
            CollectionDefinitionRepository = new CollectionDefinitionRepository(CollectionDefDataSource, IndexRepository);
            DataUnitRepository = new DataUnitRepository(DataUnitDataSource, IndexRepository);
        }
    }
}