using UnityEngine;


//This script manage single brown pole and bridge off the recursion mission
public class PoleAndBridge : MonoBehaviour
{

    public int NeededPears = 0, NumOfCollectedPears = 0;
    public bool HasChildrenWithZeroPears = false;
    public bool AllPearsCollected = false, ZeroPearsCollected = false;

    // Update is called once per frame
    void Update()
    {
        if (!AllPearsCollected && NeededPears == NumOfCollectedPears && (!HasChildrenWithZeroPears || ZeroPearsCollected))
        {
            print("In Pole And Bridge Update");
            SetChildPearsTo(true);
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

    public void SetChildPearsTo(bool setTo)
    {
        foreach (Transform child in transform)
        {
            if (child.name == "pear")
            {
                child.gameObject.SetActive(setTo);
            }
        }
    }


}
