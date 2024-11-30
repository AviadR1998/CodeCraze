using UnityEngine;


//This script controls the animation of the dragon of recursion mission
public class DragonAnomation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the dragon
        animator = GetComponent<Animator>();
        PlayAnimation("sleep");
    }

    private void Update()
    {
        PlayAnimation("sleep");
    }

    public void PlayAnimation(string animationName)
    {
        // Trigger the animation by name
        animator.Play(animationName);
    }

}
