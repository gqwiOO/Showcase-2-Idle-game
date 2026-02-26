using Mechanics.Characters;
using UnityEngine;

namespace Mechanics.Config
{
    [CreateAssetMenu(menuName = "GameAssets/Config/DefaultPlayerConfig", fileName = "DefaultPlayerConfig", order = 0)]
    public class DefaultPlayerConfig : ScriptableObject
    {
        [Header("New game defaults")]
        [SerializeField] private string _name = "l2fx6";
        [SerializeField] private int _age = 18;
        [SerializeField] private RoleType _role = RoleType.Handler;
        [SerializeField] private PeacefulRoleType _peacefulRole = PeacefulRoleType.Manager;
        [SerializeField] private int _salary = 0;
        [SerializeField] private CharacterSkill _skill = CharacterSkill.Expert;

        public string Name => _name;
        public int Age => _age;
        public RoleType Role => _role;
        public PeacefulRoleType PeacefulRole => _peacefulRole;
        public int Salary => _salary;
        public CharacterSkill Skill => _skill;
    }
}
