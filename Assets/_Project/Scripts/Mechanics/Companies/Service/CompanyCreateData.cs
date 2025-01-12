namespace Mechanics.Companies
{
    public class CompanyCreateData
    {
        public string Name { get; }
        // public Sprite Sprite { get; }
        public string OwnerKey { get; }

        public CompanyCreateData(string name, /*string sprite,*/ string ownerKey)
        {
            Name = name;
            // Sprite = sprite;
            OwnerKey = ownerKey;
        }
    }
}