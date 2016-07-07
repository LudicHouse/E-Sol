using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch : MonoBehaviour {
    public Transform smallGroup;
    public Transform shortGroup;
    public Transform mediumGroup;
    public Transform longGroup;
    public List<Sprite> bigLeavesList;

    private int bigLeavesIndex;
    private int smallIndex;
    private int shortIndex;
    private int mediumIndex;
    private int longIndex;

    public enum Anchor
    {
        Stem,
        Branch,
        Bush,
        Split
    }


	// Use this for initialization
	void Awake () {
        System.Random rand = Object.FindObjectOfType<GameController>().rand;

        bigLeavesIndex = rand.Next(0, bigLeavesList.Count);
        smallIndex = rand.Next(0, smallGroup.childCount);
        shortIndex = rand.Next(0, shortGroup.childCount);
        mediumIndex = rand.Next(0, mediumGroup.childCount);
        longIndex = rand.Next(0, longGroup.childCount);

        setStage(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Grow the branch to its next form.
    /// </summary>
    public void grow()
    {
        if (getStage() < 4)
        {
            setStage(getStage() + 1);
        }
    }

    /*/// <summary>
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
    }*/

    /// <summary>
    /// Grow the branch to the specified growth stage.
    /// </summary>
    /// <param name="growthStage">The one-based stage to grow the plant to.</param>
    private void setStage(int growthStage)
    {
        //Debug.Log("Growing branch from stage " + getStage() + " to stage " + growthStage);
        string accessory = Object.FindObjectOfType<MenuController>().targetAccessory;
        //Debug.Log("Currently targeted accessory is " + accessory);

        GameObject animal = getAnimal();
        Anchor anchor = Anchor.Branch;
        if (animal != null)
        {
            Transform parent = animal.transform.parent;
            if (parent.tag == "BranchAnchor")
            {
                anchor = Anchor.Branch;
            }
            else if (parent.tag == "BushAnchor")
            {
                anchor = Anchor.Bush;
            }
            else if (parent.tag == "StemAnchor")
            {
                anchor = Anchor.Stem;
            }
            else if (parent.tag == "SplitAnchor")
            {
                anchor = Anchor.Split;
            }
        }

        foreach (Transform branch in smallGroup)
        {
            branch.gameObject.SetActive(false);
        }
        foreach (Transform branch in shortGroup)
        {
            branch.gameObject.SetActive(false);
        }
        foreach (Transform branch in mediumGroup)
        {
            branch.gameObject.SetActive(false);
        }
        foreach (Transform branch in longGroup)
        {
            branch.gameObject.SetActive(false);
        }

        switch (growthStage)
        {
            case 1:
                smallGroup.GetChild(smallIndex).gameObject.SetActive(true);
                break;
            case 2:
                GameObject sBranch = shortGroup.GetChild(shortIndex).gameObject;
                sBranch.transform.FindChild("Leaves").GetComponent<SpriteRenderer>().sprite = bigLeavesList[bigLeavesIndex];
                sBranch.SetActive(true);
                break;
            case 3:
                GameObject mBranch = mediumGroup.GetChild(mediumIndex).gameObject;
                mBranch.transform.FindChild("Leaves").GetComponent<SpriteRenderer>().sprite = bigLeavesList[bigLeavesIndex];
                mBranch.SetActive(true);
                break;
            case 4:
                GameObject lBranch = longGroup.GetChild(longIndex).gameObject;
                lBranch.transform.FindChild("Leaves").GetComponent<SpriteRenderer>().sprite = bigLeavesList[bigLeavesIndex];
                lBranch.SetActive(true);
                break;
        }

        if (animal != null)
        {
            animal.transform.parent = getAnchor(anchor);
            animal.transform.localPosition = Vector3.zero;
        }

        int count = 0;
        foreach (AccessoryController ac in GetComponentsInChildren<AccessoryController>())
        {
            count++;
            ac.setAccessory(accessory, true);
        }
        //Debug.Log("Set accessory " + count + " times");
    }
    
    /// <summary>
    /// Get the current growth stage of the branch.
    /// </summary>
    /// <returns>The one-based stage of the branch.</returns>
    public int getStage()
    {
        foreach (Transform small in smallGroup)
        {
            if (small.gameObject.activeSelf == true)
            {
                return 1;
            }
        }

        foreach (Transform shortBranch in shortGroup)
        {
            if (shortBranch.gameObject.activeSelf == true)
            {
                return 2;
            }
        }

        foreach (Transform medium in mediumGroup)
        {
            if (medium.gameObject.activeSelf == true)
            {
                return 3;
            }
        }

        foreach (Transform longBranch in longGroup)
        {
            if (longBranch.gameObject.activeSelf == true)
            {
                return 4;
            }
        }

        return -1;
    }

    /// <summary>
    /// Get the currently active branch object.
    /// </summary>
    /// <returns>The transform component of the active branch.</returns>
    private Transform getActiveBranch()
    {
        switch (getStage())
        {
            case 1:
                return smallGroup.GetChild(smallIndex);
            case 2:
                return shortGroup.GetChild(shortIndex);
            case 3:
                return mediumGroup.GetChild(mediumIndex);
            case 4:
                return longGroup.GetChild(longIndex);
            default:
                return null;
        }
    }

    /*public void setAnimal(GameObject animal)
    {
        if (animal != null & getStage() > 1)
        {
            GameObject newAnimal = Object.Instantiate(animal);
            newAnimal.transform.parent = getActiveBranch().FindChild("Leaves");
            newAnimal.transform.localPosition = Vector3.zero;

            if (Random.Range(0, 2) == 0)
            {
                Vector3 newScale = newAnimal.transform.localScale;
                newScale.x *= -1;
                newAnimal.transform.localScale = newScale;
            }
        }
        else
        {
            Object.Destroy(getAnimal());
        }
    }*/

    /// <summary>
    /// Apply the selected animal to the branch.
    /// </summary>
    /// <param name="animal">The animal prefab to apply, or null to remove any existing animals.</param>
    /// <param name="anchor">The position on the branch to place the animal.</param>
    public void setAnimal(GameObject animal, Anchor anchor)
    {
        if (animal != null & getStage() > 1)
        {
            GameObject newAnimal = Object.Instantiate(animal);
            //newAnimal.transform.parent = getAnchor(anchor);
            newAnimal.transform.SetParent(getAnchor(anchor), false);
            newAnimal.transform.localPosition = Vector3.zero;
            

            if (anchor != Anchor.Stem && anchor != Anchor.Split && Random.Range(0, 2) == 0)
            {
                Vector3 newScale = newAnimal.transform.localScale;
                newScale.x *= -1;
                newAnimal.transform.localScale = newScale;
            }
        }
        else
        {
            Object.Destroy(getAnimal());
        }
    }

    /// <summary>
    /// Get the currently applied animal on this branch.
    /// </summary>
    /// <returns>The gameobject of the active animal.</returns>
    public GameObject getAnimal()
    {
        if (getStage() == 1 | getStage() == -1)
        {
            return null;
        }

        Transform branch = getAnchor(Anchor.Branch);
        if (branch.childCount > 0)
        {
            return branch.GetChild(0).gameObject;
        }

        Transform bush = getAnchor(Anchor.Bush);
        if (bush.childCount > 0)
        {
            return bush.GetChild(0).gameObject;
        }

        Transform stem = getAnchor(Anchor.Stem);
        if (stem.childCount > 0)
        {
            return stem.GetChild(0).gameObject;
        }

        Transform split = getAnchor(Anchor.Split);
        if (split.childCount > 0)
        {
            return split.GetChild(0).gameObject;
        }

        return null;
    }

    /// <summary>
    /// Get the specified anchor.
    /// </summary>
    /// <param name="a">The anchor position to get.</param>
    /// <returns>The transform component of the anchor.</returns>
    public Transform getAnchor(Anchor a)
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (a == Anchor.Branch & child.tag == "BranchAnchor")
            {
                return child;
            }
            else if (a == Anchor.Bush & child.tag == "BushAnchor")
            {
                return child;
            }
            else if (a == Anchor.Stem & child.tag == "StemAnchor")
            {
                return child;
            }
            else if (a == Anchor.Split & child.tag == "SplitAnchor")
            {
                return child;
            }
        }

        return null;
    }
}
