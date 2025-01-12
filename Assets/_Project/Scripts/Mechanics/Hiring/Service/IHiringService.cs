using System;
using System.Collections.Generic;
using Mechanics.Characters;

namespace Mechanics.Hiring.Service
{
    public interface IHiringService: IService
    {
        void Hire(ICharacterData characterData);
        void Hire(string characterKey);

        event Action<ICharacterData> OnHired;

        List<ICharacterData> GetAllAvailableCharactersToHire();
        List<ICharacterData> GenerateAvailableCharactersToHire();
    }
}