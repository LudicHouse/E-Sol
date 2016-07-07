using UnityEngine;
using System.Collections;

public class Stem : MonoBehaviour {
    public GameObject stemPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Add a new stem segment to the stem.
    /// </summary>
    public void addStem()
    {
        GameObject newStem = Object.Instantiate(stemPrefab);
        newStem.transform.parent = transform;
        newStem.transform.localPosition = new Vector2(0, 1 - transform.childCount);
        newStem.transform.localScale = Vector3.one;
    }
}
