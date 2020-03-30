using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {


	private GameObject target;

	private Rigidbody _rigidbody;
	private float yPosition;

	// Start is called before the first frame update
	void Start() {

	}

	// Executed only on the local player
	public override void OnStartLocalPlayer() {
		target = GameObject.FindGameObjectWithTag("CamTarget");
		_rigidbody = GetComponent<Rigidbody>();
	}

	void LateUpdate() {

		if (!isLocalPlayer) {
			return;
		}

		Rigidbody targetRb = target.GetComponent<Rigidbody>();
		targetRb.position = Vector3.Lerp(transform.position, targetRb.position, Time.deltaTime * 50);
	}



	void OnGUI() {
		//GUI.Label(new Rect(10, 10, 200, 20), "score: ");
	}
}
