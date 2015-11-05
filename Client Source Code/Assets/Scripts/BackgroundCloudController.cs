using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCloudController : MonoBehaviour {
    public List<GameObject> cloudPrefabs;
    public float spawnX;
    public float spawnYMin;
    public float spawnYMax;
    public float scrollYMin;
    public float scrollYMax;
    public float speed;
    public float spawnDelayMin;
    public float spawnDelayMax;
    public Color topColor;
    public Color bottomColor;
    public Color pollutionColor;
    public float fadeYMin;
    public float fadeYMax;

    private float spawnTimer;

    private Plant p;

	// Use this for initialization
	void Start () {
        spawnTimer = Random.Range(spawnDelayMin, spawnDelayMax);

        p = Object.FindObjectOfType<Plant>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        foreach (Transform child in transform)
        {
            Vector2 newPos = child.localPosition;
            newPos.x += speed * Time.deltaTime;

            if (speed > 0 & newPos.x > -spawnX)
            {
                Object.Destroy(child.gameObject);
            }
            else if (speed < 0 & newPos.x <  -spawnX)
            {
                Object.Destroy(child.gameObject);
            }

            if (newPos.y < scrollYMin)
            {
                while (newPos.y < scrollYMin)
                {
                    newPos.y = scrollYMax - (scrollYMin - newPos.y);
                }

                //newPos.y = scrollYMax; //Note: Might cause issues if multiple clouds pass the border in the same frame but different heigts.
            }
            else if (newPos.y > scrollYMax)
            {
                while (newPos.y > scrollYMax)
                {
                    newPos.y = scrollYMin + (newPos.y - scrollYMax);
                }

                //newPos.y = scrollYMin;
            }

            child.localPosition = newPos;

            SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
            if (p.isPollution() == true)
            {
                rend.color = pollutionColor;
            }
            else if (newPos.y > fadeYMax)
            {
                rend.color = topColor;
            }
            else if (newPos.y < fadeYMin)
            {
                rend.color = bottomColor;
            }
            else
            {
                float ratio = (newPos.y - fadeYMin) / (fadeYMax - fadeYMin);
                Color newColor = Color.Lerp(bottomColor, topColor, ratio);
                rend.color = newColor;
            }
        }

        if (spawnTimer <= 0)
        {
            int index = Random.Range(0, cloudPrefabs.Count);
            GameObject newCloud = Object.Instantiate(cloudPrefabs[index]);
            newCloud.transform.parent = this.transform;

            Vector2 newPos = new Vector2();
            newPos.x = spawnX;
            newPos.y = Random.Range(spawnYMin, spawnYMax);
            newCloud.transform.localPosition = newPos;

            spawnTimer = Random.Range(spawnDelayMin, spawnDelayMax);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
	}
}
