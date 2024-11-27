using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDrop;
    private Resolution[] resolutions;
    private List<Resolution> goodRatioResolutions;


    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();
        goodRatioResolutions = new List<Resolution>();

        List<string> resOptions = new List<string>();

        int currResIndex = 0;
        int resCounter = 0;
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width == (16 * resolution.height / 9))
            {
                resOptions.Add(resolution.width + " x " + resolution.height);
                goodRatioResolutions.Add(resolution);

                if (resolution.width == 1920f && resolution.height == 1080f)
                {
                    currResIndex = resCounter;
                }
                resCounter++;
            }


        }
        resolutionDrop.AddOptions(resOptions);

        resolutionDrop.value = currResIndex;
        resolutionDrop.RefreshShownValue();

    }
    public void SetVolume(float volume)
    {
        print(volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = goodRatioResolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
