using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Transform myTransform;
	private Rigidbody myRigidBody;

	public float playerSpeed = 3.0f;
	public float playerHealth = 10.0f;
	public float jumpHeight = 5.0f;
	public float distToGround = 0.5f; // this needs to be changed if object is a different height (do obj height/2)

	// Use this for initialization
	void Start () {
		myTransform = transform;
		myRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Move 
		myTransform.Translate (Vector3.right * playerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
	}

	// Similar to Update, but for logic using Physics
	void FixedUpdate () {
		// Jump
		if ( (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) || (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()) ) {
			Vector3 height = new Vector3(0, jumpHeight, 0);
			myRigidBody.velocity = height;
		}
	}

	bool IsGrounded() {
		return Physics.Raycast(myTransform.position, -Vector3.up, distToGround);
	}
}
