using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MoveTorch : MonoBehaviour
{

    public GameObject torch;
    public GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        torch.transform.position = GameObject.Find("FirstPersonController").transform.position + new Vector3(
        playerCamera.transform.forward.x,
        playerCamera.transform.forward.y,
        playerCamera.transform.forward.z);
    }




}
