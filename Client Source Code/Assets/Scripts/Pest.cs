using UnityEngine;
using System.Collections;

public class Pest : MonoBehaviour {
    public float moveSpeed;
    public int changeDirectionOdds;
    public Vector2 jumpSpeed;
    //public Sprite deadSprite;

    private SpawnPests flowerPot;
    private int direction = 1;
    private Rigidbody2D rb;
    private bool isDead = false;

    private Animator anim;

	// Use this for initialization
	void Start () {
        flowerPot = GetComponentInParent<SpawnPests>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        if (isDead == false)
        {
            if (Random.Range(0, changeDirectionOdds) == 0)
            {
                direction = Random.Range(-1, 2);

                if (direction > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (direction < 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            //Debug.Log(direction);

            Vector3 newPosition = transform.localPosition;
            newPosition.x = Mathf.Clamp(transform.position.x + (direction * moveSpeed * Time.deltaTime), flowerPot.pestAreaXMin, flowerPot.pestAreaXMax);
            transform.localPosition = newPosition;

            if (transform.position.x + (direction * moveSpeed * Time.deltaTime) > flowerPot.pestAreaXMax | transform.position.x + (direction * moveSpeed * Time.deltaTime) < flowerPot.pestAreaXMin)
            {
                direction *= -1;

                if (direction > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (direction < 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            if (direction != 0)
            {
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }
        }
        else
        {
            if (transform.position.y <= -5)
            {
                Object.Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Removes the pest from the flower pot and triggers the death effect.
    /// </summary>
    public void remove()
    {
        isDead = true;
        Vector2 jump = new Vector2(jumpSpeed.x, jumpSpeed.y);
        if (Random.Range(0, 2) == 0)
        {
            jump.x = -jump.x;
        }

        transform.parent = null;
        rb.isKinematic = false;
        rb.velocity = jump;

        //SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        //renderer.sprite = deadSprite;
        if (rb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetTrigger("die");
    }
}
