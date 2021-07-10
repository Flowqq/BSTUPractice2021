using System.IO;
using Program.Controller;
using Program.userInterface;

namespace Program
{
    public class Main
    {
        public FileSystemController InitDb()
        {
            Directory.CreateDirectory(FileSystemConfig.COLLECTIONS_FILEPATH);
            FileSystemConfig.CreateDirsForFile(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            var indexFileInterface = new IndexFileInterface();
            var dataUnitFileInterface = new DataUnitFileRepository();
            var collectionDefFileInterface = new CollectionDefinitionFileRepo();
            var colDefs = collectionDefFileInterface.LoadCollectionDefinitions();
            var indexController = new IndexController(colDefs, dataUnitFileInterface, indexFileInterface);
            return new FileSystemController(collectionDefFileInterface, dataUnitFileInterface, indexController);
        }
    }
}