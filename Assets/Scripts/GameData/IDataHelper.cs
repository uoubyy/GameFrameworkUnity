using System.Collections;
using System.Collections.Generic;

namespace GameData
{
    public interface IDataHelper<T>
    {
        public void Init(string dataTableName) { }
        protected void Deserialize() { }

        public bool GetData(int key, out T value) { value = default(T);  return false; }
        public bool GetData(string key, out T value) { value = default(T);  return false; }
    }
}
