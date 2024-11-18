using System.Data;
using UnityEngine;

public class DogSpawner : MonoBehaviour
{
    //public GameObject dogPrefab;  // Assign the dog prefab in the Inspector
    //public Transform spawnPoint;  // The position where the dog should spawn
    //public float spawnDelay = 2f; // Delay before spawning (optional)

    public GameObject ClassMissionManager;
    private ClassMission classMission = null;

    void Start()
    {
        classMission = ClassMissionManager.GetComponent<ClassMission>();
    }

    public GameObject SpawnDog(GameObject dogPrefab, Transform spawnPoint, uint animationNum, uint dogAge, string dogName, string bedColor, GameObject bed)
    {
        if (animationNum > 7)
        {
            animationNum = 0;
        }
        // Instantiate the dog at the spawn position and with default rotation
        GameObject newDog = Instantiate(dogPrefab, spawnPoint.position, spawnPoint.rotation);
        newDog.GetComponent<BoxCollider>().size = new Vector3(2f, 2f, 2f);

        DogAnimationController dac = newDog.GetComponent<DogAnimationController>();
        dac.dogName = dogName;
        dac.dogAge = dogAge;
        dac.isSpawn = true;
        dac.favoriteAnim = animationNum;
        dac.SetBedColor(bedColor);
        dac.bedObject = bed;
        //dac.ChangeAnimation((int)animationNum);

        // Optionally set other properties, such as position, animation, or components
        //newDog.GetComponent<Animator>().SetInteger("AnimationID", (int)animationNum);  // Start with an animation (e.g., idle)

        if (classMission)
        {
            classMission.AddDog();
        }

        return newDog;
    }
}
