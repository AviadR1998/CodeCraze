using UnityEngine;

public class Pear : MonoBehaviour
{
    public GameObject RecursionMissionObj;
    public bool ZeroPears = false;
    private bool IsCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsCollected)
        {
            IsCollected = true;
            RecursionMission recursion = RecursionMissionObj.GetComponent<RecursionMission>();
            if (recursion != null)
            {
                recursion.CollectPear(transform, ZeroPears);
            }
            SoundEffects effects = GetComponent<SoundEffects>();
            if (effects != null)
            {
                effects.PlaySoundClip();
            }
            gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        IsCollected = false;
    }


}
