using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PronoesPro.UI
{
    public class SettingsController : MonoBehaviour
    {

        public float minVolume = -80, maxVolume = 20;

        public float sfxVolume = 0, musicVolume = 0;
        public Slider sfxSlider, musicSlider;

        public AudioMixer mix;

        bool loadedData;

        private void Start()
        {
            sfxVolume = GameManager.instance.AddOrGetVariable("sfx", sfxVolume);
            musicVolume = GameManager.instance.AddOrGetVariable("music", musicVolume);

            ConfigureVolumeSlider(sfxSlider);
            sfxSlider.value = sfxVolume;

            ConfigureVolumeSlider(musicSlider);
            musicSlider.value = musicVolume;

            mix.SetFloat("MusicVol", musicVolume);
            mix.SetFloat("SFXVol", sfxVolume);

            loadedData = true;
        }

        public void ConfigureVolumeSlider(Slider slider)
        {
            if (slider != null)
            {
                slider.maxValue = maxVolume;
                slider.minValue = minVolume;
            }
        }

        public void SetSFXVolume(float volume)
        {
            if (loadedData)
            {
                sfxVolume = volume;
                ApplyVolumeChanges();
            }
        }

        public void SetMusicVolume(float volume)
        {
            if (loadedData)
            {
                musicVolume = volume;
                ApplyVolumeChanges();
            }
        }

        public void ApplyVolumeChanges()
        {
            mix.SetFloat("MusicVol", musicVolume);
            mix.SetFloat("SFXVol", sfxVolume);

            GameManager.instance.SaveVariable("sfx", sfxVolume);
            GameManager.instance.SaveVariable("music", musicVolume);

            GameManager.instance.SaveGame();
        }

    }
}