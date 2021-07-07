using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Program.DataPage;

namespace Program.userInterface
{
    //заренеймить скорее всего
    class UserInterface : IUserInterface 
    {
        Collection collection; //не уверен, что оно надо

        public Collection CreateCollection(string collectionName)
        {
            return null;
        }

        public DataUnitsPaginator GetCollectionData(string collectionId)
        {
            return null;
        }

        public DataUnit AddDataUnit(string collectionId, DataUnit dataUnit)
        {
            return null;
        }

        public DataUnit UpdateDataUnit(string collectionId, string dataUnitId, SortedSet<DataUnitProp> updatedProps)
        {
            return null;
        }

        public bool DeleteDataUnit(string collectionId, string dataUnitId)
        {
            return true;
        }

        public DataUnitsPaginator SearchDataUnits(string collectionId, SortedSet<DataUnitProp> searchFields)
        {
            return null;
        }



    }
}
