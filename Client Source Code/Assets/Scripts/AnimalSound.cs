using UnityEngine;
using System.Collections;

public class AnimalSound : MonoBehaviour {
    public AudioClip sound;
    public float minWaitTime;
    public float maxWaitTime;

    private float timer;

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();

        timer = Random.Range(minWaitTime, maxWaitTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            source.PlayOneShot(sound);
            timer = Random.Range(minWaitTime, maxWaitTime);
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
