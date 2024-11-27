using UnityEngine;

public class CreateQuestionsCanvas : MonoBehaviour
{
    public Canvas QCanvas;
    private Canvas ClonedCanvas;
    void Start()
    {
        //CreateQCanvas("Assets\\Island\\data\\q_recursion.csv");

    }

    public Canvas CreateQCanvas(string csvPath)
    {
        ClonedCanvas = Instantiate(QCanvas);

        if (ClonedCanvas != null)
        {
            print("Q_canvas Active!");
            ClonedCanvas.gameObject.SetActive(true);
            ClonedCanvas.GetComponent<QuestionCanvas>().StartAsking(csvPath);

            Transform CameraControl = transform.parent.Find("CameraControlScript");
            if (CameraControl != null)
            {
                CameraControl.GetComponent<CanvasCameraControl>().AddCanvas(ClonedCanvas);
            }
        }
        return ClonedCanvas;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
