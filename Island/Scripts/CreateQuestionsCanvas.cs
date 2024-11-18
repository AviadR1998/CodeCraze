using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuestionsCanvas : MonoBehaviour
{
    public Canvas QCanvas;
    private Canvas ClonedCanvas;
    void Start()
    {
        //CreateQCanvas("Assets\\Island\\data\\ques.csv");

    }

    public void CreateQCanvas(string csvPath)
    {
        ClonedCanvas = Instantiate(QCanvas);

        if (ClonedCanvas != null)
        {
            ClonedCanvas.gameObject.SetActive(true);
            ClonedCanvas.GetComponent<QuestionCanvas>().StartAsking(csvPath);

            Transform CameraControl = transform.parent.Find("CameraControlScript");
            if (CameraControl != null)
            {
                CameraControl.GetComponent<CanvasCameraControl>().AddCanvas(ClonedCanvas);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
