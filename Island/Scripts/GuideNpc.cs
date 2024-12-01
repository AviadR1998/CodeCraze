using TMPro;
using UnityEngine;
using UnityEngine.UI;


//This script manage the whole guide logics
public class GuideNpc : MonoBehaviour
{
    public GameObject obj, player, qScriptObj, gameFlowControler;
    public GameObject[] points;
    public Animator animator; // Animator component reference
    public Canvas guideCanvas, welcomeCanvas;
    public Button nextWelcome, toQuestionsBtn, chooseQuesBtn;
    public TextMeshProUGUI guideTxt;
    public float speed = 1;
    public GameObject boatStatueArrow, boatIslandArrow;
    public float tresh = 0.03f, minusTresh = -0.03f;
    public string fMissionTitle = "Functions", cMissionTitle = "Classes", rMissionTitle = "Recursion";
    private Vector3 actualPoints;
    private int nextPosition;
    private bool isPlayerNearby = false, qCanvasOn = false, canAnswer = true;
    private string questionsPath, questionsTitle;
    private Canvas currentQCanvas;
    string firstMissionString = "Welcome to your first mission! To begin, search the island for a floating old coin.";
    string completeFMission = "Great job completing the Functions mission! Now, let's test your"
                                + " knowledge with a few questions about Functions.";
    string completeCMission = "Excellent work on the Classes and Objects mission! Let’s solidify your"
                                + " learning with some questions.";
    string completeRMission = "Amazing job completing the Recursion mission! Let’s test your understanding with some questions.";
    string fPath = "functionsCSV.csv";
    string cPath = "classesCSV.csv";
    string rPath = "recursionCSV.csv";
    private int beforeQuestions = 0, functionMissionIndex = 0, classMissionIndex = 1, recursionMissionIndex = 2,
                afterQuestions = 1;

    void Start()
    {
        nextPosition = 0;
        animator = obj.GetComponent<Animator>(); // Get Animator component from the GameObject
        nextWelcome.onClick.AddListener(ClickNextOnWelcomePage);
        toQuestionsBtn.onClick.AddListener(StartQuestions);
    }

    void Update()
    {
        if (!isPlayerNearby || !canAnswer)
        {

            actualPoints = obj.transform.position;
            obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);

            float yCoard = (actualPoints - points[nextPosition].transform.position).y;
            float xCoard = (actualPoints - points[nextPosition].transform.position).x;
            float zCoard = (actualPoints - points[nextPosition].transform.position).z;

            if ((xCoard <= tresh && xCoard >= minusTresh) && (zCoard <= tresh && zCoard >= minusTresh) && (yCoard <= tresh && yCoard >= minusTresh))
            {
                nextPosition++;
                if (nextPosition == points.Length)
                {
                    nextPosition = 0;
                }
                obj.transform.LookAt(points[nextPosition].transform);
            }
        }

        if (qCanvasOn && currentQCanvas && !currentQCanvas.gameObject.activeInHierarchy)
        {
            gameFlowControler.GetComponent<GameFlow>().FinishedAMission();
            qCanvasOn = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !qCanvasOn && canAnswer)
        {
            isPlayerNearby = true;
            animator.speed = 0; // Stop the animation
            ChangeGuideTextAndSetQue();

            if (!GameFlow.finishAllMissions)
            {
                if (GameFlow.mission == functionMissionIndex)
                {
                    if (GameFlow.stateInMission == beforeQuestions)
                    {
                        welcomeCanvas.gameObject.SetActive(true);
                    }
                    else
                    {
                        guideCanvas.gameObject.SetActive(true);
                        toQuestionsBtn.gameObject.SetActive(true);
                    }

                    GetComponent<BlockPlayerCamera>().stopCamera();
                }
                else
                {
                    guideCanvas.gameObject.SetActive(true);
                }


                if (GameFlow.mission == classMissionIndex)
                {
                    boatIslandArrow.SetActive(true);
                    if (GameFlow.stateInMission == afterQuestions)
                    {
                        GetComponent<BlockPlayerCamera>().stopCamera();
                        toQuestionsBtn.gameObject.SetActive(true);
                    }
                }
                else if (GameFlow.mission == recursionMissionIndex)
                {
                    boatStatueArrow.SetActive(true);
                    if (GameFlow.stateInMission == afterQuestions)
                    {
                        GetComponent<BlockPlayerCamera>().stopCamera();
                        toQuestionsBtn.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                boatIslandArrow.SetActive(true);
                boatStatueArrow.SetActive(true);
                guideCanvas.gameObject.SetActive(true);
                chooseQuesBtn.gameObject.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            canAnswer = false;
            Invoke("NpcCanAnswer", 3f);
            isPlayerNearby = false;
            animator.speed = 1; // Resume the animation
            if (!toQuestionsBtn.gameObject.activeInHierarchy)
            {
                guideCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void NpcCanAnswer()
    {
        canAnswer = true;
    }


    private void ChangeGuideTextAndSetQue()
    {
        int currMission = GameFlow.mission;
        int currStateInMission = GameFlow.stateInMission;
        string newGuideTxt;

        if (!GameFlow.finishAllMissions)
        {
            switch (currMission)
            {
                case 0:
                    if (currStateInMission == beforeQuestions)
                    {
                        newGuideTxt = firstMissionString;
                    }
                    else
                    {
                        newGuideTxt = completeFMission;
                        questionsPath = fPath;
                        questionsTitle = fMissionTitle;
                    }
                    break;

                case 1:
                    if (currStateInMission == beforeQuestions)
                    {
                        newGuideTxt = "To start the next mission, find a way to reach the second island." +
                                    "\nOnce there, look for Salvador—he will teach you about Classes and " +
                                    "Objects, the core of Java programming.";
                    }
                    else
                    {
                        newGuideTxt = completeCMission;
                        questionsPath = cPath;
                        questionsTitle = cMissionTitle;
                    }
                    break;

                case 2:
                    if (currStateInMission == beforeQuestions)
                    {
                        newGuideTxt = "Ready for your next adventure? Find a way to reach"
                                        + " the horse statue to begin the Recursion mission.";
                    }
                    else
                    {
                        newGuideTxt = completeRMission;
                        questionsPath = rPath;
                        questionsTitle = rMissionTitle;
                    }
                    break;

                default:
                    newGuideTxt = firstMissionString;
                    break;
            }
        }
        else
        {
            newGuideTxt = "You've completed all your missions! Feel free to revisit any"
                            + " mission to replay it or take some time to explore the island";
        }



        guideTxt.text = newGuideTxt;
    }


    private void ClickNextOnWelcomePage()
    {
        GetComponent<BlockPlayerCamera>().resumeCamera();
    }


    private void StartQuestions()
    {
        currentQCanvas = qScriptObj.GetComponent<CreateQuestionsCanvas>().CreateQCanvas(questionsPath, questionsTitle);
        qCanvasOn = true;
    }

    public void ChooseQuestions(int quesIndex)
    {
        if (quesIndex == functionMissionIndex)
        {
            questionsPath = fPath;
            questionsTitle = fMissionTitle;
        }
        else if (quesIndex == classMissionIndex)
        {
            questionsPath = cPath;
            questionsTitle = cMissionTitle;
        }
        else
        {
            questionsPath = rPath;
            questionsTitle = rMissionTitle;
        }

        StartQuestions();
    }
}

