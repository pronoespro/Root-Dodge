using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PronoesPro.Sound
{

    [System.Serializable]
    public class SoundClip
    {
        public string name;
        public string[] tags;
        public AudioClip clip;
        public float volume;
        public float pitch;

        public void AddTag()
        {
            string[] newTags = new string[tags.Length + 1];
            for(int i = 0; i < tags.Length; i++)
            {
                newTags[i] = tags[i];
            }
            newTags[tags.Length] = "";
            tags = newTags;
        }

        public void RemoveTag(int index)
        {
            string[] newTags = new string[tags.Length - 1];
            int j = 0;
            for (int i = 0; i < newTags.Length; i++)
            {
                if (i == index)
                {
                    j++;
                }
                newTags[i] = tags[j];
                j++;
            }
            tags = newTags;
        }

    }

    public class SoundManager : MonoBehaviour
    {

        #region Instancing
        public static SoundManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        public int audioAmmount = 100;
        public SoundClip[] clips;
        public AudioMixerGroup mixGroup;

        private AudioSource[] sources;
        private int curAudio;

        private void Start()
        {
            sources = new AudioSource[audioAmmount];
            for(int i = 0; i < audioAmmount; i++)
            {
                sources[i] = gameObject.AddComponent<AudioSource>();
                sources[i].outputAudioMixerGroup = mixGroup;
            }
        }

        public void PlayAudio(string name)
        {
            if (sources != null && sources[curAudio]!=null)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i].name.ToLower() == name.ToLower())
                    {
                        sources[curAudio].clip = clips[i].clip;
                        sources[curAudio].volume = clips[i].volume;
                        sources[curAudio].pitch = clips[i].pitch;
                        sources[curAudio].Play();
                        CancelRelatedSounds(clips[i].tags);

                        curAudio = (curAudio + 1) % sources.Length;
                    }
                }
            }
        }

        public void CancelRelatedSounds(string[] tags)
        {
            for(int source = 0; source < sources.Length; source++)
            {
                if (source != curAudio)
                {
                    for(int i = 0; i < clips.Length; i++)
                    {
                        if (sources[source].clip == clips[i].clip)
                        {
                            foreach (string tag in tags)
                            {
                                foreach (string clipTag in clips[i].tags)
                                {
                                    if (tag == clipTag)
                                    {
                                        sources[source].Stop();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddClip()
        {
            SoundClip[] newClips = new SoundClip[clips.Length + 1];
            for(int i = 0; i < clips.Length; i++)
            {
                newClips[i] = clips[i];
            }
            newClips[clips.Length] = new SoundClip();
            newClips[clips.Length].tags = new string[0];
            clips = newClips;
        }

        public void RemoveClip(int index)
        {
            SoundClip[] newClips = new SoundClip[clips.Length - 1];
            int j = 0;
            for (int i = 0; i < newClips.Length; i++)
            {
                if (i==index)
                {
                    j++;
                }
                newClips[i] = clips[j];
                j++;
            }
            clips = newClips;
        }

    }

}