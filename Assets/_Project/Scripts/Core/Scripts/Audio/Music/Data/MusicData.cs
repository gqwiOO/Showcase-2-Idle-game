using System.Collections.Generic;
using Core.Scripts.Audio.Music.Asset;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Scripts.Audio.Music.Data
{
    public class MusicData
    {
        
        
        [field: SerializeField]
        public AudioClip Clip { get; set; }
        
        [field: SerializeField]
        public float Volume { get; set; }
        
        [field: SerializeField]
        public bool IsLooping { get; set; } = false;

        [field: SerializeField]
        public MusicTrackKey MusicTrack { get; set; }

        [field: SerializeField]
        [field: Range(0, 1)]
        public float ClipEndThreshold { get; set; } = 1;

        [field: SerializeField] 
        public bool PlayImmediately { get; private set; }

        [Header("Fade IN/OUT")]
        [field: SerializeField] [field: ShowIf(nameof(IsFadeInEffect))] [field: HideIf(nameof(PlayImmediately))]
        public bool IsFadeInEffect { get; set; } = true;

        [field: SerializeField] [field: ShowIf(nameof(IsFadeOutEffect))] [field: HideIf(nameof(PlayImmediately))]
        public bool IsFadeOutEffect { get; set; } = true;

        [field: SerializeField] [field: ShowIf(nameof(IsFadeInEffect))] [field: HideIf(nameof(PlayImmediately))]
        public float FadeInDuration { get; set; } = 0.25f;

        [field: SerializeField] [field: ShowIf(nameof(IsFadeOutEffect))] [field: HideIf(nameof(PlayImmediately))]
        public float FadeOutDuration { get; set; } = 0.25f;
        
        [field: SerializeField]
        public AudioMixerGroup Output { get; set; }

        [Header("AdditiveMusic")]
        [field: SerializeField]
        public bool IsAdditiveMusic { get; set; } = false;

        [field: SerializeField]
        public int Priority { get; set; } = 5;

        [field: SerializeField]
        public bool IsRandomNext { get; set; } = true;

        [field: SerializeField] [field: ShowIf(nameof(IsRandomNext))]
        public List<MusicDataAsset> NextSongs { get; private set; }

        
    }
}