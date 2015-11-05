using UnityEngine;
using System.Collections;

public class SpawnPests : MonoBehaviour {
    public float respawnDuration;
    public float respawnCounter = -1;

    public int numSpawnMin;
    public int numSpawnMax;

    public float pestAreaY;
    public float pestAreaXMin;
    public float pestAreaXMax;

    public GameObject pestPrefab;

    private bool hasThanked = true;
    private AnimalSound sounds;

	// Use this for initialization
	void Start () {
        if (respawnCounter == -1)
        {
            respawnCounter = respawnDuration;
        }
        sounds = GetComponent<AnimalSound>();
	}
	
	// Update is called once per frame
	void Update () {
        if (numPests() == 0)
        {
            if (hasThanked == false)
            {
                GameObject.FindObjectOfType<SpeechBubble>().show(Country.Message.ThankYou);
                GameObject.FindObjectOfType<TutorialManager>().setPestDone();
                hasThanked = true;

                respawnCounter = respawnDuration;
            }
            respawnCounter -= Time.deltaTime;

            sounds.enabled = false;
        }
        else
        {
            sounds.enabled = true;
        }

        if (respawnCounter <= 0 & hasThanked == true)
        {
            hasThanked = false;

            int numToSpawn = Random.Range(numSpawnMin, numSpawnMax + 1);
            for (int loop = 0; loop < numToSpawn; loop++)
            {
                //float spawnX = ((float)rand.NextDouble() * (pestAreaXMax - pestAreaXMin)) + pestAreaXMin;
                Vector2 spawnPos = new Vector2(Random.Range(pestAreaXMin, pestAreaXMax), pestAreaY);
                //Vector2 spawnPos = new Vector2(spawnX, pestAreaY);
                GameObject newPest = (GameObject)Object.Instantiate(pestPrefab, transform.TransformPoint(spawnPos), new Quaternion());

                newPest.transform.parent = transform;
            }
        }
	}

    /// <summary>
    /// Get the number of pests currently in the flower pot.
    /// </summary>
    /// <returns>The number of pests.</returns>
    public int numPests()
    {
        return GetComponentsInChildren<Pest>().Length;
    }

    /// <summary>
    /// Set the respawn timer.
    /// </summary>
    /// <param name="newVal">The value to set the timer to.</param>
    public void setTimer(float newVal)
    {
        respawnCounter = newVal;
    }
}
