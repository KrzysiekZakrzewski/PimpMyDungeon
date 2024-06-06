using Audio.SoundsData;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio.Manager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float masterVolume = 1f;
        [SerializeField]
        private SoundCollectionSO soundCollectionSO;

        [SerializeField]
        private AudioMixerGroup musicMixerGroup;
        [SerializeField]
        private AudioMixerGroup sfxMixerGroup;

        private AudioSource currentMusic;

        private void PlayRandomSound(SoundSO[] sounds)
        {
            if(sounds == null || sounds.Length == 0)
                return;

            SoundSO soundSO = sounds[Random.Range(0,sounds.Length)];
            SoundToPlay(soundSO);
        }

        private void SoundToPlay(SoundSO soundDataSO)
        {
            AudioClip clip = soundDataSO.Clip;
            float volume = soundDataSO.Volume * masterVolume;
            float pitch = soundDataSO.Pitch;
            bool loop = soundDataSO.Loop;

            pitch = RandomizePitch(soundDataSO, pitch);

            AudioMixerGroup audioMixerGroup = GetAudioMixerGroup(soundDataSO.AudioType);

            PlaySound(clip, volume, pitch, loop, audioMixerGroup);
        }

        private float RandomizePitch(SoundSO soundDataSO, float pitch)
        {
            if (soundDataSO.RandomizePitch)
            {
                float randomPitchModifier = Random.Range(
                    -soundDataSO.RandomPitchRangeModifier,
                    soundDataSO.RandomPitchRangeModifier);

                pitch = soundDataSO.Pitch + randomPitchModifier;
            }

            return pitch;
        }

        private void PlaySound(AudioClip clip, float volume, float pitch, bool loop, AudioMixerGroup audioMixerGroup)
        {
            GameObject soundObject = new GameObject("TempAudioSource");

            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.loop = loop;
            audioSource.outputAudioMixerGroup = audioMixerGroup;

            audioSource.Play();

            if (!loop)
                Destroy(soundObject, clip.length);

            DeterminMusic(audioMixerGroup, audioSource);
        }

        private void DeterminMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
        {
            if (audioMixerGroup == musicMixerGroup)
            {
                if (currentMusic != null)
                    currentMusic.Stop();

                currentMusic = audioSource;
            }
        }

        private AudioMixerGroup GetAudioMixerGroup(AudioTypes audioType)
        {
            switch(audioType)
            {
                case AudioTypes.SFX:
                    return sfxMixerGroup;
                case AudioTypes.Music: 
                    return musicMixerGroup;
                default: 
                    return null;
            }
        }
    }
}