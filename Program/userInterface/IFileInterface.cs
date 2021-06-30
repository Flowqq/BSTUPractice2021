using Program.DataPage;

namespace Program.userInterface
{
    public interface IFileInterface
    {
        bool SaveCollection(Collection collection);
        bool SaveDataUnit(string collectionId, DataUnit dataUnit);
        bool UpdateDataUnit(string collectionId, DataUnit dataUnit);
        bool DeleteDataUnit(string collectionId, string dataUnitId);
        DataUnitsPaginator GetCollection(string collectionId);
    }
}