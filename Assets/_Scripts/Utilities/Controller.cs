using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public float speed;
	private Vector3 jumpHeight = new Vector3(0,500,0);
	private Rigidbody rb;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		speed = 11;
		rb.AddForce (movement * speed);
	}

	public void ApplyForce(Vector3 force)
	{
		rb.AddForce (force);
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag ("Jump"))
		{
			rb.AddForce(jumpHeight);
		}
	}


}