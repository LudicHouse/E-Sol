using UnityEngine;
using System.Collections;

public class RandomBackgroundBranch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        System.Random rand = new System.Random(Object.FindObjectOfType<GameController>().rand.Next());
        int branchIndex = rand.Next(1, GetComponentsInChildren<Transform>(true).Length);
        //Debug.Log("Setting background branch " + branchIndex);
        GetComponentsInChildren<Transform>(true)[branchIndex].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
