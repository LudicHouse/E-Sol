using UnityEngine;
using System.Collections;

public class SmogTutorial : MonoBehaviour {
    public Transform smogContainer;
    public TutorialManager manager;

    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (smogContainer.childCount > 0 & manager.showSmogTutorial() == true)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
	}
}
