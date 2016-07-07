using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smog : MonoBehaviour {
    public List<Sprite> thickSprites;
    public List<Sprite> thinSprites; //Redundant?
    public float maxDistanceX;
    public float minMoveSpeed;
    public float maxMoveSpeed;
    public float minHealth;
    public float maxHealth;
    public float minTransparency;
    public bool damageAll;
    public AudioClip disperseSound;

    private bool moveRight;
    private float speed;
    private int spriteId;
    private float totalHealth;
    private float health;
    //private int fingerId = -1;
    private bool dragging = false;
    private bool hasDoneSound = false;

    private SpriteRenderer rend;
    private Rigidbody2D rb;
    private Collider2D col;
    private TouchManager tMan;

	// Use this for initialization
	void Start () {
        if (Random.Range(0, 2) == 0)
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }
        speed = Random.Range(minMoveSpeed, maxMoveSpeed);
        //speed = ((float)rand.NextDouble() * (maxMoveSpeed - minMoveSpeed)) + minMoveSpeed;
        spriteId = Random.Range(0, thickSprites.Count);

        rend = GetComponent<SpriteRenderer>();
        rend.sprite = thickSprites[spriteId];

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        tMan = Object.FindObjectOfType<TouchManager>();
        tMan.onDragStart += onDragStart;
        tMan.onDrag += onDrag;
        tMan.onDragStop += onDragStop;

        totalHealth = Random.Range(minHealth, maxHealth);
        health = totalHealth;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (fingerId == -1)
        {
            foreach (Touch t in Input.touches)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

                if (col == Physics2D.OverlapPoint(worldPos) & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
                {
                    fingerId = t.fingerId;
                    break;
                }
            }
        }
        else
        {
            if (Input.touchCount < fingerId + 1)
            {
                fingerId = -1;
            }
            else if (Input.GetTouch(fingerId).phase == TouchPhase.Canceled | Input.GetTouch(fingerId).phase == TouchPhase.Ended)
            {
                fingerId = -1;
            }
            else
            {
                Touch t = Input.GetTouch(fingerId);
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

                if (col == Physics2D.OverlapPoint(worldPos) & t.phase == TouchPhase.Moved)
                {
                    float damage = t.deltaPosition.magnitude;
                    health -= damage;
                }
            }
        }*/

        float transparency = minTransparency + ((1 - minTransparency) * (health / totalHealth));
        Color newColor = rend.color;
        newColor.a = transparency;
        rend.color = newColor;

        if (health <= 0)
        {
            if (damageAll == true)
            {
                bool allAtZero = true;
                foreach (Transform cloud in transform.parent)
                {
                    if (cloud.GetComponent<Smog>().health > 0)
                    {
                        allAtZero = false;
                        break;
                    }
                }

                if (allAtZero == true)
                {
                    dragging = false;
                    tMan.onDragStart -= onDragStart;
                    tMan.onDrag -= onDrag;
                    tMan.onDragStop -= onDragStop;
                    Object.Destroy(this.gameObject);
                }
            }
            else
            {
                dragging = false;
                tMan.onDragStart -= onDragStart;
                tMan.onDrag -= onDrag;
                tMan.onDragStop -= onDragStop;
                Object.Destroy(this.gameObject);
            }

            if (hasDoneSound == false)
            {
                Object.FindObjectOfType<MusicController>().playSoundEffect(disperseSound);
                hasDoneSound = true;
            }
        }
	}

    void FixedUpdate()
    {
        if (transform.localPosition.x < -maxDistanceX)
        {
            moveRight = true;
        }
        else if (transform.localPosition.x > maxDistanceX)
        {
            moveRight = false;
        }

        if (moveRight == true)
        {
            rb.AddForce(new Vector2(speed * Time.deltaTime, 0));
        }
        else
        {
            rb.AddForce(new Vector2(-speed * Time.deltaTime, 0));
        }
    }

    /// <summary>
    /// Called when the player begins a drag touch event.
    /// </summary>
    /// <param name="startPos">The starting position of the drag.</param>
    private void onDragStart(Vector2 startPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(startPos);

        if (col == Physics2D.OverlapPoint(worldPos) & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            dragging = true;
        }
    }

    /// <summary>
    /// Called as part of a drag touch event.
    /// </summary>
    /// <param name="delta">The amount the finger has moved since the last event.</param>
    private void onDrag(Vector2 delta)
    {
        if (dragging == true)
        {
            Vector2 worldDelta = Camera.main.ScreenToWorldPoint(delta) - Camera.main.ScreenToWorldPoint(Vector2.zero);
            float damage = worldDelta.magnitude; //Note: This might result in varying damage times for different DPI screens.
            health -= damage;

            if (damageAll == true)
            {
                foreach (Transform cloud in transform.parent)
                {
                    if (cloud != this.transform)
                    {
                        cloud.GetComponent<Smog>().health -= damage;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when the player finished a drag touch event.
    /// </summary>
    private void onDragStop()
    {
        dragging = false;
    }
}
