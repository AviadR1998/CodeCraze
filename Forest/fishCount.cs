using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using TMPro;
using System;

public class fishCount : MonoBehaviour
{
    public GameObject arrow;
    public GameObject target;

    public GameObject playerCamera;

    public GameObject canvas;

    public GameObject endcanvas;

    public GameObject animationCanvas;


    public TMP_Text explainWorlds;

    public TMP_Text explainCode;

    public TMP_Text animationCode;
       

    bool flag = false;

    public GameObject[] fish;

    int i = 0;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            explainWorlds.text = "Meet Count! Count is a special number that loves to keep track of things.\n Imagine Count starts at 0. So, we say: count = 0;\n " +
            "This means Count is holding the number 0 right now.\n Every time we write count++, Count gets one more.\n It's like giving Count an extra toy to hold! ";

            explainCode.text = "int main() {\n \tint fish = 1;  // Starting with 1 fish in the lake.\n \tfish++;  // Now fish is 2 so we have two fish in the lake..\n \tfish++; // Now fish is 3 so we have three fish in the lake..\n \treturn 0;.\n";
        }

    }
    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        flag = true;
        animationCode.text = "Press A and look at the fish in the lake.";
        animationCanvas.SetActive(true);

    }


    void Update()
    {
        if (arrow.active)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position,
                GameObject.Find("FirstPersonController").transform.position + new Vector3(
                playerCamera.transform.forward.x * 5,
                math.sin(playerCamera.transform.forward.y) * 5 + 2.7f,
                playerCamera.transform.forward.z * 5), 6 * Time.deltaTime);
            arrow.transform.LookAt(target.transform);
        }

        if (flag == true)
        {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (i <= 3)
                    {
                        fish[i].SetActive(true);
                        i++;
                    }
                }
        }
        if (i == 4)
        {
            animationCanvas.SetActive(false);
            endcanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                endcanvas.SetActive(false);
                i++;
            }
        }

    }
}
