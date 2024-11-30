using UnityEngine;



//This script instantiate a clone of the questions canvas and start the questions
public class CreateQuestionsCanvas : MonoBehaviour
{
    public Canvas QCanvas;
    public bool useCameraScript = false;
    private Canvas ClonedCanvas;

    public Canvas CreateQCanvas(string csvPath, string title = "Questions")
    {
        ClonedCanvas = Instantiate(QCanvas);

        if (ClonedCanvas != null)
        {
            print("Q_canvas Active!");
            ClonedCanvas.gameObject.SetActive(true);
            ClonedCanvas.GetComponent<QuestionCanvas>().StartAsking(csvPath, title);

            if (useCameraScript)
            {
                Transform CameraControl = transform.parent.Find("CameraControlScript");
                if (CameraControl != null)
                {
                    CameraControl.GetComponent<CanvasCameraControl>().AddCanvas(ClonedCanvas);
                }
            }

        }
        return ClonedCanvas;
    }

}
