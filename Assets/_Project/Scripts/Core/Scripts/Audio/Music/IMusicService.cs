using Core.Scripts.Audio.Music.Data;

namespace Core.Scripts.Audio.Music
{
    public interface IMusicService
    {
        void PlayMusic(MusicData musicData, bool ignorePriority = false);
        void PlayMusic(MusicTrackKey musicTrack, bool ignorePriority = false);
        void PlayAdditiveMusic(MusicTrackKey musicTrack);
        void StopMusic();
        void StopImmediately();
        void ContinueMusic();
        void SetMusicVolume(float volume);
        void ToggleLoop(bool isLooping);
        void ToggleFadeInEffect(bool isFadeEffect);
        void ToggleFadeOutEffect(bool isFadeEffect);
    }
}