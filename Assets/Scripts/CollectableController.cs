using UnityEngine;
using System.Collections;

public class CollectableController : MonoBehaviour {

	private Transform myTransform;

	public bool isMagneted;
	public float myValue = 5.0f;

	public GameObject target;
	public float magnetSpeed = 3.0f;


	// Use this for initialization
	void Start () {
		myTransform = transform;
		isMagneted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMagneted) {
			// Move towards player 
			myTransform.position = Vector3.MoveTowards(myTransform.position, target.transform.position, magnetSpeed * Time.deltaTime);
		}
	}

}
