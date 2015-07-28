using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {


	public	Rigidbody rb;// = GetComponent<Rigidbody> ();

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()

	{
		if (rb.position.y < .25f) {
			rb.position = new Vector3(rb.position.x, .2f, rb.position.z);
			rb.velocity = new Vector3(0,0,0);
		}

	}
}
