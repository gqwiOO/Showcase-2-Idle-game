using System;
using System.Collections.Generic;
using Mechanics.Characters;

namespace Mechanics.Developing.Data
{
    public interface IDevelopingData
    {
        List<ICharacterData> Developers { get; }
        
        event Action<float> OnProgressUpdated;
        float CurrentProgress { get; }

        void AddProgress(float value);
    }
}