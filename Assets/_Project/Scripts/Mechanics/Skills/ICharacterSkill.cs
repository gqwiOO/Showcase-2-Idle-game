namespace Mechanics.Skills
{
    public interface ICharacterSkill
    {
        public string Key { get; }
        public CharacterSkillType SkillType { get; }
        public int MaxLevel { get; }
        public int CurrentLevel { get; }
    }

    public enum CharacterSkillType
    {
        None = 0,
        Soft = 1,
        Hard = 2,
    }
}