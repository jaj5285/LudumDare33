using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public Transform myTransform;
	public float damageGiven = 5.0f;

	// Use this for initialization
	void Start () {
		myTransform = transform;			
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
		// Dies on player fire attack
		if (col.gameObject.CompareTag("Attack_Fire")) {
			Destroy(this.gameObject);
		}
	}
}
