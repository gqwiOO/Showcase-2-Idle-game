using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/HiringConfig", fileName = "HiringConfig", order = 0)]
    public class HiringConfig : ScriptableObject
    {
        [Header("Candidates")]
        [SerializeField] private IntProperty _maxCandidatesCount = IntProperty.Constant(10);

        public int MaxCandidatesCount => _maxCandidatesCount.Value;
    }
}
