using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetExploredColor : MonoBehaviour {
    public Color unexploredColor;
    public Color exploredColor;

	// Use this for initialization
	void Start () {
        string code = Country.getCode(name);
        Image img = GetComponent<Image>();

        if (Country.isExplored(code) == true)
        {
            img.color = exploredColor;
        }
        else
        {
            img.color = unexploredColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
