using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameFlow : MonoBehaviour
{
    public GameObject gameFlow;
    private void OnTriggerEnter(Collider other)
    {
        gameFlow.GetComponent<GameFlow>().StartRecursionMission();
    }

}
