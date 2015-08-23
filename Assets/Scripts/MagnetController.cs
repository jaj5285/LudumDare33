using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {

	public bool isOn = true;

	void OnTriggerEnter (Collider col) {
		if (isOn){
			if ( col.gameObject.CompareTag("Collectable") ) {
				col.gameObject.GetComponent<CollectableController>().isMagneted = true;
			}
		}
	}
}
