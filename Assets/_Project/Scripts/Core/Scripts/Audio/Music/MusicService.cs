using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Extentions;
using Core.Scripts.Audio.Music.Asset;
using Core.Scripts.Audio.Music.Data;
using Core.Scripts.Pools;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Core.Scripts.Audio.Music
{
    public class MusicService : MonoBehaviour, IMusicService
    {
        [SerializeField] private bool debugMode;
        
        [Header("Main Parameters")]
        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource musicSource_1;

        [SerializeField]
        private float durationChangeMusic = 0.5f;

        [SerializeField]
        private PoolGameObjects audioSourcePool;

        [FormerlySerializedAs("musicData")]
        [SerializeField]
        [Header("Music Tracks")]
        private List<MusicDataAsset> musicDatas = new();
        
        private MusicData _currentMusic;
        private Tween _changeMusicTween;
        private CancellationTokenSource _checkMusicProgressCancellationTokenSource;

        private AudioSource _currentAudioSource;
        
        private bool _isStopped = true;

        private AudioSource MusicSourceFadeIn 
            => _currentAudioSource == musicSource ? musicSource_1 : musicSource; 

        private void Awake()
        {
            InitializeMusicTracks();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _changeMusicTween?.Kill();
        }

        private void InitializeMusicTracks()
        {
            _isStopped = true;
        }

        private MusicDataAsset GetMusicAsset(MusicTrackKey key) => 
            musicDatas.Where(asset => asset.MusicData.MusicTrack == key).ToList().PickRandom();
        
        private async Task CheckMusicProgress()
        {
            while (!_isStopped)
            {
                if (_currentAudioSource.clip != null && _currentMusic != null)
                {
                    float currentTime = _currentAudioSource.time;
                    float clipLength = _currentAudioSource.clip.length;
                    bool isPlaying = _currentAudioSource.isPlaying;

                    if (isPlaying && currentTime / clipLength >= _currentMusic.ClipEndThreshold)
                    {
                        HandleClipEnding();
                    }
                    else if (!isPlaying)
                    {
                        HandleClipEnding();
                    }
                } 

                // await UniTask.Delay(1,
                    // ignoreTimeScale: true,
                    // cancellationToken: _checkMusicProgressCancellationTokenSource.Token);
            }
        }

        private void EnsureCheckMusicProgressIsRunning()
        {
            if (_checkMusicProgressCancellationTokenSource == null && !_isStopped)
            {
                _checkMusicProgressCancellationTokenSource = new();
                // CheckMusicProgress().Forget();
            }
        }

        private void HandleClipEnding()
        {
            if (_currentMusic.IsLooping || !_currentMusic.IsRandomNext)
            {
                return;
            }
            PlayRandomMusic(_currentMusic);
        }

        public void PlayMusic(MusicData musicData, bool ignorePriority = false)
        {
            if (_currentAudioSource == null)
                _currentAudioSource = musicSource;
            
            if (musicData == null)
            {
                if(debugMode)
                    Debug.LogWarning("MusicManager: Attempted to play null MusicData.");
                return;
            }
            
            if(debugMode)
                Debug.Log($"[MusicManager] Started transition to {musicData.Clip.name}");

            if (!ignorePriority)
            {
                if (_currentMusic != null && _currentMusic.Priority > musicData.Priority)
                {
                    if(debugMode)
                        Debug.Log("[MusicManager] New music didn't play because current music has higher priority");
                    return;
                }
            }
            
            StopCheckMusicProgressCoroutine();

            _isStopped = false;

            ChangeMusicAnimationAsync(musicData);
        }

        public void PlayMusic(MusicTrackKey musicTrack, bool ignorePriority = false)
        {
            var musicDataAsset = GetMusicAsset(musicTrack);
            if (musicDataAsset != null)
            {
                PlayMusic(musicDataAsset.MusicData, ignorePriority);
                return;
            }
            if(debugMode)
                Debug.LogWarning($"MusicManager: Music track {musicTrack} not found.");
        }

        public void PlayRandomMusic(MusicData currentMusic)
        {
            List<MusicDataAsset> availableTracks = currentMusic.NextSongs;

            if (availableTracks.Count > 0)
            {
                PlayMusic(availableTracks[Random.Range(0, availableTracks.Count)].MusicData);
                return;
            }
            if(debugMode)
                Debug.LogWarning("MusicManager: No other tracks available to play.");
            if (_currentMusic != null)
            {
                PlayMusic(_currentMusic); // Replay current track if no other options
            }
        }

        public void PlayAdditiveMusic(MusicTrackKey musicTrack)
        {
            var musicDataAsset = GetMusicAsset(musicTrack);
            if (musicDataAsset != null)
            {
                if(debugMode)
                    Debug.LogWarning($"MusicManager: Music track {musicTrack} not found.");
                return;
            }

            AudioSource availableSource = FindAvailableAudioSource() ?? FadeOutOldestAudioSource();
            PlayAdditiveMusic(availableSource, musicDataAsset.MusicData);
        }

        private void PlayAdditiveMusic(AudioSource source, MusicData music)
        {
            source.clip = music.Clip;
            source.outputAudioMixerGroup = music.Output;
            source.loop = music.IsLooping;
            source.volume = music.IsFadeInEffect ? 0 : music.Volume;
            source.priority = music.Priority;

            source.Play();

            if (music.IsFadeInEffect)
            {
                float fadeInDuration = music.FadeInDuration > 0 ? music.FadeInDuration : durationChangeMusic / 2;
                source.DOFade(music.Volume, fadeInDuration).SetEase(Ease.Linear);
            }

            if (music.IsFadeOutEffect)
            {
                _ = FadeOutMusicAsync(source, music, CancellationToken.None);
            }
        }

        private AudioSource FindAvailableAudioSource()
        {
            return audioSourcePool.ObjectQueue
                .Select(obj => obj.GetOwner<AudioSource>())
                .FirstOrDefault(source => !source.isPlaying);
        }

        private AudioSource FadeOutOldestAudioSource()
        {
            AudioSource source = audioSourcePool.Pull().GetOwner<AudioSource>();
            source.DOFade(0, durationChangeMusic / 2).SetEase(Ease.Linear)
                .OnComplete(() => {
                    source.Stop();
                    audioSourcePool.Push(source.gameObject.GetComponent<IPoolObject>());
                });
            return source;
        }

        private async Task FadeOutMusicAsync(AudioSource audioSource, MusicData music, CancellationToken cancellationToken)
        {
            float fadeOutDuration = music.FadeOutDuration > 0 ? music.FadeOutDuration : durationChangeMusic / 2;
            try
            {
                await audioSource.DOFade(0, fadeOutDuration).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }
            catch (OperationCanceledException)
            {
                return;
            }
            audioSource.Stop();
            audioSourcePool.Push(audioSource.gameObject.GetComponent<IPoolObject>());
        }

        public void StopMusic()
        {
            _isStopped = true;
            if (_currentAudioSource.isPlaying)
            {
                FadeOutMusic();
            }
            StopCheckMusicProgressCoroutine();
        }
        public void StopImmediately()
        {
            _isStopped = true;
            _currentAudioSource.volume = 0;
            
            StopCheckMusicProgressCoroutine();
        }

        public void ContinueMusic()
        {
            _isStopped = false;
            // if (!_currentAudioSource.isPlaying)
            // {
                _currentAudioSource.volume = 1f;
            // }

            EnsureCheckMusicProgressIsRunning();
        }

        private void StopCheckMusicProgressCoroutine()
        {
            if (_checkMusicProgressCancellationTokenSource != null)
            {
                _checkMusicProgressCancellationTokenSource.Cancel();
                _checkMusicProgressCancellationTokenSource = null;
            }
        }

        private void FadeOutMusic()
        {
            _currentAudioSource.DOFade(0, _currentMusic?.FadeOutDuration ?? durationChangeMusic).SetEase(Ease.Linear);
        }
        
        private void FadeInMusic()
        {
            _currentAudioSource.DOFade(1f, _currentMusic?.FadeOutDuration ?? durationChangeMusic).SetEase(Ease.Linear);
        }

        private async Task ChangeMusicAnimationAsync(MusicData musicData)
        {
            try
            {
                if (musicData.PlayImmediately)
                    PlayImmediately(musicData);
                else
                {
                    // PlayFadeOutEffect(_currentMusic).Forget();
                    // PlayFadeInEffect(musicData).Forget();    
                }
                
                _changeMusicTween?.Kill();
                _changeMusicTween = null;

                _currentMusic = musicData;
                _currentAudioSource = MusicSourceFadeIn;
            }
            catch (Exception ex)
            {
                if(debugMode)
                    Debug.LogError($"MusicManager: An error occurred during music change: {ex.Message}");
                // Ensure music plays even if an error occurs
                // musicSource.clip = musicData.Clip;
                // musicSource.volume = musicData.Volume;
                // musicSource.Play();
            }
            finally
            {
                EnsureCheckMusicProgressIsRunning();
            }
        }

        private void PlayImmediately(MusicData musicData)
        {
            _currentAudioSource.clip = musicData.Clip;
            _currentAudioSource.volume = musicData.Volume;
            _currentAudioSource.Play();
        }

        private async Task PlayFadeInEffect(MusicData musicData)
        {
            if (musicData.IsFadeInEffect)
            {
                float fadeInDuration = musicData.FadeInDuration > 0 ? musicData.FadeInDuration : durationChangeMusic / 2;

                MusicSourceFadeIn.volume = 0f;
                MusicSourceFadeIn.clip = musicData.Clip;
                MusicSourceFadeIn.outputAudioMixerGroup = musicData.Output;
                MusicSourceFadeIn.Play();
                var fadeInTask = MusicSourceFadeIn.DOFade(musicData.Volume, fadeInDuration).SetUpdate(true).SetEase(Ease.Linear).AsyncWaitForCompletion();

                if (await Task.WhenAny(fadeInTask, Task.Delay(TimeSpan.FromSeconds(fadeInDuration * 1.5))) != fadeInTask)
                {
                    if(debugMode)
                        Debug.LogWarning("MusicManager: Fade in operation timed out. Music might not have faded in completely.");
                    MusicSourceFadeIn.volume = musicData.Volume;
                }
            }
        }

        private async Task PlayFadeOutEffect(MusicData currentMusic)
        {
            if (_currentAudioSource.isPlaying && currentMusic != null && currentMusic.IsFadeOutEffect)
            {
                float fadeOutDuration = currentMusic.FadeOutDuration > 0 ? currentMusic.FadeOutDuration : durationChangeMusic ;

                var fadeOutTask = _currentAudioSource.DOFade(0, fadeOutDuration).SetUpdate(true).SetEase(Ease.Linear).AsyncWaitForCompletion();

                if (await Task.WhenAny(fadeOutTask, Task.Delay(TimeSpan.FromSeconds(fadeOutDuration * 1.5))) != fadeOutTask)
                {
                    if(debugMode)
                        Debug.LogWarning("MusicManager: Fade out operation timed out. Proceed1ing with music change.");
                }
            }
            else
            {
                _currentAudioSource.volume = 0f;
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }

        public void ToggleLoop(bool isLooping)
        {
            if (_currentMusic != null)
            {
                _currentMusic.IsLooping = isLooping;
                musicSource.loop = isLooping;
            }
        }

        public void ToggleFadeInEffect(bool isFadeEffect)
        {
            if (_currentMusic != null)
            {
                _currentMusic.IsFadeInEffect = isFadeEffect;
            }
        }

        public void ToggleFadeOutEffect(bool isFadeEffect)
        {
            if (_currentMusic != null)
            {
                _currentMusic.IsFadeOutEffect = isFadeEffect;
            }
        }
    }
}