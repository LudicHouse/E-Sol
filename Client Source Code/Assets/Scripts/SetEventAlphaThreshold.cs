using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetEventAlphaThreshold : MonoBehaviour {
    public float alpha;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().eventAlphaThreshold = alpha;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().eventAlphaThreshold = alpha;
	}
}
