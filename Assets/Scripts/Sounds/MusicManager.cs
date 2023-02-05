using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MusicTracks
{
    public string name;
    public AudioClip clip;
    public float volume;
    public float pitch;
}

public class MusicManager : MonoBehaviour
{

    #region Instanciate

    public static MusicManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }

    #endregion


    public MusicTracks[] musicTracks;
    public AudioMixerGroup mixGroup;
    public int maxAudioSources=2;
    public float transitionTime=0.1f;

    private AudioSource[] sources;
    private int curAudioSource;

    private bool midChange;
    private int desiredTrack;

    private void Start()
    {
        sources = new AudioSource[maxAudioSources];
        for(int i = 0; i < maxAudioSources; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].outputAudioMixerGroup = mixGroup;
            sources[i].playOnAwake = false;
            sources[i].loop = true;
            sources[i].Stop();
        }
        sources[0].clip = musicTracks[0].clip;
        sources[0].volume = musicTracks[0].volume;
        sources[0].pitch = musicTracks[0].pitch;
        sources[0].Play();
    }

    public void ChangeTrack(string track)
    {

        if (track == "" || track == "Null")
        {

            if (!midChange)
            {
                StartCoroutine(FadeOutMusic());
            }
        }
        else
        {

            int trackNum = GetTrack(track);
            if (trackNum >= 0)
            {
                if (!midChange && (trackNum!=desiredTrack|| sources[curAudioSource].volume==0))
                {
                    StartCoroutine(ChangeTrack(trackNum));
                }
                desiredTrack = trackNum;
            }
        }
    }

    public int GetTrack(string trackName)
    {
        for(int i = 0; i < musicTracks.Length; i++)
        {
            if (musicTracks[i].name == trackName)
            {
                return i;
            }
        }
        return -1;
    }

    public IEnumerator FadeOutMusic()
    {
        midChange = true;

        float timer = 0;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            sources[curAudioSource].volume=1-timer/transitionTime;
            yield return null;
        }

        midChange = false;
    }

    public IEnumerator ChangeTrack(int desTrack)
    {
        midChange = true;

        int lastSource = curAudioSource;
        float lastSourceVol = sources[lastSource].volume;

        curAudioSource = (curAudioSource + 1) % sources.Length;

        sources[curAudioSource].clip = musicTracks[desTrack].clip;
        sources[curAudioSource].volume= musicTracks[desTrack].volume;
        sources[curAudioSource].pitch= musicTracks[desTrack].pitch;
        sources[curAudioSource].Play();

        float timer = 0;

        while (timer < transitionTime)
        {
            sources[curAudioSource].volume = (timer / transitionTime)* musicTracks[desTrack].volume;
            sources[lastSource].volume = (1 - timer / transitionTime)* lastSourceVol;

            timer += Time.deltaTime;
            yield return null;
        }
        sources[curAudioSource].volume = musicTracks[desTrack].volume;
        sources[lastSource].volume = 0f;

        midChange = false;

        if (desiredTrack != desTrack)
        {
            StartCoroutine(ChangeTrack(desiredTrack));
        }

    }

}
