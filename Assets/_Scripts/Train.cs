using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {




	Rigidbody rb;
	Vector3 down = new Vector3 (0, -1, 0);
	Vector3 dir = new Vector3 (0, 0, 1);

	public void train(){
		RaycastHit hit;
		Physics.Raycast (rb.position, down, out hit, 5f); //9.5f distance recommended
			rb.transform.Translate(dir*.1f);

			if(!hit.collider.CompareTag("Rail")){
			dir = new Vector3(0,0,dir.z*-1f);
			rb.transform.Translate(dir);
			Debug.Log ("inverting..." + dir);
		}
		return ;
		
		
	}


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		train();
	}
}
