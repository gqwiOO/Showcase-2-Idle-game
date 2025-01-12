using System;
using System.Collections.Generic;
using Mechanics.Characters;

namespace Mechanics.Developing.Data
{
    [Serializable]
    public class DevelopingData : IDevelopingData
    {
        public DevelopingData(List<CharacterData> developers)
        {
            Developers = developers;
        }

        public List<CharacterData> Developers { get; set; }
        public event Action<float> OnProgressUpdated;
        public float CurrentProgress { get; set; } = 0f;
        public void AddProgress(float value)
        {
            CurrentProgress += value;
            OnProgressUpdated?.Invoke(CurrentProgress);
        }
    }
}