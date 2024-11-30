using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//This script manage the dog animations and values for the classes mission
public class DogAnimationController : MonoBehaviour
{
    private Animator animator;

    // Assign this in the inspector or dynamically

    public String dogName, dogBreed;
    public uint dogAge, favoriteAnim;
    public Canvas messageCanvas, editDogCanvas;
    public CanvasGroup canvasGroup;
    public bool isSpawn = false;
    public GameObject bedObject = null;
    public static bool canBarkWhenClicked = true;
    private string bedColor = "Yellow", configBtnName = "ConfigBtn";
    private bool isTriggered = false, toConfigure = false, editOpen = false;
    public float startBarkIn = 1f, resetAnimIn = 5f;
    private Dog dog;
    private char startOfBtnName = 'S';
    private int startAnimVal = 0, endAnimVal = 7, barkIndex = 6;

    public class Dog
    {
        private string Name, Breed;
        private uint Age, FavoriteAnim;
        private static string barkStr = " says: Woof!";
        private static uint defaultAnim = 0, maxIndexAnim = 7;

        public Dog(string name, string breed, uint age, uint favoriteAnim)
        {
            this.Name = name;
            this.Breed = breed;
            this.Age = age;
            this.FavoriteAnim = favoriteAnim;
            if (favoriteAnim > maxIndexAnim)
            {
                FavoriteAnim = defaultAnim;
            }

        }
        public string Bark()
        {
            return Name + barkStr;
        }
        public int GetAnim()
        {
            return (int)FavoriteAnim;
        }
        public void SetAnim(uint anim)
        {
            this.FavoriteAnim = anim;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetAge(uint age)
        {
            this.Age = age;
        }

    }


    void Start()
    {
        this.dog = new Dog(dogName, dogBreed, dogAge, favoriteAnim);
        // Get the Animator component on this GameObject
        animator = GetComponent<Animator>();
        animator.SetInteger("AnimationID", dog.GetAnim());
        SetBedColor(bedColor);

        if (isSpawn)
        {
            Button[] configureButtons = editDogCanvas.gameObject.GetComponentsInChildren<Button>();
            foreach (Button btn in configureButtons)
            {
                print(btn.name);
                if (btn.name[0] == startOfBtnName)
                {
                    btn.onClick.AddListener(ConfigureClicked);
                }
                else if (btn.name == configBtnName)
                {
                    btn.onClick.AddListener(SaveConiguration);
                }
            }
        }

    }

    public void ChangeAnimation(int anim)
    {
        if (anim >= startAnimVal && anim <= endAnimVal)
        {
            // Get the Animator component on this GameObject
            animator.SetInteger("AnimationID", anim);
            dog.SetAnim((uint)anim);
            favoriteAnim = (uint)anim;
        }
    }

    public void SetBedColor(string colorName)
    {
        bedColor = colorName;
        Color color;
        if (bedObject)
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString(colorName, out color))
            {
                Transform pillow = bedObject.transform.Find("Pillow");
                if (pillow)
                {
                    Renderer renderer = pillow.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Set the material color to the parsed color
                        renderer.material.color = color;
                    }
                }

                Transform tray = bedObject.transform.Find("Tray");
                if (tray)
                {
                    Renderer renderer = tray.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Set the material color to the parsed color
                        renderer.material.color = color;
                    }
                }
            }
        }
    }

    public void resetAnim()
    {
        ChangeAnimation((int)favoriteAnim);
    }

    private void OnMouseDown()
    {
        if (!isSpawn && canBarkWhenClicked)
        {
            startBarking();
        }

    }

    private void startBarking()
    {
        int lastAnim = dog.GetAnim();
        ChangeAnimation(barkIndex);
        string barkStr = dog.Bark();
        messageCanvas.gameObject.SetActive(true);
        FadeAway fadeAwayComponent = messageCanvas.gameObject.AddComponent<FadeAway>();
        fadeAwayComponent.Fade = canvasGroup;
        TextMeshProUGUI textComponent = messageCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = barkStr;
        }
        Invoke("startPlayBark", startBarkIn);
        this.favoriteAnim = (uint)lastAnim;
        Invoke("resetAnim", resetAnimIn);

        if (!isSpawn)
        {
            GetComponentInParent<ClassMission>().ClickDog();
        }

    }

    private void startPlayBark()
    {
        GetComponent<SoundEffects>().PlaySoundClip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isSpawn)
        { // Ensure only the player triggers this
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isSpawn)
        {
            isTriggered = false;
        }
    }

    private void Update()
    {
        if (isTriggered && Input.GetKeyDown("b") && !editOpen)
        {
            startBarking();
        }
        else if (isTriggered && Input.GetKeyDown("i") && !editOpen)
        {
            GetComponent<BlockPlayerCamera>().stopCamera();
            OpenDogEditOption();
        }
    }


    private uint getAnimationInt(string animation)
    {
        uint val = 0;
        switch (animation)
        {
            case "breathing":
                val = 0;
                break;
            case "Wiggling Tail":
                val = 1;
                break;
            case "Slow Walk":
                val = 2;
                break;
            case "Fast Walk":
                val = 3;
                break;
            case "Running":
                val = 4;
                break;
            case "Eating":
                val = 5;
                break;
            case "Barking":
                val = 6;
                break;
            case "Sitting":
                val = 7;
                break;
            default:
                val = 0;
                break;
        }

        return val;
    }

    private string getAnimationName(uint animationId)
    {
        string animationName;
        switch (animationId)
        {
            case 0:
                animationName = "Breathing";
                break;
            case 1:
                animationName = "Wiggling Tail";
                break;
            case 2:
                animationName = "Slow Walk";
                break;
            case 3:
                animationName = "Fast Walk";
                break;
            case 4:
                animationName = "Running";
                break;
            case 5:
                animationName = "Eating";
                break;
            case 6:
                animationName = "Barking";
                break;
            case 7:
                animationName = "Sitting";
                break;
            default:
                animationName = "Breathing";
                break;
        }

        return animationName;
    }

    private void ChangeDogName(String newName)
    {
        dogName = newName;
        dog.SetName(newName);
    }

    private void OpenDogEditOption()
    {
        if (editDogCanvas != null)
        {
            editDogCanvas.gameObject.SetActive(true);
            editOpen = true;
            TextMeshProUGUI[] textFields = editDogCanvas.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI textField in textFields)
            {
                if (textField.name == "NameVar") // Replace with the specific child object's name
                {
                    textField.text = dogName;
                }
                else if (textField.name == "AgeVar")
                {
                    textField.text = "" + dogAge;
                }
                else if (textField.name == "BreedVar")
                {
                    textField.text = dogBreed;
                }
                else if (textField.name == "SignatureMoveVar")
                {
                    textField.text = getAnimationName(favoriteAnim);
                }
                else if (textField.name == "BedColorVar")
                {
                    textField.text = bedColor;
                }
            }
        }
    }

    private void ConfigureClicked()
    {
        toConfigure = true;
    }

    private void SaveConiguration()
    {
        if (!editOpen)
        {
            return;
        }
        if (toConfigure)
        {
            string newName = "", newAge, newColor = "", newSignature;
            uint newAgeVal = dogAge, newSignatureVal = 0;
            TMP_InputField[] inputFields = editDogCanvas.GetComponentsInChildren<TMP_InputField>();
            foreach (TMP_InputField inputField in inputFields)
            {
                if (inputField.name == "AddName")
                {
                    newName = inputField.text;
                }
                else if (inputField.name == "AddAge")
                {
                    newAge = inputField.text;
                    int numericValue;
                    bool isNumber = int.TryParse(newAge, out numericValue);
                    if (numericValue >= 0)
                    {
                        newAgeVal = (uint)numericValue;
                    }
                }

                inputField.gameObject.SetActive(false);
            }

            TMP_Dropdown[] tMP_Dropdowns = editDogCanvas.GetComponentsInChildren<TMP_Dropdown>();
            foreach (TMP_Dropdown tMP_Dropdown in tMP_Dropdowns)
            {
                if (tMP_Dropdown.name == "ChooseMove")
                {
                    newSignature = tMP_Dropdown.options[tMP_Dropdown.value].text;
                    newSignatureVal = getAnimationInt(newSignature);
                    ChangeAnimation((int)newSignatureVal);
                }
                else if (tMP_Dropdown.name == "ChooseColor")
                {
                    newColor = tMP_Dropdown.options[tMP_Dropdown.value].text;
                    if (newColor != "")
                    {
                        SetBedColor(newColor);
                    }
                }

                tMP_Dropdown.gameObject.SetActive(false);
            }

            if (newName != "")
            {
                ChangeDogName(newName);
            }

            dogAge = newAgeVal;

        }


        editDogCanvas.gameObject.SetActive(false);
        toConfigure = false;
        editOpen = false;
        GetComponent<BlockPlayerCamera>().resumeCamera();
    }

}

