using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Dropdown graphicsDropdown;
    public Toggle fullscreenButton;

    public bool startAdjust = true;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
              currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        SetVolume(PlayerPrefs.GetFloat("VolumeKey"));
        SetQuality(PlayerPrefs.GetInt("QualityKey"));
        SetResolution(PlayerPrefs.GetInt("ResolutionKey"));

        if (Screen.fullScreen == true)
        {
            fullscreenButton.isOn = true;
        }
        else
        {
            fullscreenButton.isOn = false;
        }

        startAdjust = false;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionKey", resolutionIndex);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        if (startAdjust == true)
        {
            Debug.Log("Starting up settings!");
        }
        else
        {
            AudioManager.instance.Play("ToggleSound");
        }

    }

    public void SetVolume(float volume)
    {  
        audioMixer.SetFloat("volume", volume);
        volumeSlider.value = volume;
        PlayerPrefs.SetFloat("VolumeKey", volume);
    }

    public void SetQuality(int qualityIndex)
    {   
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsDropdown.value = qualityIndex;
        PlayerPrefs.SetInt("QualityKey", qualityIndex);
        if (startAdjust == true)
        {
            Debug.Log("Starting up settings!");
        }
        else
        {
            AudioManager.instance.Play("ToggleSound");
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (startAdjust == true)
        {
            Debug.Log("Starting up settings!");
        }
        else
        {
            AudioManager.instance.Play("ToggleSound");
        }
    }

}
