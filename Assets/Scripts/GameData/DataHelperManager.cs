using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace GameData
{
    public class DataHelperManager : Singleton<DataHelperManager>
    {
        private RewardsDataHelper m_rewardsDataHelper;

        protected override void OnAwake()
        {
            base.OnAwake();

            m_rewardsDataHelper = new RewardsDataHelper();
            m_rewardsDataHelper.Init("RewardsInfo");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
