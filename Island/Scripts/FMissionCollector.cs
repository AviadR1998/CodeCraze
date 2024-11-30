using UnityEngine;



//This script manage the collection of an object for the funciton mission
public class FMissionCollector : MonoBehaviour
{
    public GameObject coin;
    public GameObject inBoxObject;
    public GameObject player; // Reference to the player GameObject
    public bool isTorch = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            FunctionMissionCoin functionMissionCoin = coin.GetComponent<FunctionMissionCoin>();
            if (functionMissionCoin != null)
            {
                if (isTorch)
                {
                    functionMissionCoin.AddTorch();
                }
                else
                {
                    functionMissionCoin.AddMatch();
                }
            }

            gameObject.SetActive(false);
            gameObject.GetComponent<SoundEffects>().PlaySoundClip();
            if (inBoxObject != null)
            {
                inBoxObject.SetActive(true);
            }
        }
    }
}
