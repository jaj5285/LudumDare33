using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Transform myTransform;
	private Rigidbody myRigidBody;
	private Renderer renderer;
	private bool isDoubleJump;

	// Player Stats
	public float playerSpeed = 3.0f;
	public float playerHealth = 10.0f;
	public float jumpHeight = 15.0f;
	public float doubleJumpHeight = 16.0f;

	// Player PoweUps
	public bool hasFirePower = true;
	public bool hasDoubleJumpPower = true;
	public bool hasMagnetPower = true;
	public float playerMoney = 0.0f;

	// Helper variables
	public float downwardForce = -1f; // gravity so jumps are not float-y
	public float myHeight; // used to calculate distance to ground for raycast isGrounded

	// External Objects
	public GameObject flamethrowerObj;
	public GameObject magnetObj;
	public Text healthText;
	public Text collectableText;

	// Buttons
	public KeyCode FIRE_BTN = KeyCode.V;



	// Use this for initialization
	void Start () {
		myTransform = transform;
		myRigidBody = GetComponent<Rigidbody> ();
		renderer = this.gameObject.GetComponent<Renderer> ();
		isDoubleJump = false;
		
		flamethrowerObj.SetActive(false);
		magnetObj.SetActive (false);

		myHeight = renderer.bounds.size.y;
		healthText.text = "Health: " + playerHealth;
	}
	
	// Update is called once per frame
	void Update () {
		// Move 
		myTransform.Translate (Vector3.right * playerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);

		// Jump
		if ( Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) ) {
			// Regular jump
			if ( IsGrounded() ) {	
				Jump(jumpHeight);
			} 
			else // Double jump
			{
				if (!isDoubleJump && hasDoubleJumpPower) 
				{
					isDoubleJump = true;
					Jump(doubleJumpHeight);
				}
			}
		}

		if (IsGrounded ()) {
			isDoubleJump = false;
		} 
		else {
			// Downward gravity
			Vector3 gravity = new Vector3 (0, downwardForce, 0);
			myRigidBody.velocity += gravity;
		}

		// Fire Attack
		if (Input.GetKey (FIRE_BTN) && hasFirePower) {
			flamethrowerObj.SetActive(true);
		} else if (Input.GetKeyUp (FIRE_BTN)) {
			flamethrowerObj.SetActive(false);
		}

		// Magnet
		if (hasMagnetPower && !magnetObj.activeSelf) {
			magnetObj.SetActive(true);
		}
	}

	void Jump (float height) {		
		Vector3 myJump = new Vector3(0, height, 0);
		myRigidBody.velocity = myJump;
	}

	bool IsGrounded() {
		return Physics.Raycast(myTransform.position, -Vector3.up, (myHeight/2 + 0.05f));
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
			EnemyController enemyObj = col.gameObject.GetComponent<EnemyController> ();
			
			// damage player
			playerHealth -= enemyObj.damageGiven;
			
			// Update text box
			healthText.text = "Health: " + playerHealth;
			
			// Make player flash red
			StartCoroutine (ColorFlash (5.0f, 0.3f, Color.red));
			
			// Calculate if player is dead
			if (playerHealth <= 0) {
				Destroy (this.gameObject);
			}
		}
		//Collectables
		if (col.gameObject.CompareTag ("Collectable")) {
			CollectableController CC = col.gameObject.GetComponent<CollectableController>();

			// Add money
			playerMoney += CC.myValue;

			// Update text box
			collectableText.text = "$ " + playerMoney;

			// Destroy collectable
			Destroy(col.gameObject);
		}
	}
	
	
}
