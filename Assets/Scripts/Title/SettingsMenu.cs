using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
    }
}
