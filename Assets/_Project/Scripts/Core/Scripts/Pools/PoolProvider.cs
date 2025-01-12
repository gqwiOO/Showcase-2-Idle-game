using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Scripts.Pools
{

    public class PoolProvider : MonoBehaviour
    {
        [SerializeField]
        private List<BasePoolObjects> _pools;


        public void Initialize()
        {
            _pools?.ForEach(x => x.Init());
        }
        
        [Button("Update pools list")]
        private void UpdatePoolsList()
        {
            _pools = new List<BasePoolObjects>();
            _pools.AddRange(GetComponentsInChildren<BasePoolObjects>());
        }
    }
}
