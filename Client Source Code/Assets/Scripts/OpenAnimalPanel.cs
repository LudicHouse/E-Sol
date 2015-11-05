using UnityEngine;
using System.Collections;

public class OpenAnimalPanel : MonoBehaviour {
    public Branch branch;

    private Collider2D col;
    private TouchManager tMan;
    private GameController cont;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        tMan = Object.FindObjectOfType<TouchManager>();
        cont = Object.FindObjectOfType<GameController>();

        tMan.onTap += onTap;
	}
	
	// Update is called once per frame
	void Update () {
        /*foreach (Touch t in Input.touches)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

            if (col == Physics2D.OverlapPoint(worldPos) & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
            {
                GameObject.FindObjectOfType<GameController>().openAnimalPanel(animalRenderer);
            }
        }*/
	}

    /// <summary>
    /// Called when the player begins a tap touch event.
    /// </summary>
    /// <param name="screenPos">The position the player tapped the screen.</param>
    private void onTap(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (cont.isPanelOpen() == false && (worldPos.x > col.bounds.min.x && worldPos.x < col.bounds.max.x && worldPos.y > col.bounds.min.y && worldPos.y < col.bounds.max.y))
        {
            cont.openAnimalPanel(branch);
        }

        /*if (col == Physics2D.OverlapPoint(worldPos) & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            GameObject.FindObjectOfType<GameController>().openAnimalPanel(branch);
        }*/
    }
}
