using UnityEngine;

public class ArrowAboveBoat : MonoBehaviour
{
    private bool eveBeenShown = false;
    private void Start()
    {
        eveBeenShown = true;
    }

    void Update()
    {
        if (eveBeenShown && ChangeCameraFocus.isSailing)
        {
            gameObject.SetActive(false);
        }
    }
}
