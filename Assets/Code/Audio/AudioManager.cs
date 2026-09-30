using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using RED.Utility.Singleton;

[System.Serializable]
public class SfxAudioList
{
    // aqui hay que agregar todos los efectos de sonido que tendra todo el juego
    [Header("UI & Menu")]
    public AudioClip uiClick;
    
    [Header("Gameplay")]
    public AudioClip gameOver;
    public AudioClip startGame;
}

[System.Serializable]
public class MusicAudioList
{
    // aqui hay que agregar todas las canciones (musica) que tendra todo el juego
    public AudioClip menuMusic;
    public AudioClip gameMusic;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer Settings")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Audio Catalogs")]
    public SfxAudioList sfxList;
    public MusicAudioList musicList;

    [Header("Music Transition")]
    public float defaultFadeTime = 1.5f;

    private List<AudioSourceHandler> sourceHandlers = new List<AudioSourceHandler>();
    private Transform sourcesRoot;

    protected override void Awake()
    {
        base.Awake();
        sourcesRoot = new GameObject("AudioSources_Pool").transform;
        sourcesRoot.SetParent(this.transform);
    }
    
    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                PlayMusicWithCrossfade(musicList.menuMusic);
                break;
                
            case GameState.Generating:
                PlaySound(SoundType.Sfx, sfxList.startGame);
                break;
                
            case GameState.Playing:
                PlayMusicWithCrossfade(musicList.gameMusic);
                break;
                
            case GameState.GameOver:
                StopAllMusic();
                PlaySound(SoundType.Sfx, sfxList.gameOver);
                break;
        }
    }
    
    public void SetVolume(string exposedParameter, float sliderValue)
    {
        float clampedValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float decibels = Mathf.Log10(clampedValue) * 20f;
        mainMixer.SetFloat(exposedParameter, decibels);
    }

    public AudioSourceHandler PlaySound(SoundType type, AudioClip clip, bool loop = false, bool persistent = false)
    {
        if (clip == null || type == SoundType.Invalid) return null;

        AudioSourceHandler handler = GetFreeHandler();
        AudioMixerGroup groupToUse = (type == SoundType.Music) ? musicGroup : sfxGroup;
        
        handler.Init(type, clip, groupToUse, persistent);
        handler.Play(loop);
        return handler;
    }
    
    public void PlayMusicWithCrossfade(AudioClip newClip, bool persistent = true)
    {
        if (newClip == null) return;
        
        foreach (var handler in sourceHandlers)
        {
            if (handler.soundType == SoundType.Music && handler.source.isPlaying)
            {
                if (handler.source.clip == newClip) return;
                
                handler.FadeOutAndStop(defaultFadeTime);
            }
        }
        
        AudioSourceHandler newHandler = GetFreeHandler();
        newHandler.Init(SoundType.Music, newClip, musicGroup, persistent);
        newHandler.FadeInAndPlay(defaultFadeTime, true);
    }

    public void StopAllMusic()
    {
        foreach (var handler in sourceHandlers)
        {
            if (handler.soundType == SoundType.Music) handler.Stop();
        }
    }

    private AudioSourceHandler GetFreeHandler()
    {
        foreach (var handler in sourceHandlers)
        {
            if (handler.IsFree()) return handler;
        }
        
        GameObject newWorker = new GameObject("AudioWorker");
        newWorker.transform.SetParent(sourcesRoot);
        AudioSourceHandler newHandler = newWorker.AddComponent<AudioSourceHandler>();
        sourceHandlers.Add(newHandler);
        
        return newHandler;
    }
}
