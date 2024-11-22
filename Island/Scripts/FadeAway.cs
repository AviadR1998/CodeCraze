using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    [SerializeField] public CanvasGroup Fade;
    public float startFadeAwayAfter = 5f;
    //private bool shouldPlaySound = true;

    void Update()
    {
        Invoke("FadeOut", startFadeAwayAfter);
    }

    void FadeOut()
    {
        if (gameObject.activeInHierarchy)
        {
            Fade.alpha -= Time.deltaTime;

            if (Fade.alpha == 0)
            {
                Fade.gameObject.SetActive(false);
                Fade.alpha = 1;
            }
        }

    }


}
