using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxGame : MonoBehaviour
{
    public GameObject orderCanvas;
    public GameObject explanationsCanvas;
    public GameObject orderPanel;
    public GameObject cheboxCanvas;
    public GameObject camera;
    public GameObject player;
    public GameObject arrow;
    public TMP_Text checkboxText;
    public TMP_Text practicalText;
    public TMP_Text orderText;
    public TMP_Text talkingTextExplanations;
    public TMP_Text practicalTextExplanations;
    static public List<string> boxNumbers;
    public delegate void EndFunc();

    bool[] checkBoxArr;
    List<List<TextPartition>> texts;
    List<EndFunc> funcs;
    int level;
    List<GameObject> balls;
    List<int> ballNumbers;
    bool canPress, firstTouch;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("hello darling. how are you today? do you know what is array?\nI guess not, but don't worry now you will know everything you need to know", ""));
        list.Add(new TextPartition("An array is a data structure that stores a collection of elements, typically of the same type. Each element in an array can be accessed directly by its index, which is a numerical position within the array For example:", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n}"));
        list.Add(new TextPartition("we define a general array by this example:\ndataType which type of data we use for example - int, String\narrayName means the name we want to caled our array for exapmle - arr", "dataType[] arrayName;"));
        list.Add(new TextPartition("you can create array in many ways for example:", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};  // Array of integers\n\tString[] names = {\"Alice\", \"Bob\", \"Charlie\"}; // Array of Strings\n\n\tint[] numbers2 = new int[4];\n}"));
        list.Add(new TextPartition("I have noticed that you don't know what is the key word 'new' means. Let say for now\nthat 'new' keyword is used to create new objects and allocate memory for them", "public static void main(String[] args) {\n\ttint[] numbers = new int[4];\n}"));
        list.Add(new TextPartition("Some facts about arrays:\n1. Once an array is created, its size cannot be changed in Java.\n2. Array elements are accessed using their index, which starts from 0 (the first element) and goes up to the size minus 1 (the last element).\n", ""));
        list.Add(new TextPartition("3. Be cautious of accessing elements outside the array bounds (index less than 0 or greater than or equal to the size) as it can lead to ArrayIndexOutOfBoundsException.\nLets some examples", ""));
        list.Add(new TextPartition("Here the code will print the number 9 because we printing the number in the index 2 and this is 9", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n\tSystem.out.println(numbers[2]);\n}"));
        list.Add(new TextPartition("Here the code will get a runtime exception because we are wanted to print a value out of the array boundaries ", "public static void main(String[] args) {\n\tint[] numbers = {1, 5, 9, 2};\n\tSystem.out.println(numbers[4]);\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tint[] arr = {3, 4};\n\tint temp = arr[0];\n\tarr[0] = arr[1];\n\tarr[0] = temp;\n\tSystem.out.println(arr[0]);\n\tSystem.out.println(arr[1]);\n}"));
        list.Add(new TextPartition("The code here will print 4 and then 3 becasue we are making a swap between the values of arr[0] and arr[1] ans they switching places.", "public static void main(String[] args) {\n\tint[] arr = {3, 4};\n\tint temp = arr[0];\n\tarr[0] = arr[1];\n\tarr[0] = temp;\n\tSystem.out.println(arr[0]);\n\tSystem.out.println(arr[1]);\n}"));
        list.Add(new TextPartition("We can define also 2d array, 3d array, ... and so on but usually you need up to 2d. Above 2d it is for some uncommon use", ""));
        list.Add(new TextPartition("you can define 2d array for example:\nyou can consider 2d array as a matrix with row and columns and here we have 2 rows and 4 columns", "public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2, 3, 4}, {5, 6, 7, 8}};\n\tint[][] arr2d2 = new int[2][4]\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "\"public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2},{3, 4}};\n\tint[] arr = new int[2];\n\tSystem.out.println(arr2d[0][0] + arr2d[1][0]);\n\tSystem.out.printlnarr2d[0][1] + arr2d[1][1]);\n}"));
        list.Add(new TextPartition("the code here will print 4 and then 6 becasue we add the first elemnt in the row number [0] and the first element of row number [1] and print it.\nThen we add the second elemnt in the row number [0] and the second element of row number [1] and print it", "\"public static void main(String[] args) {\n\tint[][] arr2d = {{1, 2},{3, 4}};\n\tSystem.out.println(arr2d[0][0] + arr2d[1][0]);\n\tSystem.out.printlnarr2d[0][1] + arr2d[1][1]);\n}"));
        list.Add(new TextPartition("all the rules you have in 1d array are applied in 2d array too", ""));
        texts.Add(list);
        checkBoxArr = new bool[7] {false, false, false, false, false, false, false};
        firstTouch = canPress = true;
        level = 0;
        funcs = new List<EndFunc>();
        funcs.Add(level0);
        funcs.Add(level1);
        funcs.Add(level2);
        funcs.Add(level3);
        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        AdminMission.okFunc = arrayOkFunc;
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(1.5f);
        funcs[level]();
        canPress = true;
    }

    private IEnumerator delayMission()
    {
        yield return new WaitForSeconds(2.5f);
        if (++level < funcs.Count)
        {
            funcs[level]();
        }
        canPress = true;
    }

    private IEnumerator delayEnd()
    {
        yield return new WaitForSeconds(3);
        cheboxCanvas.SetActive(false);
    }

    GameObject createBall(int index, float x, float y, float z)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "ball" + index;
        ball.GetComponent<SphereCollider>().isTrigger = true;
        ball.tag = "BallBox";
        ball.transform.position = new Vector3(x, y, z);
        ball.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); ;
        return ball;
    }

    void level0()
    {
        balls = new List<GameObject>();
        boxNumbers = new List<string>();
        ballNumbers = new List<int>();
        orderText.text = "press 'E' button to catch/drop the ball";
        balls.Add(createBall(1, -1073.52f, 9.834f, 353.66f));
        balls.Add(createBall(2, -1065.36f, 9.834f, 353.66f));
        ballNumbers.Add(Random.Range(0, 7));
        ballNumbers.Add(Random.Range(0, 7));
        while (ballNumbers[0] == ballNumbers[1])
        {
            ballNumbers[1] = Random.Range(0, 7);
        }
        practicalText.text = "insert the balls into box numer [" + ballNumbers[0] + "]\nand box number [" + ballNumbers[1] + "]\nWhen you finish go back to gradma and press on 'F'";
    }

    void randomNumber()
    {
        ballNumbers = new List<int>();
        ballNumbers.Add(Random.Range(0, 7));
        for (int i = 0; i < 2; i++)
        {
            int rnd = Random.Range(0, 7);
            for (int j = 0; j < ballNumbers.Count; j++)
            {
                if (rnd == ballNumbers[j])
                {
                    rnd = Random.Range(0, 7);
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
        balls.Add(createBall(1, -1073.52f, 9.834f, 353.66f));
        balls.Add(createBall(2, -1065.36f, 9.834f, 353.66f));
        balls.Add(createBall(3, -1069.36f, 9.834f, 357.66f));
        randomNumber();
        practicalText.text = "insert the balls into box numer [" + ballNumbers[0] + "]\nand box number [" + ballNumbers[1] + "]\nand box number [" + ballNumbers[2] + "]\nWhen you finish go back to gradma and press on 'F'";
    }

    void level2()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject.Find("boxNumber" + i).SetActive(false);
        }
        level1();
    }

    public void clickCheckBox(GameObject chBox)
    {
        checkBoxArr[chBox.name[8] - '0'] = !checkBoxArr[chBox.name[8] - '0'];
    }

    public void checkButton()
    {
        int cnt = 0;
        for (int i = 0; i < checkBoxArr.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (checkBoxArr[i] && i == ballNumbers[j])
                {
                    cnt++;
                    continue;
                }
            }
        }
        if (cnt == 3)
        {
            checkboxText.text = "correct! you finished all the tasks here Good luck!!!";
            player.transform.position += new Vector3(0, 0, 15);
            arrow.transform.position += new Vector3(0, 0, 15);
            camera.transform.position = new Vector3(0.1645798f, 0.0239689f, 0.01668368f) + player.transform.position;
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<Movement>().enabled = true;
            StartCoroutine(delayEnd());
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else
        {
            checkboxText.text = "Wrong! please try again";
            StartCoroutine(delayPress());
            canPress = false;
        }
        for (int i = 0; i < balls.Count; i++)
        {
            Destroy(balls[i]);
        }
    }

    void level3()
    {
        //
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Movement>().enabled = false;
        player.transform.position += new Vector3(0, 0, -15);
        arrow.transform.position += new Vector3(0, 0, -15);
        camera.transform.position = new Vector3(-1070.866f, 22.4789f, 318.9044f);
        camera.transform.rotation = Quaternion.Euler(17, 0, 0);
        //
        orderCanvas.SetActive(false);
        cheboxCanvas.SetActive(true);
        checkboxText.text = "In which indexes the balls are?\nplease check the right boxes";
        balls = new List<GameObject>();
        randomNumber();
        for (int i = 1; i < 4; i++)
        {
            Vector3 box = GameObject.Find("BoxArr" + ballNumbers[i - 1]).transform.position;
            balls.Add(createBall(i, box.x, box.y, box.z));
        }
        practicalText.text = "in which indexses the balls are?";
    }

    void arrayOkFunc()
    {
        if (texts[AdminMission.currentSubMission].Count == AdminMission.currentSubText)
        {
            //funcs[AdminMission.currentSubMission]();
            AdminMission.endOk = true;
            AdminMission.currentSubMission++;
            explanationsCanvas.SetActive(false);
            GameObject.Find("Player").GetComponent<Movement>().enabled = true;
            orderCanvas.SetActive(true);
            orderPanel.SetActive(false);
            firstTouch = false;
            level0();
        }
    }

    void intro()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        GameObject.Find("Player").GetComponent<Movement>().enabled = false;
        explanationsCanvas.SetActive(true);
        talkingTextExplanations.text = texts[0][0].talking;
        practicalTextExplanations.text = texts[0][0].practical;
        AdminMission.currentSubText = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstTouch)
        {
            intro();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("f") && !Movement.hoverBall && canPress) 
        {
            canPress = false;
            for (int i = 0; i < balls.Count; i++)
            {
                Destroy(balls[i]);
            }
            if (boxNumbers.Count != ballNumbers.Count)
            {
                practicalText.text = "Worong placment!";
                StartCoroutine(delayPress());
                return;
            }
            for (int i = 0; i < boxNumbers.Count; i++)
            {
                if (!ballNumbers.Contains(boxNumbers[i][6] - '0'))
                {
                    practicalText.text = "Worong placment!";
                    StartCoroutine(delayPress());
                    return;
                }
            }
            practicalText.text = "Correct!!!!";
            StartCoroutine(delayMission());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
