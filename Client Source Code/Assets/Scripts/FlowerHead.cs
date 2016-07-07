using UnityEngine;
using System.Collections;

public class FlowerHead : MonoBehaviour {
    public float maxDragForce;
    public float maxDragDist;
    public Transform headAnchor;
    public Transform face;
    public float maxFaceDist;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        if (Input.touchCount > 0)
        {
            Vector2 mousePos = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            Vector2 mouseOffset = mousePos - (Vector2)headAnchor.localPosition;

            Vector2 direction = mouseOffset.normalized;
            float dist = mouseOffset.magnitude;

            if (dist > maxDragDist)
            {
                rb.AddForce(direction * maxDragForce * Time.deltaTime);
            }
            else
            {
                float force = (dist / maxDragDist) * maxDragForce;
                rb.AddForce(direction * force * Time.deltaTime);
            }

            //rb.AddForce(mouseOffset * forceMultiplier * Time.deltaTime);
        }

        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
        Vector2 offset = transform.localPosition - headAnchor.localPosition;
        face.localPosition = (offset / joint.distance) * maxFaceDist;
	}
}
