using UnityEngine;
using System.Collections;

public class FlipParticles : MonoBehaviour {
    public bool left = false;

    //private float startSpeed;
    private float startRot;
    private float lifeRot;
    private ParticleSystem system;

	// Use this for initialization
	void Start () {
        system = GetComponent<ParticleSystem>();
        //startSpeed = system.startSpeed;
        startRot = system.startRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (left == true)
        {
            //system.startSpeed = -startSpeed;
            system.startRotation = -startRot;
        }
        else
        {
            //system.startSpeed = startSpeed;
            system.startRotation = startRot;
        }
	}
}
