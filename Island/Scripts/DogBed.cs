using UnityEngine;
using UnityEngine.UI;
using TMPro;


//This script manage the bed of a dog, create a dog when press 'e' 
public class DogBed : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField ageInput;
    public TMP_Dropdown breedDropdown, signutareMoveDropdown, colorDropdown;
    public GameObject germanPrefab, corgiPrefab, pugPrefab, curPrefab, chiuPrefab;
    public Transform dogLocation;
    public Button createDogBtn;
    public Canvas createDogCanvas, canvasToClose;
    private DogSpawner dogSpawner;
    private GameObject dog;
    private bool dogCreated = false;
    private uint breathingAnim = 0, wigglingAmim = 1, slowWalkAnim = 2, fastWalkAnim = 3, runningAnim = 4,
                eatingAnim = 5, barkingAnim = 6, sittingAnim = 7;

    private bool isPlayerInTrigger = false;
    private string selectedColor;

    private void Start()
    {
        createDogBtn.onClick.AddListener(GetFormData);
        dogSpawner = GetComponentInParent<DogSpawner>();
        dog = null;
    }

    public void GetFormData()
    {
        if (!isPlayerInTrigger)
        {
            return;
        }
        // Retrieve text from InputFields
        string dogName = nameInput.text;
        string dogAge = ageInput.text;

        // Retrieve selected value from Dropdown
        string selectedBreed = breedDropdown.options[breedDropdown.value].text;
        string selectedSigMove = signutareMoveDropdown.options[signutareMoveDropdown.value].text;
        selectedColor = colorDropdown.options[colorDropdown.value].text;


        int numericValue;
        bool isNumber = int.TryParse(dogAge, out numericValue);
        if (dogName == "" || !isNumber || numericValue < 1)
        {
            return;
        }

        activateDog(dogName, numericValue, selectedBreed, selectedSigMove, selectedColor);

    }

    private void activateDog(string dogName, int age, string breed, string sigMove, string color)
    {
        GameObject selectedPrefab;
        switch (breed)
        {
            case "German Sheperd":
                selectedPrefab = germanPrefab;
                break;
            case "Pug":
                selectedPrefab = pugPrefab;
                break;
            case "Cur":
                selectedPrefab = curPrefab;
                break;
            case "Corgi":
                selectedPrefab = corgiPrefab;
                break;
            case "Chihuahua":
                selectedPrefab = chiuPrefab;
                break;
            default:
                selectedPrefab = germanPrefab;
                break;
        }

        if (dogSpawner)
        {
            dog = dogSpawner.SpawnDog(selectedPrefab, dogLocation, getAnimationInt(sigMove), (uint)age,
             dogName, selectedColor, transform.parent.gameObject);
        }

        GetComponent<SoundEffects>().PlaySoundClip();
        canvasToClose.gameObject.SetActive(false);
        GetComponentInParent<BlockPlayerCamera>().resumeCamera();
        dogCreated = true;
    }

    private uint getAnimationInt(string animation)
    {
        uint val = 0;
        switch (animation)
        {
            case "breathing":
                val = breathingAnim;
                break;
            case "Wiggling Tail":
                val = wigglingAmim;
                break;
            case "Slow Walk":
                val = slowWalkAnim;
                break;
            case "Fast Walk":
                val = fastWalkAnim;
                break;
            case "Running":
                val = runningAnim;
                break;
            case "Eating":
                val = eatingAnim;
                break;
            case "Barking":
                val = barkingAnim;
                break;
            case "Sitting":
                val = sittingAnim;
                break;
            default:
                val = breathingAnim;
                break;
        }

        return val;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { // Ensure only the player triggers this
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown("e") && !dogCreated)
        {
            GetComponentInParent<BlockPlayerCamera>().stopCamera();
            createDogCanvas.gameObject.SetActive(true);
        }
    }

    public void RestartBed()
    {
        if (dog)
        {
            dog.GetComponent<DogAnimationController>().SetBedColor("Yellow");
            Destroy(dog);
            dogCreated = false;
        }

    }

}
