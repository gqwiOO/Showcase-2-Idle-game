using System;
using UnityEngine;

namespace Core.Scripts.Debugging
{
    public class DebuggingContainer: MonoBehaviour
    {
        [SerializeField] private DebuggingSettings settings;

        private void Awake() 
            => Debugging.Init(settings);
    }
}