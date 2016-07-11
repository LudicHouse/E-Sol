using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PrototypeMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadPlantPhys()
	{
		SceneManager.LoadScene("Plant Physics Test");
	}
}
