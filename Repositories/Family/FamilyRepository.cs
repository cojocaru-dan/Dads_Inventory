using System.Collections.Generic;

namespace DadsInventory.Models
{
    public class FamilyRepository
    {
        private readonly List<FamilyMember> _familyMembers = new()
        {
            new FamilyMember() 
            { 
                Name="Dad", 
                Password="Pass#1", 
                Roles=new string[1] {"first"}
            },
            new FamilyMember()
            {
                Name="Mom",
                Password="Pass#2",
                Roles=new string[1] {"second"}
            },
        };

        public FamilyRepository() {}

        public List<FamilyMember> GetAllMembers() => _familyMembers;
        public FamilyMember GetLoggedInMember(string isLoggedIn) => _familyMembers.Find(m => m.Name == isLoggedIn);
    }
}