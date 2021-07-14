namespace Program
{
    public class FileSystemConfig
    {
        public static readonly string FILE_EXTENSION = ".dat";
        public static readonly string COLLECTION_DEFS_FILEPATH = "db/colsDefs" + FILE_EXTENSION;
        public static readonly string COLLECTIONS_DATA_FILEPATH = "db/collections/";
        public static readonly string COLLECTION_INDEX_FILENAME = "colIndex" + FILE_EXTENSION;
        public static readonly string COLLECTION_INDEX_BACKUP_FILENAME = "colIndex_backup" + FILE_EXTENSION;
        public static readonly string DATA_FILE_POSTFIX = "_Indexed";
        public static readonly string LEFT_INDEX_POSTFIX = "L";
        public static readonly string RIGHT_INDEX_POSTFIX = "R";
    }
}