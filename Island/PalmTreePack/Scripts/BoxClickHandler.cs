using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxClickHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            print("ZZZ");
            animator.SetTrigger("OpenBox");
        }
    }
}

