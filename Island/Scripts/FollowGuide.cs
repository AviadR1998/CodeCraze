using UnityEngine;


//This script controls the arrow floats and follow above an npc
public class FollowGuide : MonoBehaviour
{
    public GameObject npc;
    public float addToX = 0.528f, addToY = 5f, addToZ = 0.746f;

    // Update is called once per frame
    void Update()
    {
        Vector3 lastPos = npc.transform.position;
        transform.position = new Vector3(lastPos.x + addToX, lastPos.y + addToY, lastPos.z + addToZ);
    }
}
