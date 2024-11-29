using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoxGame : MonoBehaviour
{
    public GameObject orderCanvas;
    public GameObject explanationsCanvas;
    public GameObject orderPanel;
    public GameObject practicalPanel;
    public GameObject talkingPanel;
    public GameObject cheboxCanvas;
    public GameObject camera;
    public GameObject player;
    public GameObject arrow;
    public GameObject[] lettersObj;
    public GameObject practiceNPC;
    public GameObject missionCompleteCanvas;
    public UnityEngine.UI.Button checkboxButton;
    public UnityEngine.UI.Toggle[] toggles;
    public TMP_Text checkboxText;
    public TMP_Text practicalText;
    public TMP_Text orderText;
    public TMP_Text talkingTextExplanations;
    public TMP_Text practicalTextExplanations;
    static public List<string> boxNumbers;
    static public char[] boxLetters;
    public delegate void EndFunc();
    public AudioSource soccesSound;
    public AudioSource failSound;
    public GameObject[] numbersOnBox;
    public static bool startFromQuestions = false;

    bool[] checkBoxArr;
    List<List<TextPartition>> texts;
    List<EndFunc> funcs;
    int level;
    List<GameObject> balls, LettersList;
    List<Vector3> originalLetterPlace;
    List<int> ballNumbers;
    bool canPress, firstTouch, arrayState, levelFirst, level3Start;
    string resultLevel3;

    const int DELAY_END = 3, ID1 = 1, ID2 = 2, ID3 = 3, BOX_LEN = 7, BALL_NUMBER = 3, CAMERA_FAR = 21, CAMERA_CLOSE = 60, JUMP = 19, ROTATION = 17, LETTER_JUMP = 3, MIN_LETTER = 3, MAX_LETTER = 8;
    const float DELAY = 1.5f, BALL_SIZE = 0.5f, BALL_POS_X1 = -1073.52f, BALL_POS_X2 = -1065.36f, BALL_POS_X3 = -1069.36f, BALL_POS_Y = 9.834f, BALL_POS_Z1 = 353.66f, BALL_POS_Z2 = 357.66f, CAMERA_POS_X1 = 0.1645798f, CAMERA_POS_Y1 = 0.0239689f, CAMERA_POS_Z1 = 0.01668368f, CAMERA_POS_X2 = -1069.866f, CAMERA_POS_Y2 = 22.4789f, CAMERA_POS_Z2 = 312.5044f, DOWN_LETTER = 6.35f;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("Hello, darling! How are you today? Do you know what an array is? I guess you don’t, but don’t worry; now you will learn everything you need to know!", ""));
        list.Add(new TextPartition("An array is a data structure that stores a collection of elements, typically of the same type. Each element in an array can be accessed directly by its index, which is its numerical position within the array. For example:", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n}"));
        list.Add(new TextPartition("We define a general array using the following example:\ndataType indicates the type of data we will use, such as int or String.\narrayName refers to the name we want to give our array, for example, arr.", "dataType[] arrayName;"));
        list.Add(new TextPartition("You can create an array in many ways. For example:", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};  // Array of integers\n\tString[] names = {\"Alice\", \"Bob\", \"Charlie\"}; // Array of Strings\n\tint[] numbers2 = new int[4];\n}"));
        list.Add(new TextPartition("I've noticed that you might not know what the keyword 'new' means. For now, let's say that the 'new' keyword is used to create new objects and allocate memory for them.", "public static void main(String[] args) {\n\ttint[] numbers = new int[4];\n}"));
        list.Add(new TextPartition("Here are some facts about arrays:\nOnce an array is created, its size cannot be changed in Java.\nArray elements are accessed using their index, which starts from 0 (the first element) and goes up to the size minus 1 (the last element).", ""));
        list.Add(new TextPartition("3. Be cautious when accessing elements outside the array bounds (an index less than 0 or greater than or equal to the size), as this can lead to an ArrayIndexOutOfBoundsException.\nLet's look at some examples", ""));
        list.Add(new TextPartition("4. In arrays, we can find the number of elements using the length property. For example, this code will print 3 because there are three elements in arr.", "public static void main(String[] args) {\n\tint[] arr = {1, 2, 3};\n\tSystem.out.println(arr.Length);\n}"));
        list.Add(new TextPartition("5. When you create an array of int using new, all the values will be initialized to 0.", ""));
        list.Add(new TextPartition("In this code, it will print the number 9 because we are accessing the value at index 2, which is 9.", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n\tSystem.out.println(numbers[2]);\n}"));
        list.Add(new TextPartition("This code will result in a runtime exception because we are trying to print a value outside the array boundaries. ", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n\tSystem.out.println(numbers[4]);\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tint[] arr = {3, 4};\n\tint temp = arr[0];\n\tarr[0] = arr[1];\n\tarr[0] = temp;\n\tSystem.out.println(arr[0]);\n\tSystem.out.println(arr[1]);\n}"));
        list.Add(new TextPartition("The code will print 4 and then 3 because we are swapping the values of arr[0] and arr[1], and they are switching places.", "public static void main(String[] args) {\n\tint[] arr = {3, 4};\n\tint temp = arr[0];\n\tarr[0] = arr[1];\n\tarr[0] = temp;\n\tSystem.out.println(arr[0]);\n\tSystem.out.println(arr[1]);\n}"));
        list.Add(new TextPartition("We can also define 2D arrays, 3D arrays, and so on, but usually, you only need up to 2D arrays. Arrays with more than two dimensions are typically used for uncommon purposes.", ""));
        list.Add(new TextPartition("You can define a 2D array, for example:\nYou can think of a 2D array as a matrix with rows and columns. In this case, we have 2 rows and 4 columns.", "public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2, 3, 4}, {5, 6, 7, 8}};\n\tint[][] arr2d2 = new int[2][4]\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2},{3, 4}};\n\tint[] arr = new int[2];\n\tSystem.out.println(arr2d[0][0] + arr2d[1][0]);\n\tSystem.out.printlnarr2d[0][1] + arr2d[1][1]);\n}"));
        list.Add(new TextPartition("The code will print 4 and then 6 because we add the first element in row [0] to the first element in row [1] and print the result. Then, we add the second element in row [0] to the second element in row [1] and print that result.", "public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2},{3, 4}};\n\tSystem.out.println(arr2d[0][0] + arr2d[1][0]);\n\tSystem.out.printlnarr2d[0][1] + arr2d[1][1]);\n}"));
        list.Add(new TextPartition("All the rules that apply to 1D arrays also apply to 2D arrays.", ""));
        list.Add(new TextPartition("Now, let's do some exercises!", ""));
        texts.Add(list);

        //list = new List<TextPartition>();
        //list.Add(new TextPartition("in java we have c", ""));

        checkBoxArr = new bool[7] {false, false, false, false, false, false, false};
        boxLetters = new char[7] {'\0', '\0', '\0', '\0', '\0', '\0', '\0' };
        levelFirst = arrayState = firstTouch = canPress = true;
        level3Start = false;
        level = 0;
        funcs = new List<EndFunc>();
        funcs.Add(level0);
        funcs.Add(level1);
        funcs.Add(level2);
        funcs.Add(level3);
        funcs.Add(level4);
        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        AdminMission.okFunc = arrayOkFunc;
        PauseMenu.updateSave("City", "Array", 0);
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(DELAY);
        funcs[level]();
        canPress = true;
    }

    private IEnumerator delayPressLevel4()
    {
        yield return new WaitForSeconds(DELAY);
        funcs[level]();
        canPress = true;
        checkboxButton.interactable = true;
    }

    private IEnumerator delayMission()
    {
        yield return new WaitForSeconds(DELAY + 1);
        if (++level < funcs.Count)
        {
            levelFirst = true;
            funcs[level]();
        }
        canPress = true;
    }

    private IEnumerator delayEnd()
    {
        yield return new WaitForSeconds(DELAY_END);
        cheboxCanvas.SetActive(false);
        missionCompleteCanvas.SetActive(true);
    }

    GameObject createBall(int index, float x, float y, float z)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "ball" + index;
        ball.GetComponent<SphereCollider>().isTrigger = true;
        ball.tag = "BallBox";
        ball.transform.position = new Vector3(x, y, z);
        ball.transform.localScale = new Vector3(BALL_SIZE, BALL_SIZE, BALL_SIZE); ;
        return ball;
    }

    void level0()
    {
        balls = new List<GameObject>();
        boxNumbers = new List<string>();
        ballNumbers = new List<int>();
        orderText.text = "press 'E' button to catch/drop the ball";
        balls.Add(createBall(ID1, BALL_POS_X1, BALL_POS_Y, BALL_POS_Z1));
        balls.Add(createBall(ID2, BALL_POS_X2, BALL_POS_Y, BALL_POS_Z1));
        ballNumbers.Add(Random.Range(0, BOX_LEN));
        ballNumbers.Add(Random.Range(0, BOX_LEN));
        while (ballNumbers[0] == ballNumbers[1])
        {
            ballNumbers[1] = Random.Range(0, BOX_LEN);
        }
        practicalText.text = "insert the balls into box number [" + ballNumbers[0] + "]\nand box number [" + ballNumbers[1] + "]\nWhen you finish go back to gradma and press on 'F'";
    }

    void randomNumber()
    {
        ballNumbers = new List<int>();
        ballNumbers.Add(Random.Range(0, BOX_LEN));
        for (int i = 0; i < 2; i++)
        {
            int rnd = Random.Range(0, BOX_LEN);
            for (int j = 0; j < ballNumbers.Count; j++)
            {
                if (rnd == ballNumbers[j])
                {
                    rnd = Random.Range(0, BOX_LEN);
                    j = -1;
                }
            }
            ballNumbers.Add(rnd);
        }
    }

    void level1()
    {
        balls = new List<GameObject>();
        boxNumbers = new List<string>();
        orderText.text = "press 'E' button to catch/drop the ball";
        balls.Add(createBall(ID1, BALL_POS_X1, BALL_POS_Y, BALL_POS_Z1));
        balls.Add(createBall(ID2, BALL_POS_X2, BALL_POS_Y, BALL_POS_Z1));
        balls.Add(createBall(ID3, BALL_POS_X3, BALL_POS_Y, BALL_POS_Z2));
        randomNumber();
        practicalText.text = "insert the balls into box number [" + ballNumbers[0] + "]\nand box number [" + ballNumbers[1] + "]\nand box number [" + ballNumbers[2] + "]\nWhen you finish go back to gradma and press on 'F'";
    }

    void level2()
    {
        if (levelFirst)
        {
            for (int i = 0; i < BOX_LEN; i++)
            {
                GameObject.Find("boxNumber" + i).SetActive(false);
            }
        }
        levelFirst = false;
        level1();
    }

    public void clickCheckBox(GameObject chBox)
    {
        checkBoxArr[chBox.name[BOX_LEN + 1] - '0'] = !checkBoxArr[chBox.name[BOX_LEN + 1] - '0'];
    }

    public void checkButton()
    {
        checkboxButton.interactable = false;
        int cnt = 0;
        for (int i = 0; i < checkBoxArr.Length; i++)
        {
            bool wrongCheck = checkBoxArr[i];
            for (int j = 0; j < BALL_NUMBER; j++)
            {
                if (checkBoxArr[i] && i == ballNumbers[j])
                {
                    cnt++;
                    wrongCheck = false;
                    continue;
                }
            }
            if (wrongCheck)
            {
                cnt = BALL_NUMBER + 1;
            }
        }
        if (cnt == BALL_NUMBER)
        {
            camera.GetComponent<Camera>().fieldOfView = CAMERA_CLOSE;
            soccesSound.Play();
            checkboxText.text = "correct! you finished all the tasks here Good luck!!!";
            player.transform.position += new Vector3(0, 1, JUMP);
            arrow.SetActive(true);
            arrow.transform.position = player.transform.position;
            camera.transform.position = new Vector3(CAMERA_POS_X1, CAMERA_POS_Y1, CAMERA_POS_Z1) + player.transform.position;
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<Movement>().enabled = true;
            StartCoroutine(delayEnd());
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            Practice.taskName = "array";
            Practice.canAsk = PauseMenu.canPause = true;
            Movement.mission = practiceNPC;
            PauseMenu.updateSave("City", "Array", 1);
        }
        else
        {
            failSound.Play();
            checkboxText.text = "Wrong! please try again";
            StartCoroutine(delayPressLevel4());
            canPress = false;
        }
        for (int i = 0; i < balls.Count; i++)
        {
            Destroy(balls[i]);
        }
    }

    void level4()
    {
        //
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        PauseMenu.canPause = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Movement>().enabled = false;
        if (levelFirst)
        {
            camera.GetComponent<Camera>().fieldOfView = CAMERA_FAR;
            player.transform.position += new Vector3(0, 0, -JUMP);
            levelFirst = false;
        }
        camera.transform.position = new Vector3(CAMERA_POS_X2, CAMERA_POS_Y2, CAMERA_POS_Z2);
        camera.transform.rotation = Quaternion.Euler(ROTATION, 0, 0);
        //
        orderCanvas.SetActive(false);
        cheboxCanvas.SetActive(true);
        checkboxText.text = "In which indexes the balls are?\nplease check the right boxes";
        balls = new List<GameObject>();
        randomNumber();
        for (int i = 1; i < BALL_NUMBER + 1; i++)
        {
            Vector3 box = GameObject.Find("BoxArr" + ballNumbers[i - 1]).transform.position;
            balls.Add(createBall(i, box.x, box.y, box.z));
        }
        practicalText.text = "in which indexes the balls are?";
    }

    void arrayOkFunc()
    {
        if (texts[AdminMission.currentSubMission].Count == AdminMission.currentSubText)
        {
            AdminMission.endOk = true;
            AdminMission.currentSubMission++;
            explanationsCanvas.SetActive(false);
            GameObject.Find("Player").GetComponent<Movement>().enabled = true;
            orderCanvas.SetActive(true);
            practicalPanel.SetActive(true);
            orderPanel.SetActive(false);
            firstTouch = false;
            level0();
        }
    }

    void level3()
    {
        level3Start = true;
        int size = Random.Range(MIN_LETTER, MAX_LETTER);
        int[] capacity = {0, 0, 0, 0};
        resultLevel3 = "";
        LettersList = new List<GameObject>();
        originalLetterPlace = new List<Vector3>();
        for (int i = 0; i < size; i++)
        {
            int rnd = Random.Range(0, capacity.Length);
            if (capacity[rnd] == MIN_LETTER)
            {
                i--;
                continue;
            }
            resultLevel3 += (char)('A' + rnd);
            capacity[rnd]++;
        }
        practicalText.text = "Create the word:\n\"" + resultLevel3 + "\"\nWhen you finish go back to gradma and press on 'F'";
        for (int i = 0; i < capacity.Length; i++)
        {
            for(int j = 0; j < capacity[i]; j++)
            {
                LettersList.Add(lettersObj[i * LETTER_JUMP + j]);
                originalLetterPlace.Add(new Vector3(lettersObj[i * LETTER_JUMP + j].transform.position.x, lettersObj[i * LETTER_JUMP + j].transform.position.y, lettersObj[i * LETTER_JUMP + j].transform.position.z));
                lettersObj[i * LETTER_JUMP + j].transform.position -= new Vector3(0, DOWN_LETTER, 0);
                for (int k = 0; k < lettersObj[i * LETTER_JUMP + j].transform.childCount; k++)
                {
                    lettersObj[i * LETTER_JUMP + j].transform.GetChild(k).GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    void intro()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        PauseMenu.canPause = false;
        GameObject.Find("Player").GetComponent<Movement>().enabled = false;
        explanationsCanvas.SetActive(true);
        talkingTextExplanations.text = texts[0][0].talking;
        practicalTextExplanations.text = texts[0][0].practical;
        AdminMission.currentSubText = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Login.world != "City" && Movement.missionInProgress == "")
        {
            talkingPanel.SetActive(true);
            checkboxButton.interactable = true;
            for (int i = 0; i < numbersOnBox.Length; i++)
            {
                numbersOnBox[i].SetActive(true);
            }
            for (int i = 0; i < toggles.Length; i++)
            {
                checkBoxArr[i] = toggles[i].isOn = false;
            }
            checkBoxArr = new bool[BOX_LEN] { false, false, false, false, false, false, false };
            boxLetters = new char[BOX_LEN] { '\0', '\0', '\0', '\0', '\0', '\0', '\0' };
            levelFirst = arrayState = firstTouch = canPress = true;
            level3Start = false;
            level = 0;
            Movement.missionInProgress = "array";
            AdminMission.texts = texts;
            AdminMission.currentSubMission = AdminMission.currentSubText = 0;
            AdminMission.okFunc = arrayOkFunc;
        }
        if (Login.world != "City" && Movement.missionInProgress != "array")
        {
            return;
        }
        if (firstTouch)
        {
            arrow.SetActive(false);
            intro();
        }
    }

    void checkLevel3()
    {
        bool findMistake = false;
        for (int i = 0; i < LettersList.Count; i++)
        {
            LettersList[i].transform.position = originalLetterPlace[i];
            for (int k = 0; k < LettersList[i].transform.childCount; k++)
            {
                LettersList[i].transform.GetChild(k).GetComponent<MeshRenderer>().enabled = false;
            }
        }
        for (int i = 0; i < resultLevel3.Length; i++ )
        {
            if (resultLevel3[i] != boxLetters[i])
            {
                findMistake = true;
            }
        }
        for (int i = resultLevel3.Length; i < BOX_LEN; i++)
        {
            if (boxLetters[i] != 0)
            {
                findMistake = true;
            }
        }
        if (findMistake)
        {
            for (int i = 0; i < BOX_LEN; i++)
            {
                boxLetters[i] = '\0';
            }
            failSound.Play();
            practicalText.text = "Wrong placment!";
            StartCoroutine(delayPress());
        }
        else
        {
            soccesSound.Play();
            practicalText.text = "Correct!!!!";
            StartCoroutine(delayMission());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (arrayState && Input.GetKeyDown("f") && (!Movement.hoverBall && !Movement.hoverLetter) && canPress) 
        {
            canPress = false;
            if (level3Start)
            {
                checkLevel3();
                return;
            }
            for (int i = 0; i < balls.Count; i++)
            {
                Destroy(balls[i]);
            }
            if (boxNumbers.Count != ballNumbers.Count)
            {
                failSound.Play();
                practicalText.text = "Wrong placment!";
                StartCoroutine(delayPress());
                return;
            }
            for (int i = 0; i < boxNumbers.Count; i++)
            {
                if (!boxNumbers.Contains("BoxArr" + ballNumbers[i]))
                {
                    failSound.Play();
                    practicalText.text = "Wrong placment!";
                    StartCoroutine(delayPress());
                    return;
                }
            }
            soccesSound.Play();
            practicalText.text = "Correct!!!!";
            StartCoroutine(delayMission());
        } 
        else
        {
            if (Input.GetKeyDown("f") && canPress && (!Movement.hoverBall && !Movement.hoverLetter))
            {
                canPress = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startFromQuestions)
        {
            startFromQuestions = false;
            firstTouch = false;
            Practice.taskName = "array";
            Practice.canAsk = PauseMenu.canPause = true;
            Movement.mission = practiceNPC;
            Practice.nextMission = null;
        }
    }
}
