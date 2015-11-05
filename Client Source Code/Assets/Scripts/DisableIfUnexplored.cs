using UnityEngine;
using System.Collections;

public class DisableIfUnexplored : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string code = Country.getCode(name);

        if (Country.isExplored(code) == false)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
