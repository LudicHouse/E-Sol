using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        System.Random rand = Object.FindObjectOfType<GameController>().rand;

        Color newColor = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
        //Color newColor = new Color((float)rand.NextDouble(), 0, (float)rand.NextDouble());
        //newColor.g = (float)(rand.NextDouble() * Mathf.Max(newColor.r, newColor.b));

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in renderers)
        {
            r.color = newColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
