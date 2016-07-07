using UnityEngine;
using System.Collections;

public class SpawnSmog : MonoBehaviour {
    public float spawnDelayLevel1;
    public float spawnDelayLevel2;
    public float spawnDelayLevel3;
    public float spawnDelayLevel4;
    public float spawnNumber;
    public Rect spawnArea;
    public GameObject smogPrefab;
    public float spawnTimer = -1;

    private bool hasThanked = true;

	// Use this for initialization
	void Start () {
        if (spawnTimer == -1)
        {
            spawnTimer = getSpawnDelay();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponentsInChildren<Smog>().Length == 0)
        {
            if (hasThanked == false)
            {
                GameObject.FindObjectOfType<SpeechBubble>().show(Country.Message.ThankYou);
                GameObject.FindObjectOfType<TutorialManager>().setSmogDone();
                hasThanked = true;

                spawnTimer = getSpawnDelay();
            }
            spawnTimer -= Time.deltaTime;
        }

        if (spawnTimer <= 0 & hasThanked == true)
        {
            hasThanked = false;

            for (int loop = 0; loop < spawnNumber; loop++)
            {
                GameObject newSmog = Object.Instantiate(smogPrefab);
                newSmog.transform.parent = this.transform;

                if (Random.Range(0, 2) == 0)
                {
                    newSmog.transform.localScale = new Vector3(-1, 1, 1);
                }

                //float spawnX = ((float)rand.NextDouble() * (spawnArea.xMax - spawnArea.xMin)) + spawnArea.xMin;
                //float spawnY = ((float)rand.NextDouble() * (spawnArea.yMax - spawnArea.yMin)) + spawnArea.yMin;
                Vector2 newPos = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax));
                //Vector2 newPos = new Vector2(spawnX, spawnY);
                newSmog.transform.localPosition = newPos;
            }
        }
	}

    /// <summary>
    /// Get the duration the game should wait after clearing all smog clouds before spawning a new batch.
    /// </summary>
    /// <returns>The wait duration, in seconds.</returns>
    private float getSpawnDelay()
    {
        float particulate = Country.getParticulate(Object.FindObjectOfType<GameController>().localRegion);

        if (particulate < 20)
        {
            return spawnDelayLevel1;
        }
        else if (particulate >= 20 & particulate < 25)
        {
            return spawnDelayLevel2;
        }
        else if (particulate >= 25 & particulate < 30)
        {
            return spawnDelayLevel3;
        }
        else
        {
            return spawnDelayLevel4;
        }
    }
}
