// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NewBehaviourScript : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
using UnityEngine;

public class ActivateOnClick : MonoBehaviour
{
    public GameObject pooUpScrn;
    public GameObject question;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnMouseDown()
    {
        print("Inside");
        if (pooUpScrn != null)
        {
            print("NotNull");
            pooUpScrn.SetActive(true);
        }
        if (question != null)
        {
            question.SetActive(true);
        }
        // Destroy(gameObject, 0f);
    }

    public void OnClickBtn()
    {
        print("Yesssssssssssssss\n");
        pooUpScrn.SetActive(false);
    }
}



