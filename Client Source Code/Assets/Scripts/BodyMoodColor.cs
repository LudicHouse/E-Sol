using UnityEngine;
using System.Collections;

public class BodyMoodColor : MonoBehaviour {
    private Plant p;
    private SpriteRenderer rend;
    private float initialRed;

	// Use this for initialization
    void Start()
    {
        p = Object.FindObjectOfType<Plant>();
        rend = GetComponent<SpriteRenderer>();
        initialRed = rend.color.r;
    }
	
	// Update is called once per frame
	void Update () {
        float diff = 1 - p.plantHydration;

        Color newColor = rend.color;
        newColor.r = initialRed + (Mathf.Clamp(diff, 0, 1) * (1f - initialRed));
        rend.color = newColor;
	}

    /// <summary>
    /// Reset the initial colour settings of the sprite (in the event that the sprite colour is changed elsewhere in the code).
    /// </summary>
    public void reset()
    {
        if (rend != null)
        {
            initialRed = rend.color.r;
        }
    }
}
