using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndWorldScript : MonoBehaviour
{
    public GameObject finalScreenCanvas;
    public static bool activated = false;

    const int ISLAND_ID = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        activated = true;
    }

    public void okButton()
    {
        PauseMenu.updateSave("Island", "Functions", 0);
        SceneManager.LoadSceneAsync(ISLAND_ID);
        finalScreenCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        activated = Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
