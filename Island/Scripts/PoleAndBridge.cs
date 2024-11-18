using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAndBridge : MonoBehaviour
{

    public int NeededPears = 0, NumOfCollectedPears = 0;
    public bool HasChildrenWithZeroPears = false;
    private bool AllPearsCollected = false, ZeroPearsCollected = false;

    // Update is called once per frame
    void Update()
    {
        if (!AllPearsCollected && NeededPears == NumOfCollectedPears && (!HasChildrenWithZeroPears || ZeroPearsCollected))
        {
            print("In Pole And Bridge Update");
            foreach (Transform child in transform)
            {
                if (child.name == "pear")
                {
                    child.gameObject.SetActive(true);
                }
            }
            AllPearsCollected = true;
        }
    }

    public void PearCollected()
    {
        NumOfCollectedPears++;
    }

    public void CollectZeroPears()
    {
        if (HasChildrenWithZeroPears)
        {
            ZeroPearsCollected = true;
        }
    }


}
