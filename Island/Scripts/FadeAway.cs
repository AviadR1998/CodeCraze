using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    [SerializeField] public CanvasGroup Fade;
    private bool stillFade = true;

    void Update()
    {
        Invoke("FadeOut", 5f);
    }

    void FadeOut()
    {
        if (stillFade)
        {
            Fade.alpha -= Time.deltaTime;
        }
        if (Fade.alpha == 0)
        {
            Fade.gameObject.SetActive(false);
            Fade.alpha = 1;
            stillFade = false;
        }
    }

    public void setStillFade()
    {
        stillFade = true;
    }

}
