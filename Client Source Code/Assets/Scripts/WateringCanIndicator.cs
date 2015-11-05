using UnityEngine;
using System.Collections;

public class WateringCanIndicator : MonoBehaviour {
    public WateringCan can;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 newScale = transform.localScale;
        newScale.y = can.water / can.maxWater;
        transform.localScale = newScale;
	}
}
