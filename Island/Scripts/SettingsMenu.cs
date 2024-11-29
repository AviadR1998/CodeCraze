using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject outsideMenu;
    public GameObject settingsCanvas;
    public GameObject firstMenuPanel;
    public GameObject mainMenuPanel;
    public GameObject resetButton;
    public GameObject deleteButton;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDrop;
    private Resolution[] resolutions;
    private List<Resolution> goodRatioResolutions;

    public static bool activateButtons;

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
    private IEnumerator sendDeleteRequest()
    {
        UnityWebRequest request = UnityWebRequest.Delete("http://" + MainMenu.serverIp + ":5000/api/Users/Delete");
        request.SetRequestHeader("authorization", "Bearer " + Login.token);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User deleted successfully!");
        }
        else
        {
            Debug.LogError($"Error deleting user: {request.error}");
        }
    }

    IEnumerator resetUserRequest()
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Users/Reset", form))
        {
            request.SetRequestHeader("authorization", "Bearer " + Login.token);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {

            }
            else
            {
            }
        }
    }

    void OnEnable()
    {
        outsideMenu.SetActive(false);
        resetButton.SetActive(activateButtons);
        deleteButton.SetActive(activateButtons);
    }

    public void backToOutMenu()
    {
        outsideMenu.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void resetUser()
    {
        Login.world = "Forest";
        Login.task = "Swing";
        Login.state = 0;
        StartCoroutine(resetUserRequest());
    }

    public void deleteUser()
    {
        Login.world = "Forest";
        Login.task = "Swing";
        Login.state = 0;
        outsideMenu.SetActive(true);
        mainMenuPanel.SetActive(false);
        firstMenuPanel.SetActive(true);
        outsideMenu.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
        StartCoroutine(sendDeleteRequest());
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
