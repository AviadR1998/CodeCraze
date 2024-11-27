using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideNpc : MonoBehaviour
{
    public GameObject obj, player, qScriptObj, gameFlowControler;
    public GameObject[] points;
    public Animator animator; // Animator component reference
    public Canvas guideCanvas, welcomeCanvas;
    public Button nextWelcome, toQuestionsBtn;
    public TextMeshProUGUI guideTxt;
    public float speed = 1;
    public GameObject boatStatueArrow, boatIslandArrow;
    private Vector3 actualPoints;
    private int nextPosition;
    private bool isPlayerNearby = false, qCanvasOn = false;
    private string questionsPath;
    private Canvas currentQCanvas;

    void Start()
    {
        nextPosition = 0;
        animator = obj.GetComponent<Animator>(); // Get Animator component from the GameObject
        nextWelcome.onClick.AddListener(ClickNextOnWelcomePage);
        toQuestionsBtn.onClick.AddListener(StartQuestions);
    }

    void Update()
    {
        if (!isPlayerNearby)
        {
            float tresh = 0.03f;
            float minusTresh = -0.03f;
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
        if (other.gameObject == player)
        {
            isPlayerNearby = true;
            animator.speed = 0; // Stop the animation
            ChangeGuideTextAndSetQue();

            if (GameFlow.mission == 0)
            {
                if (GameFlow.stateInMission == 0)
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


            if (GameFlow.mission == 1)
            {
                boatIslandArrow.SetActive(true);
            }
            else if (GameFlow.mission == 2)
            {
                boatStatueArrow.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearby = false;
            animator.speed = 1; // Resume the animation
            guideCanvas.gameObject.SetActive(false);
            welcomeCanvas.gameObject.SetActive(false);
        }
    }


    private void ChangeGuideTextAndSetQue()
    {
        int currMission = GameFlow.mission;
        int currStateInMission = GameFlow.stateInMission;

        string newGuideTxt;

        switch (currMission)
        {
            case 0:
                if (currStateInMission == 0)
                {
                    newGuideTxt = "Welcome to your first mission! To begin, search the island for a floating old coin.";
                }
                else
                {
                    newGuideTxt = "Great job completing the Functions mission! Now, let's test your"
                                    + " knowledge with a few questions about Functions.";
                    questionsPath = "Assets\\Island\\data\\q_functions.csv";
                }
                break;

            case 1:
                if (currStateInMission == 0)
                {
                    newGuideTxt = "To start the next mission, find a way to reach the second island."
                                    + " There, you'll learn about Classes and Objects—the core of Java programming.";
                }
                else
                {
                    newGuideTxt = "Excellent work on the Classes and Objects mission! Let’s solidify your"
                                    + " learning with some questions.";
                    questionsPath = "Assets\\Island\\data\\q_classes.csv";
                }
                break;

            case 2:
                if (currStateInMission == 0)
                {
                    newGuideTxt = "Ready for your next adventure? Find a way to reach"
                                    + " the horse statue to begin the Recursion mission.";
                }
                else
                {
                    newGuideTxt = "Amazing job completing the Recursion mission! Let’s test your understanding with some questions.";
                    questionsPath = "Assets\\Island\\data\\q_recursion.csv";
                }
                break;

            default:
                newGuideTxt = "You've completed all your missions! Feel free to revisit any"
                                + " mission to replay it or take some time to explore the island";
                break;
        }


        guideTxt.text = newGuideTxt;
    }


    private void ClickNextOnWelcomePage()
    {
        GetComponent<BlockPlayerCamera>().resumeCamera();
    }


    private void StartQuestions()
    {
        currentQCanvas = qScriptObj.GetComponent<CreateQuestionsCanvas>().CreateQCanvas(questionsPath);
        qCanvasOn = true;
    }
}

