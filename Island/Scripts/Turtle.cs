using UnityEngine;


//this script manage the turtles on the island
public class Turtle : MonoBehaviour
{
    public Animator myAnim;
    public bool shouldRest = false;

    private void Start()
    {
        ChangeFourAnimTo(false, false, shouldRest, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeFourAnimTo(false, false, false, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeFourAnimTo(false, false, shouldRest, false);
        }
    }

    private void ChangeFourAnimTo(bool dead, bool walk, bool rest, bool hide)
    {
        myAnim.SetBool("Dead", dead);
        myAnim.SetBool("Walk", walk);
        myAnim.SetBool("Rest", rest);
        myAnim.SetBool("Hide", hide);
    }


}
