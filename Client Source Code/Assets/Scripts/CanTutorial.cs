using UnityEngine;
using System.Collections;

public class CanTutorial : MonoBehaviour {
    public WateringCan can;
    public TutorialManager manager;

    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (can.water >= can.maxWater & manager.showCanTutorial() == true)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }

        //Debug.Log("Can tutorial: " + rend.enabled);
	}
}
