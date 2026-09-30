using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        MetaData meta = SaveManager.Instance.CurrentMeta;
        
        if (masterSlider != null) 
        { 
            masterSlider.value = meta.masterVolume == 0 ? 1f : meta.masterVolume; 
            masterSlider.onValueChanged.AddListener(SetMasterVol); 
            SetMasterVol(masterSlider.value);
        }
        if (musicSlider != null) 
        { 
            musicSlider.value = meta.musicVolume == 0 ? 1f : meta.musicVolume; 
            musicSlider.onValueChanged.AddListener(SetMusicVol); 
            SetMusicVol(musicSlider.value);
        }
        if (sfxSlider != null) 
        { 
            sfxSlider.value = meta.sfxVolume == 0 ? 1f : meta.sfxVolume; 
            sfxSlider.onValueChanged.AddListener(SetSfxVol); 
            SetSfxVol(sfxSlider.value);
        }
    }

    private void SetMasterVol(float value)
    {
        AudioManager.Instance.SetVolume("Master_Vol", value);
        SaveManager.Instance.CurrentMeta.masterVolume = value;
        SaveManager.Instance.SaveMeta();
    }

    private void SetMusicVol(float value)
    {
        AudioManager.Instance.SetVolume("Music_Vol", value);
        SaveManager.Instance.CurrentMeta.musicVolume = value;
        SaveManager.Instance.SaveMeta();
    }

    private void SetSfxVol(float value)
    {
        AudioManager.Instance.SetVolume("Sfx_Vol", value);
        SaveManager.Instance.CurrentMeta.sfxVolume = value;
        SaveManager.Instance.SaveMeta();
    }
}
