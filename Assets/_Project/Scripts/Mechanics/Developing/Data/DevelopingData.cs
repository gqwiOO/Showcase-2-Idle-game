using System;
using System.Collections.Generic;
using Mechanics.Characters;

namespace Mechanics.Developing.Data
{
    public class DevelopingData : IDevelopingData
    {
        public DevelopingData(List<ICharacterData> developers)
        {
            Developers = developers;
        }

        public List<ICharacterData> Developers { get; set; }
        public event Action<float> OnProgressUpdated;
        public float CurrentProgress { get; private set; } = 0f;
        public void AddProgress(float value)
        {
            CurrentProgress += value;
            OnProgressUpdated?.Invoke(CurrentProgress);
        }
    }
}