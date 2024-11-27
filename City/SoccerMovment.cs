using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SoccerMovment : MonoBehaviour
{
    public static bool haveAnswer, turn, ifInitiateTurn;
    public GameObject arrow;
    public GameObject boy;
    public GameObject originalBoy;
    public GameObject player;
    public GameObject camera;
    public GameObject ball;
    public GameObject[] pointGame;
    public GameObject[] ballPos;
    public GameObject canvasSoccer;
    public GameObject questionSoccerCanvas;
    public GameObject nextMission;
    public GameObject missionCompleteCanvas;
    public TMP_Text scoreText;
    public TMP_Text turnText;
    public TMP_Text timeText;
    public TMP_Text explanationText;
    public GameObject canvasMission;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public GameObject mainCamera;
    public delegate void questionFunc();
    public AudioSource failSound;
    public AudioSource cheerSound;
    public AudioSource booingSound;
    List<questionFunc> funcs;

    string explanation;
    Vector3 enemyKeeperPos, startEnemyPos;
    int score, rnd, correctAnswer;
    float rndf;
    bool kick, endGame, firstFrame;
    // Start is called before the first frame update
    void Start()
    {
        startEnemyPos = new Vector3(pointGame[0].transform.position.x, pointGame[0].transform.position.y, pointGame[0].transform.position.z);
        funcs = new List<questionFunc>();
        funcs.Add(sumFor);
        funcs.Add(factFor);
        funcs.Add(comleteFor);
        funcs.Add(breakFor);
        funcs.Add(continueFor);
        funcs.Add(countingFor);
        funcs.Add(calculateFor);
    }

    private void OnEnable()
    {
        score = 0;
        scoreText.text = "0 - 0";
        firstFrame = turn = true;
        ifInitiateTurn = haveAnswer = endGame = kick = false;
        boy.SetActive(true);
        initiateTurn(0, 1, 0, new Vector3(0, 2.1f, 0), new Vector3(0, 2f, 0));
        InvokeRepeating("reduceSecond", 1f, 1f);
    }

    void initiateTurn(int i, int j, int ballI, Vector3 boyStart, Vector3 ballStart)
    {
        boy.transform.position = pointGame[i].transform.position - boyStart;
        player.transform.position = pointGame[j].transform.position;
        arrow.transform.position = pointGame[j].transform.position - new Vector3(6.2f, 0.6f, -0.3f);
        ball.transform.position = ballPos[ballI].transform.position + ballStart;
        player.transform.LookAt(pointGame[i].transform);
        camera.transform.LookAt(pointGame[i].transform);
        arrow.transform.LookAt(pointGame[i].transform);
        boy.transform.LookAt(pointGame[j].transform);
    }

    void shuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd1 = Random.Range(0, list.Count), rnd2 = Random.Range(0, list.Count);
            T temp = list[rnd1];
            list[rnd1] = list[rnd2];
            list[rnd2] = temp;
        }
    }

    List<int> createAnswers(int correct, int lowerBound, int upperBound)
    {
        List<int> ret = new List<int>();
        ret.Add(correct);
        while (ret.Count != 4)
        {
            int rnd = Random.Range(Mathf.Max(0, correct - lowerBound), correct + upperBound);
            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i] == rnd)
                {
                    rnd = Random.Range(Mathf.Max(0, correct - lowerBound), correct + upperBound);
                    i = -1;
                }
            }
            ret.Add(rnd);
        }
        shuffleList<int>(ret);
        return ret;
    }

    void sumFor()
    {
        int range = Random.Range(3, 8), sum = 0;
        string questionCode = "int sum = 0;\nfor (int i = 1; i <= " + range + "; i++) {\n\tsum += i;\n}\nSystem.out.println(sum);";
        for (int i = 1; i <= range; i++)
        {
            sum += i;
        }
        correctAnswer = sum;
        List<int> answers = createAnswers(sum, 20, 10);
        string questionText = "What is the output?";
        explanation = "The answer is " + sum + " because if you add all the numbers from 1 to " + range + " you will get " + sum + ".";
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, sum + "");
    }

    void factFor()
    {
        int range = Random.Range(2, 5), sum = 1;
        string questionCode = "int sum = 1;\nfor (int i = 1; i <= " + range + "; i++) {\n\tsum *= i;\n}\nSystem.out.println(sum);";
        for (int i = 2; i <= range; i++)
        {
            sum *= i;
        }
        correctAnswer = sum;
        List<int> answers = createAnswers(sum, 20, 10);
        string questionText = "What is the output?";
        explanation = "The answer is " + sum + " because if you multiply all the numbers from 1 to " + range + " you will get " + sum + ".";
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, sum + "");
    }

    void comleteFor()
    {
        int option = Random.Range(0, 2), range = Random.Range(5, 16);
        string opr = option == 0 ? "<" : "<=";
        string questionCode = "for (int i = 0; i " + opr + " _; i++) {\n\tSystem.out.println(i);\n}";
        string questionText = "complete the for so that it runs " + range + " times.";
        explanation = "The answer is " + (range - option) + " because the operator was " + opr + " and the for need to run " + range + " times.";
        List<int> answers = createAnswers(range - option, 20, 10);
        correctAnswer = range - option;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, (range - option) + "");
    }

    void breakFor()
    {
        int range = Random.Range(5, 16);
        int brFor = Random.Range(range / 2, range);
        string questionCode = "for (int i = 0; i < " + range + "; i++) {\n\tif (i == " + brFor + ") {\n\t\tbreak;\n\t}\n\tSystem.out.println(i);\n}";
        string questionText = "how many times the for will run?";
        explanation = "the for will run " + brFor + " times because when the for reach to that number the for will break.";
        List<int> answers = createAnswers(brFor, 20, 10);
        correctAnswer = brFor;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, brFor + "");
    }

    void continueFor()
    {
        int range = Random.Range(5, 16);
        int conFor = Random.Range(range / 2, range);
        string questionCode = "for (int i = 0; i < " + range + "; i++) {\n\tif (i == " + conFor + ") {\n\t\tcontinue;\n\t}\n\tSystem.out.println(i);\n}";
        string questionText = "which number will not be printed?";
        explanation = "the for will not print the number " + conFor + " because when the for reach to that number the for will skip to next number.";
        List<int> answers = createAnswers(conFor, 20, range - conFor);
        correctAnswer = conFor;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, conFor + "");
    }

    void calculateFor()
    {
        int range = Random.Range(2, 4), addF = Random.Range(2, 4), incF = Random.Range(1, 3), sum = 0;
        for (int i = 0; i < range; i += incF) 
        {
            sum += addF;
            sum *= 2;
        }
        string questionText = "What is the output?";
        string questionCode = "int sum = 0;\nfor (int i = 0; i < " + range + "; i += " + incF + ") {\n\tsum += " + addF + ";\n\tsum *= 2;\n}\nSystem.out.println(sum);";
        explanation = "The for runs " + ((range / incF) + (range % incF)) + " times and in each iteration the sum increase by " + addF + " and multiply by 2.";
        List<int> answers = createAnswers(sum, 20, 10);
        correctAnswer = sum;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, sum + "");
    }

    void countingFor()
    {
        int range = Random.Range(0, 10);
        string questionText = "How many times the for will run?";
        string questionCode = "for (int i = 0; i < " + range + "; i++) {\n\tSystem.out.println(i);\n}";
        explanation = "The for run " + range + " times because when the \'i\' equal to that number the for is finished.";
        List<int> answers = createAnswers(range, 20, 10);
        correctAnswer = range;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, range + "");
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
        {
            return;
        }
        if (firstFrame)
        {
            PauseMenu.canPause = false;
            pointGame[0].transform.position = enemyKeeperPos = startEnemyPos;
            boy.transform.position = startEnemyPos - new Vector3(0, 2, 0);
            player.transform.LookAt(pointGame[0].transform);
            arrow.transform.LookAt(pointGame[0].transform);
            firstFrame = false;
            funcs[Random.Range(0, 7)]();
        }
        if (SoccerQuestion.timeStatic == 0 && !SoccerQuestion.questionAnswered)
        {
            failSound.Play();
            timeText.color = Color.red;
            SoccerQuestion.ifSelectedOpion = SoccerQuestion.questionAnswered = true;
            turn = false;
            explanationText.text = "Time passed! the correct answer is " + correctAnswer + "\nExplanation: " + explanation;
        }
        if (haveAnswer)
        {
            CancelInvoke("reduceSecond");
            if (turn)
            {
                if (ifInitiateTurn)
                {
                    canvasSoccer.SetActive(true);
                    ifInitiateTurn = false;
                    arrow.SetActive(true);
                    pointGame[0].transform.position = startEnemyPos;
                    initiateTurn(0, 1, 0, new Vector3(0, 2.1f, 0), new Vector3(0, 2f, 0));
                }
                turnText.SetText("Your Turn");
                turnText.color = Color.green;
                playerTurn();
            }
            else
            {
                if (ifInitiateTurn)
                {
                    canvasSoccer.SetActive(true);
                    ifInitiateTurn = false;
                    arrow.SetActive(false);
                    initiateTurn(3, 2, 1, new Vector3(0, 2.1f, 0), new Vector3(0, 1.31f, -0.1f));
                    kick = false;
                    Invoke("npcKick", 2);
                }
                turnText.SetText("Opponent Turn");
                turnText.color = Color.red;
                npcTurn();
            }
        }
    }

    void playerTurn()
    {
        if (!kick && (Input.GetKey("right") || Input.GetKey("d")) && pointGame[0].transform.position.z < -12.6f)
        {
            pointGame[0].transform.position += new Vector3(0, 0, 0.2f);
        }
        if (!kick && (Input.GetKey("left") || Input.GetKey("a")) && pointGame[0].transform.position.z > -22.4f)
        {
            pointGame[0].transform.position -= new Vector3(0, 0, 0.2f);
        }
        if (!kick && Input.GetKey("space"))
        {
            kick = true;
            rnd = Random.Range(0, 4);
            rndf = Random.Range(-21.7f, -13.3f);
        }
        if (kick)
        {
            if (rnd == 0)
            {
                boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, rndf), Time.deltaTime * 5);
            }
            if (rnd == 2 || rnd == 1)
            {
                if (pointGame[0].transform.position.z > -13.3f)
                {
                    boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -13.3f), Time.deltaTime * 5);
                }
                else
                {
                    if (pointGame[0].transform.position.z < -21.7f)
                    {
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -21.7f), Time.deltaTime * 5);
                    }
                    else
                    {
                        float fixKeeperPosition = pointGame[0].transform.position.z - enemyKeeperPos.z;
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, pointGame[0].transform.position - new Vector3(0, 2.1f, fixKeeperPosition / 5), Time.deltaTime * 5);
                    }
                }
            }
            transform.Rotate(0, 13, 13);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, pointGame[0].transform.position - new Vector3(4, 1.5f, 0), Time.deltaTime * 10);
        }
        arrow.transform.LookAt(pointGame[0].transform);
    }

    void npcKick()
    {
        kick = true;
        rndf = Random.Range(-12.6f, -22.4f);
    }

    void npcTurn()
    {
        if ((Input.GetKey("right") || Input.GetKey("d")) && player.transform.position.z < -12.6f)
        {
            player.transform.position += new Vector3(0, 0, 0.2f);
        }
        if ((Input.GetKey("left") || Input.GetKey("a")) && player.transform.position.z > -22.4f)
        {
            player.transform.position -= new Vector3(0, 0, 0.2f);
        }
        if (kick)
        {
            transform.Rotate(0, 13, 13);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector3(player.transform.position.x + 4, player.transform.position.y, rndf), Time.deltaTime * 15);
        }
        /*else
        {
            transform.Rotate(0, 0, 0);
            ball.transform.position = ballPos[1].transform.position + new Vector3(0, 1.3f, -0.1f);
        }*/
    }

    void winOk()
    {
        AdminMission.endOk = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        canvasMission.SetActive(false);
        GameObject.Find("Player").GetComponent<Movement>().enabled = true;
        originalBoy.SetActive(Login.world != "City");
        ball.transform.position = ballPos[0].transform.position;
        boy.SetActive(false);
        Movement.missionInProgress = "";
        nextMission.SetActive(true);
        Movement.mission = nextMission;
        missionCompleteCanvas.SetActive(true);
        ball.GetComponent<SoccerMovment>().enabled = false;
    }

    void loseOK()
    {
        AdminMission.endOk = true;
        endGame = false;
        canvasMission.SetActive(false);
        CancelInvoke("reduceSecond");
        InvokeRepeating("reduceSecond", 1f, 1f);
        funcs[Random.Range(0, 7)]();
    }

    void reduceSecond()
    {
        if (SoccerQuestion.timeStatic > 0 && !SoccerQuestion.questionAnswered)
        {
            SoccerQuestion.timeStatic--;
        }
    }

    void triggerHit(Collider other, int addScore)
    {
        if (other.tag == "NPC" || other.tag == "Player")
        {
            kick = false;
        }
        if (other.tag == "StopLine")
        {
            if (addScore == 1)
            {
                cheerSound.Play();
            }
            else
            {
                booingSound.Play();
            }
            score += addScore;
            scoreText.text = score / 10 + " - " + score % 10;
            kick = false;
        }
        if ((turn && score % 10 == 3) || (!turn && score / 10 == 3))
        {
            endGame = true;
            canvasSoccer.SetActive(false);
            canvasMission.SetActive(true);
            PauseMenu.canPause = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            if (turn)
            {
                talkingText.text = "Conrgulations!! you Win!!! you can pass now to the next lesson.";
                Movement.npcMissionCounter++;
                practicalText.text = "";
                AdminMission.okFunc = winOk;
            }
            else
            {
                talkingText.text = "You lose. sorry but you need to try again.";
                score = 0;
                scoreText.text = "0 - 0";
                practicalText.text = "";
                AdminMission.okFunc = loseOK;
            }
        }
        else
        {
            canvasSoccer.SetActive(false);
            CancelInvoke("reduceSecond");
            InvokeRepeating("reduceSecond", 1f, 1f);
            funcs[Random.Range(0, 7)]();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (kick)
        {
            if (turn)
            {
                triggerHit(other, 1);
            }
            else
            {
                triggerHit(other, 10);
            }
            turn = !turn;
        }
    }

    private void OnDestroy()
    {
        PauseMenu.canPause = true;
    }
}
