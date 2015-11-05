using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchOld : MonoBehaviour {
    public GameObject smallBranch;
    public GameObject bigBranch;
    public GameObject bigLeaves;
    public List<Sprite> bigLeavesList;

	// Use this for initialization
	void Awake () {
        System.Random rand = Object.FindObjectOfType<GameController>().rand;
        Sprite randLeaves = bigLeavesList[rand.Next(0, bigLeavesList.Count)];
        bigLeaves.GetComponent<SpriteRenderer>().sprite = randLeaves;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Grow the branch to its larger form.
    /// </summary>
    public void grow()
    {
        smallBranch.SetActive(false);
        bigBranch.SetActive(true);
    }

    /// <summary>
    /// Checks whether the branch is fully grown.
    /// </summary>
    /// <returns>True if a large branch, false otherwise.</returns>
    public bool hasGrown()
    {
        if (bigBranch.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
