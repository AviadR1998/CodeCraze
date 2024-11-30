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
    public AudioSource finishSound;
    List<questionFunc> funcs;

    string explanation;
    Vector3 enemyKeeperPos, startEnemyPos;
    int score, rnd, correctAnswer;
    float rndf;
    bool kick, endGame, firstFrame;

    const int UPPER_LIMIT_ANSWER = 20, DOWN_LIMIT_ANSWER = 10, SCORE = 10, TARGET_SCORE = 3, CAMERA_FAR = 21, CAMERA_NEAR = 60, ROTATE = 13, SPEED_BALL_BOY = 15, SPEED_BALL_PLAYER = 10;
    const int SUM_LOW = 3, SUM_HIGH = 8, FACT_LOW = 2, FACT_HIGH = 5, COMPLETE_HIGH1 = 2, COMPLETE_LOW = 5, COMPLETE_HIGH2 = 16, SPEED = 5, CALC_MAX1 = 4, CALC_MIN1 = 2, CALC_MAX2 = 3, CALC_MIN2 = 1;
    const int FIX_BALL_X = 4, CHANCE_SAVE = 4;
    const float PLAYER_Y_CICK = 2.1f, INIT_ARROW_X = 6.2f, INIT_ARROW_Y = 0.6f, INIT_ARROW_Z = -0.3f, BACK_POINT = 0.2f, MAX_PLAYER_KEEPER = -12.6f, MIN_PLAYER_KEEPER = -22.4f, MAX_BOY_KEEPER = -13.3f, MIN_BOY_KEEPER = -21.7f;
    const float FIX_BOY_Y = 2.1f, FIX_BALL_Y = 1.5f, BALL_START_Y = 1.31f, BALL_START_Z = -0.1f;
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
        initiateTurn(0, 1, 0, new Vector3(0, PLAYER_Y_CICK, 0), new Vector3(0, 2f, 0));
        InvokeRepeating("reduceSecond", 1f, 1f);
    }

    void initiateTurn(int i, int j, int ballI, Vector3 boyStart, Vector3 ballStart)
    {
        boy.transform.position = pointGame[i].transform.position - boyStart;
        player.transform.position = pointGame[j].transform.position;
        arrow.transform.position = pointGame[j].transform.position - new Vector3(INIT_ARROW_X, INIT_ARROW_Y, INIT_ARROW_Z);
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
        int range = Random.Range(SUM_LOW, SUM_HIGH), sum = 0;
        string questionCode = "int sum = 0;\nfor (int i = 1; i <= " + range + "; i++) {\n\tsum += i;\n}\nSystem.out.println(sum);";
        for (int i = 1; i <= range; i++)
        {
            sum += i;
        }
        correctAnswer = sum;
        List<int> answers = createAnswers(sum, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
        string questionText = "What is the output?";
        explanation = "The answer is " + sum + " because if you add all the numbers from 1 to " + range + " you will get " + sum + ".";
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, sum + "");
    }

    void factFor()
    {
        int range = Random.Range(FACT_LOW, FACT_HIGH), sum = 1;
        string questionCode = "int sum = 1;\nfor (int i = 1; i <= " + range + "; i++) {\n\tsum *= i;\n}\nSystem.out.println(sum);";
        for (int i = 2; i <= range; i++)
        {
            sum *= i;
        }
        correctAnswer = sum;
        List<int> answers = createAnswers(sum, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
        string questionText = "What is the output?";
        explanation = "The answer is " + sum + " because if you multiply all the numbers from 1 to " + range + " you will get " + sum + ".";
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, sum + "");
    }

    void comleteFor()
    {
        int option = Random.Range(0, COMPLETE_HIGH1), range = Random.Range(COMPLETE_LOW, COMPLETE_HIGH2);
        string opr = option == 0 ? "<" : "<=";
        string questionCode = "for (int i = 0; i " + opr + " _; i++) {\n\tSystem.out.println(i);\n}";
        string questionText = "complete the for so that it runs " + range + " times.";
        explanation = "The answer is " + (range - option) + " because the operator was " + opr + " and the for need to run " + range + " times.";
        List<int> answers = createAnswers(range - option, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
        correctAnswer = range - option;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, (range - option) + "");
    }

    void breakFor()
    {
        int range = Random.Range(COMPLETE_LOW, COMPLETE_HIGH2);
        int brFor = Random.Range(range / 2, range);
        string questionCode = "for (int i = 0; i < " + range + "; i++) {\n\tif (i == " + brFor + ") {\n\t\tbreak;\n\t}\n\tSystem.out.println(i);\n}";
        string questionText = "how many times the for will print?";
        explanation = "the for will run " + brFor + " times because when the for reach to that number the for will break.";
        List<int> answers = createAnswers(brFor, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
        correctAnswer = brFor;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, brFor + "");
    }

    void continueFor()
    {
        int range = Random.Range(COMPLETE_LOW, COMPLETE_HIGH2);
        int conFor = Random.Range(range / 2, range);
        string questionCode = "for (int i = 0; i < " + range + "; i++) {\n\tif (i == " + conFor + ") {\n\t\tcontinue;\n\t}\n\tSystem.out.println(i);\n}";
        string questionText = "which number will not be printed?";
        explanation = "the for will not print the number " + conFor + " because when the for reach to that number the for will skip to next number.";
        List<int> answers = createAnswers(conFor, UPPER_LIMIT_ANSWER, range - conFor);
        correctAnswer = conFor;
        questionSoccerCanvas.SetActive(true);
        SoccerQuestion.initiateQuestion(questionCode, questionText, explanation, answers, conFor + "");
    }

    void calculateFor()
    {
        int range = Random.Range(CALC_MIN1, CALC_MAX1), addF = Random.Range(CALC_MIN1, CALC_MAX1), incF = Random.Range(CALC_MIN2, CALC_MAX2), sum = 0;
        for (int i = 0; i < range; i += incF) 
        {
            sum += addF;
            sum *= 2;
        }
        string questionText = "What is the output?";
        string questionCode = "int sum = 0;\nfor (int i = 0; i < " + range + "; i += " + incF + ") {\n\tsum += " + addF + ";\n\tsum *= 2;\n}\nSystem.out.println(sum);";
        explanation = "The for runs " + ((range / incF) + (range % incF)) + " times and in each iteration the sum increase by " + addF + " and multiply by 2.";
        List<int> answers = createAnswers(sum, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
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
        List<int> answers = createAnswers(range, UPPER_LIMIT_ANSWER, DOWN_LIMIT_ANSWER);
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
            funcs[Random.Range(0, funcs.Count)]();
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
                    initiateTurn(0, 1, 0, new Vector3(0, PLAYER_Y_CICK, 0), new Vector3(0, 2f, 0));
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
                    initiateTurn(3, 2, 1, new Vector3(0, PLAYER_Y_CICK, 0), new Vector3(0, BALL_START_Y, BALL_START_Z));
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
        if (!kick && (Input.GetKey("right") || Input.GetKey("d")) && pointGame[0].transform.position.z < MAX_PLAYER_KEEPER)
        {
            pointGame[0].transform.position += new Vector3(0, 0, BACK_POINT);
        }
        if (!kick && (Input.GetKey("left") || Input.GetKey("a")) && pointGame[0].transform.position.z > MIN_PLAYER_KEEPER)
        {
            pointGame[0].transform.position -= new Vector3(0, 0, BACK_POINT);
        }
        if (!kick && Input.GetKey("space"))
        {
            kick = true;
            rnd = Random.Range(0, CHANCE_SAVE);
            rndf = Random.Range(MIN_BOY_KEEPER, MAX_BOY_KEEPER);
        }
        if (kick)
        {
            if (rnd == 0)
            {
                boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, rndf), Time.deltaTime * SPEED);
            }
            if (rnd == 2 || rnd == 1)
            {
                if (pointGame[0].transform.position.z > MAX_BOY_KEEPER)
                {
                    boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, MAX_BOY_KEEPER), Time.deltaTime * SPEED);
                }
                else
                {
                    if (pointGame[0].transform.position.z < MIN_BOY_KEEPER)
                    {
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, MIN_BOY_KEEPER), Time.deltaTime * SPEED);
                    }
                    else
                    {
                        float fixKeeperPosition = pointGame[0].transform.position.z - enemyKeeperPos.z;
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, pointGame[0].transform.position - new Vector3(0, FIX_BOY_Y, fixKeeperPosition / 5), Time.deltaTime * SPEED);
                    }
                }
            }
            transform.Rotate(0, ROTATE, ROTATE);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, pointGame[0].transform.position - new Vector3(FIX_BALL_X, FIX_BALL_Y, 0), Time.deltaTime * SPEED_BALL_PLAYER);
        }
        arrow.transform.LookAt(pointGame[0].transform);
    }

    void npcKick()
    {
        kick = true;
        rndf = Random.Range(MAX_PLAYER_KEEPER, MIN_PLAYER_KEEPER);
    }

    void npcTurn()
    {
        if ((Input.GetKey("right") || Input.GetKey("d")) && player.transform.position.z < MAX_PLAYER_KEEPER)
        {
            player.transform.position += new Vector3(0, 0, BACK_POINT);
        }
        if ((Input.GetKey("left") || Input.GetKey("a")) && player.transform.position.z > MIN_PLAYER_KEEPER)
        {
            player.transform.position -= new Vector3(0, 0, BACK_POINT);
        }
        if (kick)
        {
            transform.Rotate(0, ROTATE, ROTATE);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector3(player.transform.position.x + FIX_BALL_X, player.transform.position.y, rndf), Time.deltaTime * SPEED_BALL_BOY);
        }
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
        finishSound.Play();
        missionCompleteCanvas.SetActive(true);
        ball.GetComponent<SoccerMovment>().enabled = false;
        camera.GetComponent<Camera>().fieldOfView = CAMERA_NEAR;
    }

    void loseOK()
    {
        AdminMission.endOk = true;
        endGame = false;
        canvasMission.SetActive(false);
        CancelInvoke("reduceSecond");
        InvokeRepeating("reduceSecond", 1f, 1f);
        funcs[Random.Range(0, funcs.Count)]();
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
            scoreText.text = score / SCORE + " - " + score % SCORE;
            kick = false;
        }
        if ((turn && score % SCORE == TARGET_SCORE) || (!turn && score / SCORE == TARGET_SCORE))
        {
            endGame = true;
            canvasSoccer.SetActive(false);
            canvasMission.SetActive(true);
            PauseMenu.canPause = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            if (turn)
            {
                talkingText.text = "Congratulations!! you Win!!! you can pass now to the next lesson.";
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
            funcs[Random.Range(0, funcs.Count)]();
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
                triggerHit(other, SCORE);
            }
            turn = !turn;
        }
    }

    private void OnDestroy()
    {
        PauseMenu.canPause = true;
    }
}
