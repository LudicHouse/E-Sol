using UnityEngine;
using System.Collections;

public class AnimalTutorial : MonoBehaviour {
    public GrowBranches branchContainer;
    public TutorialManager manager;
    public Vector2 posOffset;

    private bool positionSet = false;
    private Branch selectedBranch;
    private Plant p;
    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        p = Object.FindObjectOfType<Plant>();
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (positionSet == false)
        {
            if (branchContainer.transform.childCount > 0)
            {
                foreach (Branch b in branchContainer.GetComponentsInChildren<Branch>())
                {
                    if (b.getStage() >= 2)
                    {
                        selectedBranch = branchContainer.sortBranches()[0].GetComponent<Branch>();
                        positionSet = true;
                        break;
                    }
                }
            }
        }

        if (positionSet == true)
        {
            transform.position = selectedBranch.getAnchor(Branch.Anchor.Bush).position + (Vector3)posOffset;

            if (p.getNumAnimals() > 0 & manager.showAnimTutorial() == true)
            {
                rend.enabled = true;
            }
            else
            {
                rend.enabled = false;
            }
        }
        else
        {
            rend.enabled = false;
        }
	}
}
