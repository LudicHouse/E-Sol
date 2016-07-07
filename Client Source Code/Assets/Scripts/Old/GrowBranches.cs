using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrowBranches : MonoBehaviour {
    public Transform flower;
    public int regionsUntilFirstBranch;
    public int regionsUntilNewBranch;
    public int regionsUntilGrow;
    public int initialRegion;
    public GameObject leftBranchPrefab;
    public GameObject rightBranchPrefab;

    private int nextRegion;
    private int nextHeight;
    private System.Random growRand;
    private Dictionary<Transform, float> distanceBelowHead;

	// Use this for initialization
	void Start () {
        //Debug.Log("Branch started");
        nextRegion = regionsUntilFirstBranch;
        nextHeight = initialRegion;
        growRand = new System.Random(Object.FindObjectOfType<GameController>().rand.Next());
        distanceBelowHead = new Dictionary<Transform, float>();
	}
	
	// Update is called once per frame
	void Update () { //Note to self: Could cause issues with the script execution order (re: plant stem growth) so keep an eye on this
        while (flower.position.y >= nextRegion)
        {
            //Debug.Log("Spawning");
            spawnBranch();

            nextRegion += regionsUntilNewBranch;
            nextHeight += regionsUntilNewBranch;
        }

        foreach (Transform branch in sortBranches())
        {
            if (branch.GetComponent<Branch>() != null)
            {
                if (flower.position.y - regionsUntilFirstBranch + initialRegion >= branch.localPosition.y + regionsUntilGrow & branch.GetComponent<BranchOld>().hasGrown() == false)
                {
                    //Debug.Log("Grew");
                    //Debug.Log("Growing branch " + branch.localPosition.y);
                    growBranch(branch);
                }
            }
        }
	}

    /// <summary>
    /// Spawns a new branch or branches on the plant.
    /// </summary>
    private void spawnBranch()
    {
        System.Random rand = Object.FindObjectOfType<GameController>().rand;
        int offset = Mathf.FloorToInt(flower.position.y - nextRegion + 0.5f); //TODO: Double check this solution!
        //Debug.Log("Next height is " + nextHeight + ", offset is " + offset + ", checking at " + (nextHeight + offset) + " (" + (flower.position.y - (nextHeight + offset)) + ").");

        int side = rand.Next(-1, 2);

        if (side <= 0)
        {
            GameObject leftBranch = Object.Instantiate(leftBranchPrefab);
            leftBranch.transform.parent = transform;

            Vector2 newPos = new Vector2();
            //Debug.Log(flower.GetComponent<Plant>().numLeftStems(nextHeight + offset) + " stems on left.");
            newPos.x = -0.25f * rand.Next(0, flower.GetComponent<Plant>().numLeftStems(nextHeight + offset) + 1);
            //newPos.y = nextHeight;
            newPos.y = (float)rand.NextDouble() + nextHeight;
            leftBranch.transform.localPosition = newPos;

            distanceBelowHead.Add(leftBranch.transform, flower.position.y - (nextHeight + offset));
        }
        
        if (side >= 0)
        {
            GameObject rightBranch = Object.Instantiate(rightBranchPrefab);
            rightBranch.transform.parent = transform;

            Vector2 newPos = new Vector2();
            //Debug.Log(flower.GetComponent<Plant>().numRightStems(nextHeight + offset) + " stems on left.");
            newPos.x = 0.25f * rand.Next(0, flower.GetComponent<Plant>().numRightStems(nextHeight + offset) + 1);
            //newPos.y = nextHeight;
            newPos.y = (float)rand.NextDouble() + nextHeight;
            rightBranch.transform.localPosition = newPos;

            distanceBelowHead.Add(rightBranch.transform, flower.position.y - (nextHeight + offset));
        }
    }

    /// <summary>
    /// Grow a branch to its larger size.
    /// </summary>
    /// <param name="branch">The branch to grow.</param>
    private void growBranch(Transform branch)
    {
        int checkRegion = Mathf.FloorToInt(flower.position.y - distanceBelowHead[branch] - regionsUntilGrow);

        if (branch.tag == "LeftBranch")
        {
            Vector2 newPos = branch.localPosition;
            if ((newPos.x / -0.25f) < flower.GetComponent<Plant>().numLeftStems(checkRegion))
            {
                newPos.x = -0.25f * growRand.Next((int)(newPos.x / -0.25f) + 1, flower.GetComponent<Plant>().numLeftStems(checkRegion) + 1);
            }
            branch.localPosition = newPos;
        }
        else
        {
            Vector2 newPos = branch.localPosition;
            if ((newPos.x / 0.25f) < flower.GetComponent<Plant>().numRightStems(checkRegion))
            {
                newPos.x = 0.25f * growRand.Next((int)(newPos.x / 0.25f) + 1, flower.GetComponent<Plant>().numRightStems(checkRegion) + 1);
            }
            branch.localPosition = newPos;
        }

        branch.GetComponent<Branch>().grow();
    }

    /// <summary>
    /// Sorts the branches in order of lowest to highest.
    /// </summary>
    /// <returns>The sorted list.</returns>
    public List<Transform> sortBranches() //Note to self: This could end up making some CPU strain with high numbers of branches. Look into finding a better solution
    {
        List<Transform> output = new List<Transform>();
        foreach (Transform branch in transform)
        {
            output.Add(branch);
        }

        bool sorted = false;
        while (sorted == false)
        {
            sorted = true;
            for (int loop = 0; loop < output.Count - 1; loop++)
            {
                if (output[loop].localPosition.y > output[loop + 1].localPosition.y)
                {
                    Transform temp = output[loop];
                    output[loop] = output[loop + 1];
                    output[loop + 1] = temp;
                    sorted = false;
                }
            }
        }

        return output;
    }
}
