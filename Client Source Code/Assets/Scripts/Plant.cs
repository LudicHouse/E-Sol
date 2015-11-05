using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Plant : MonoBehaviour {
    public Stem centerStem;
    public List<Stem> leftStems;
    public List<Stem> rightStems;
    public GameObject newStemPrefab;
    public int regionsUntilFirstNewStem;
    public int regionsPerNewStem;
    public int maxSideStems;
    public float plantHydration;
    public float hydroDrainRate;
    public SpawnPests flowerPot;
    public GameObject smogContainer;
    public float insectsMultiplier;
    public float waterMultiplier;
    public float pollutionMultiplier;
    public int animalsPerMultiplier;
    public float growthPerMoodPerSecond;
    public GameObject animalPanel;
    public Animator headAnimator;
    public float minBlinkDelay;
    public float maxBlinkDelay;

    private int lastRegion = 0;
    private int lastStemRegion = 0;
    private float blinkTimer;
    private float mood;

	// Use this for initialization
	void Start () {
        blinkTimer = Random.Range(minBlinkDelay, maxBlinkDelay);
	}
	
	// Update is called once per frame
    void Update()
    {
        mood = plantHydration * waterMultiplier;
        if (flowerPot.numPests() == 0)
        {
            mood += insectsMultiplier;
            headAnimator.SetBool("Pests", false);
        }
        else
        {
            headAnimator.SetBool("Pests", true);
        }
        if (isPollution() == false)
        {
            mood += pollutionMultiplier;
            headAnimator.SetBool("Smog", false);
        }
        else
        {
            headAnimator.SetBool("Smog", true);
        }

        int numAnimals = getNumAnimals();

        float growth = mood * (Mathf.Floor((float)numAnimals / (float)animalsPerMultiplier) + 1);
        //growth = 40;
        //Debug.Log(mood + " " + numAnimals + " " + growth);
        //Debug.Log(mood);

        headAnimator.SetInteger("Hydration", (int)(plantHydration * 100));
        headAnimator.SetInteger("Mood", (int)(mood * 100));

        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0)
        {
            headAnimator.SetTrigger("Blink");
            blinkTimer = Random.Range(minBlinkDelay, maxBlinkDelay);
        }

        Vector2 newPosition = transform.position;
        newPosition.y += growth * growthPerMoodPerSecond * Time.deltaTime;
        transform.position = newPosition;

        while (transform.position.y > lastRegion + 1)
        {
            lastRegion++;

            centerStem.addStem();
            foreach (Stem s in leftStems)
            {
                s.addStem();
            }
            foreach (Stem s in rightStems)
            {
                s.addStem();
            }

            if (numSideStems() == 0)
            {
                if (lastRegion >= regionsUntilFirstNewStem)
                {
                    newStem();
                }
            }
            else if (lastRegion >= lastStemRegion + regionsPerNewStem & numSideStems() < maxSideStems)
            {
                newStem();
            }
        }

        plantHydration = Mathf.Max(0, plantHydration - hydroDrainRate * Time.deltaTime);
    }

    /// <summary>
    /// Adds a new stem to the plant.
    /// </summary>
    private void newStem()
    {
        bool rightSide = true;
        if (rightStems.Count > leftStems.Count)
        {
            rightSide = false;
        }

        GameObject newStem = Object.Instantiate(newStemPrefab);
        newStem.transform.parent = transform;
        if (rightSide == false)
        {
            newStem.transform.localScale = new Vector3(-1, 1, 1);
        }

        Vector2 pos = new Vector2();
        if (rightSide == true)
        {
            pos.x = 0.25f * (rightStems.Count + 1);
        }
        else
        {
            pos.x = -0.25f * (leftStems.Count + 1);
        }
        pos.y = -0.5f - lastRegion;

        newStem.transform.localPosition = pos;
        if (rightSide == true)
        {
            rightStems.Add(newStem.GetComponent<Stem>());
        }
        else
        {
            leftStems.Add(newStem.GetComponent<Stem>());
        }

        lastStemRegion = lastRegion;
    }

    /// <summary>
    /// Get the total number of plant stems.
    /// </summary>
    /// <returns>The number of stems.</returns>
    private int numSideStems()
    {
        return rightStems.Count + leftStems.Count;
    }

    /// <summary>
    /// Get the number of stems on the left side of the plant.
    /// </summary>
    /// <returns>The number of stems.</returns>
    public int numLeftStems()
    {
        return leftStems.Count;
    }

    /// <summary>
    /// Get the number of stems on the left side of the plant.
    /// </summary>
    /// <param name="region">The region at which to check.</param>
    /// <returns>The number of stems.</returns>
    public int numLeftStems(int region)
    {
        int total = 0;

        foreach (Stem s in leftStems)
        {
            if (transform.localPosition.y + s.transform.localPosition.y > region + 1)
            {
                total++;
            }
        }

        return total;
    }

    /// <summary>
    /// Get the number of stems on the right side of the plant.
    /// </summary>
    /// <returns>The number of stems.</returns>
    public int numRightStems()
    {
        return rightStems.Count;
    }

    /// <summary>
    /// Get the number of stems on the right side of the plant.
    /// </summary>
    /// <param name="region">The region at which to check.</param>
    /// <returns>The number of stems.</returns>
    public int numRightStems(int region)
    {
        int total = 0;

        foreach (Stem s in rightStems)
        {
            if (transform.localPosition.y + s.transform.localPosition.y > region + 1)
            {
                total++;
            }
        }

        return total;
    }

    /// <summary>
    /// Get the current mood of the plant.
    /// </summary>
    /// <returns>The mood value.</returns>
    public float getMood()
    {
        return mood;
    }

    /// <summary>
    /// Check whether there is any pollution at present.
    /// </summary>
    /// <returns>True if smog clouds exist, false otherwise.</returns>
    public bool isPollution()
    {
        if (smogContainer.transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Get the number of unlocked animals.
    /// </summary>
    /// <returns>The number of animals.</returns>
    public int getNumAnimals()
    {
        int total = 0;

        foreach (Image i in animalPanel.GetComponentsInChildren<Image>(true))
        {
            if (i.tag == "AnimalOption")
            {
                if (i.GetComponent<Toggle>().interactable == true)
                {
                    total++;
                }
            }
        }

        return total;
    }
}
