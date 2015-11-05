using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemovePests : MonoBehaviour {
    public List<AudioClip> slapSounds;

    //private List<int> fingersPressing = new List<int>();

    private Collider2D col;
    private TouchManager tMan;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        tMan = Object.FindObjectOfType<TouchManager>();

        tMan.onTap += onTap;
	}
	
	// Update is called once per frame
	void Update () {
        /*List<int> newPresses = new List<int>();

        foreach (Touch t in Input.touches)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

            if (col == Physics2D.OverlapPoint(worldPos) & fingersPressing.Contains(t.fingerId) == false & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
            {
                newPresses.Add(t.fingerId);
            }
        }

        Pest[] pests = GetComponentsInChildren<Pest>();
        if (newPresses.Count > 0 & pests.Length > 0)
        {
            pests[Random.Range(0, pests.Length)].remove();
        }*/
	}

    /// <summary>
    /// Called when the player begins a tap touch event.
    /// </summary>
    /// <param name="screenPos">The position the player tapped the screen.</param>
    private void onTap(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (col == Physics2D.OverlapPoint(worldPos) & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            Pest[] pests = GetComponentsInChildren<Pest>();
            if (pests.Length > 0)
            {
                pests[Random.Range(0, pests.Length)].remove();

                AudioClip clip = slapSounds[Random.Range(0, slapSounds.Count)];
                Object.FindObjectOfType<MusicController>().playSoundEffect(clip);
            }
        }
    }
}
