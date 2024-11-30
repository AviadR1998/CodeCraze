using UnityEngine;



//this script instatiate a new dog by specific values
public class DogSpawner : MonoBehaviour
{

    public GameObject ClassMissionManager;
    public uint maxAnimVal = 7, minAnimVal = 0;
    private float dogSize = 2f;
    private ClassMission classMission = null;

    void Start()
    {
        classMission = ClassMissionManager.GetComponent<ClassMission>();
    }

    public GameObject SpawnDog(GameObject dogPrefab, Transform spawnPoint, uint animationNum, uint dogAge, string dogName, string bedColor, GameObject bed)
    {
        if (animationNum > maxAnimVal)
        {
            animationNum = minAnimVal;
        }
        // Instantiate the dog at the spawn position and with default rotation
        GameObject newDog = Instantiate(dogPrefab, spawnPoint.position, spawnPoint.rotation);
        newDog.GetComponent<BoxCollider>().size = new Vector3(dogSize, dogSize, dogSize);

        DogAnimationController dac = newDog.GetComponent<DogAnimationController>();
        dac.dogName = dogName;
        dac.dogAge = dogAge;
        dac.isSpawn = true;
        dac.favoriteAnim = animationNum;
        dac.SetBedColor(bedColor);
        dac.bedObject = bed;

        if (classMission)
        {
            classMission.AddDog();
        }

        return newDog;
    }
}
