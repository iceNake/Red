using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType { Invalid, Sfx, Music }

public class AudioSourceHandler : MonoBehaviour
{
    public AudioSource source { get; private set; }
    public SoundType soundType { get; private set; }
    public bool isPersistent { get; private set; }
    
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Init(SoundType type, AudioClip clip, AudioMixerGroup mixerGroup, bool persistent = false)
    {
        soundType = type;
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        isPersistent = persistent;
        source.volume = 1f;
    }
    
    public bool IsFree()
    {
        return !source.isPlaying && !isPersistent && fadeCoroutine == null;
    }

    public void Play(bool loop = false)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        source.loop = loop;
        source.volume = 1f;
        source.Play();
    }

    public void Stop()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        source.Stop();
        isPersistent = false;
        fadeCoroutine = null;
    }

    public void FadeInAndPlay(float fadeTime, bool loop = true)
    {
        source.loop = loop;
        source.volume = 0f;
        source.Play();
        
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(0f, 1f, fadeTime, false));
    }

    public void FadeOutAndStop(float fadeTime)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(source.volume, 0f, fadeTime, true));
    }

    private IEnumerator FadeRoutine(float startVol, float targetVol, float duration, bool stopAtEnd)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVol, time / duration);
            yield return null;
        }
        
        source.volume = targetVol;

        if (stopAtEnd)
        {
            Stop();
        }
        fadeCoroutine = null;
    }
}
