using UnityEngine;


//This script manage respawn a player when jump off statue
public class StatueLimitation : MonoBehaviour
{
    public GameObject player;
    public Transform playerRespawnPoint;
    public static bool shouldLimit = false;
    public float waterLevel = -3, respawnAfter = 1.5f;
    private SoundEffects[] soundEffects;
    private bool madeRespawn = false;
    // Start is called before the first frame update
    void Start()
    {
        soundEffects = GetComponents<SoundEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLimit && player.transform.position.y < waterLevel && !madeRespawn)
        {
            soundEffects[1].PlaySoundClip();
            madeRespawn = true;
            Invoke("respawnPlayer", respawnAfter);
        }
    }

    private void respawnPlayer()
    {
        soundEffects[0].PlaySoundClip();
        player.transform.position = playerRespawnPoint.position;
        madeRespawn = false;
    }


}
