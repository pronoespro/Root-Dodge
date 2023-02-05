using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Sound {
    public class PlaySoundOnEnable : MonoBehaviour
    {

        public bool onEnabled, onDisabled;
        public string sound;
        public float soundDelay;

        private float curDelay;

        private void Update()
        {
            curDelay -= Time.deltaTime;
        }

        private void OnEnable()
        {
            if (onEnabled)
            {
                SoundManager.instance.PlayAudio(sound);
            }
            curDelay = soundDelay;
        }

        private void OnDisable()
        {
            if (onDisabled && curDelay<=0)
            {
                SoundManager.instance.PlayAudio(sound);
            }
            curDelay = soundDelay;
        }

        public void PlaySound()
        {
            SoundManager.instance.PlayAudio(sound);
        }

    }
}