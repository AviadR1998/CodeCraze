using UnityEngine;

//This script manage the floating of an arrow object above boats objects
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
