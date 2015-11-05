using UnityEngine;
using System.Collections;

public class PestTutorial : MonoBehaviour {
    public GameObject pestContainer;
    public TutorialManager manager;

    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (pestContainer.GetComponentsInChildren<Pest>().Length > 0 & manager.showPestTutorial() == true)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
	}
}
