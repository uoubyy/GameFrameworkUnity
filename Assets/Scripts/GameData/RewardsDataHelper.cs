using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace GameData
{
    [Serializable]
    public struct RewardInfo
    {
        public int id { get; private set; }
        public string type { get; private set; }
        public string icon { get; private set; }
        public string name { get; private set; }
        public string descr { get; private set; }
    }


    public class RewardsDataHelper: IDataHelper<RewardInfo>
    {
        private bool m_initialized = false;

        private List<RewardInfo> m_rewardsInfo;

        private string m_tableName;
        public void Init(string dataTableName)
        {
            if (m_initialized) return;

            m_tableName = dataTableName;

            m_rewardsInfo = new List<RewardInfo>();

            Deserialize();
            m_initialized = true;
        }
        protected void Deserialize()
        {
            TextAsset jsonTextFile = Resources.Load<TextAsset>(string.Format("Text/{0}", m_tableName));

            m_rewardsInfo = JsonConvert.DeserializeObject<List<RewardInfo>>(jsonTextFile.text);
        }

        public bool GetData(int key, out RewardInfo value)
        {
            foreach (var item in m_rewardsInfo)
            {
                if (item.id == key)
                {
                    value = item;
                    return true;
                }
            }
            value = default(RewardInfo);
            return false;
        }

        public bool GetData(string key, out RewardInfo value)
        {
            foreach (var item in m_rewardsInfo)
            {
                if (item.name == key)
                {
                    value = item;
                    return true;
                }
            }
            value = default(RewardInfo);
            return false;
        }
    }
}
