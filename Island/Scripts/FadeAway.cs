using UnityEngine;


//This script fades a given canvas
public class FadeAway : MonoBehaviour
{
    [SerializeField] public CanvasGroup Fade;
    public float startFadeAwayAfter = 5f;
    private float noAlpha = 0f, maxAlpha = 1f;

    void Update()
    {
        Invoke("FadeOut", startFadeAwayAfter);
    }

    void FadeOut()
    {
        if (gameObject.activeInHierarchy)
        {
            Fade.alpha -= Time.deltaTime;

            if (Fade.alpha == noAlpha)
            {
                Fade.gameObject.SetActive(false);
                Fade.alpha = maxAlpha;
            }
        }

    }


}
