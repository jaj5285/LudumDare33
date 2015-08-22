using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Transform myTransform;
	private Rigidbody myRigidBody;
	private Renderer renderer;
	
	public float playerSpeed = 3.0f;
	public float playerHealth = 10.0f;
	public float jumpHeight = 5.0f;
	public float distToGround = 0.5f; // this needs to be changed if object is a different height (do obj height/2)

	public Text healthText;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		myRigidBody = GetComponent<Rigidbody> ();
		renderer = this.gameObject.GetComponent<Renderer> ();
		
		healthText.text = "Health: " + playerHealth;
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
		
	IEnumerator ColorFlash(float time, float intervalTime, Color flashColor)
	{
		Color[] colors = {flashColor, Color.white};
		float elapsedTime = 0f;
		int index = 0;
		while(elapsedTime < time )
		{
			renderer.material.color = colors[index % 2];
			elapsedTime += intervalTime;
			index++;
			yield return new WaitForSeconds(intervalTime);
		}	
		renderer.material.color = Color.white;
	}

	void OnCollisionEnter (Collision col) {
		//collide with Enemy
		if (col.gameObject.CompareTag ("Enemy")) {
			EnemyController enemyObj = col.gameObject.GetComponent<EnemyController>();
			
			// damage player
			playerHealth -= enemyObj.damageGiven;
			
			// Update text box
			healthText.text = "Health: " + playerHealth;
			
			// Make player flash red
			StartCoroutine (ColorFlash (5.0f, 0.3f, Color.red));
			
			// Calculate if player is dead
			if (playerHealth <= 0) {
				Destroy(this.gameObject);
			}
		}
	}
	
	
}
