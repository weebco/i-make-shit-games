using UnityEngine;
using System.Collections;

public class followingMistScript : MonoBehaviour {

	public GameObject player;
	public Rigidbody playerRb;
	public Rigidbody self;

	// Use this for initialization
	void Start () {
		self = GetComponent<Rigidbody> ();
		 player = GameObject.Find ("Player");
		playerRb = player.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 moveTo = playerRb.position;
		self.MovePosition(new Vector3(moveTo.x, 0, moveTo.z));
	}
}
