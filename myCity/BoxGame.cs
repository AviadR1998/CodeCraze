using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoxGame : MonoBehaviour
{
    public GameObject orderCanvas;
    public GameObject orderPanel;
    public TMP_Text practicalText;
    public TMP_Text orderText;
    static public List<string> boxNumbers;
    public delegate void EndFunc();

    List<EndFunc> funcs;
    int level;
    List<GameObject> balls;
    GameObject ball1, ball2, ball3;
    List<int> ballNumbers;
    bool canPress;
    // Start is called before the first frame update
    void Start()
    {
        canPress = true;
        level = 0;
        orderCanvas.SetActive(true);
        orderPanel.SetActive(false);
        level0();
        funcs = new List<EndFunc>();
        funcs.Add(level0);
        funcs.Add(level1);
        funcs.Add(level2);
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

    void level1()
    {
        balls = new List<GameObject>();
        boxNumbers = new List<string>();
        ballNumbers = new List<int>();
        orderText.text = "press 'E' button to catch/drop the ball";
        balls.Add(createBall(1, -1073.52f, 9.834f, 353.66f));
        balls.Add(createBall(2, -1065.36f, 9.834f, 353.66f));
        balls.Add(createBall(2, -1069.36f, 9.834f, 357.66f));
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
        while (ballNumbers[0] == ballNumbers[1])
        {
            ballNumbers[1] = Random.Range(0, 7);
        }
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

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("f") && !Movement.hoverBall && canPress) 
        {
            canPress = false;
            for (int i = 0; i < balls.Count; i++)
            {
                Destroy(balls[i]);
            }
            print(ballNumbers.Count);
            print(boxNumbers.Count);
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
