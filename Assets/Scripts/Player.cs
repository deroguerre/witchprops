using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	private GameObject _target;
	private Rigidbody _targetRigibody;
	private Rigidbody _rigidbody;

	// Executed only on the local player
	public override void OnStartLocalPlayer() {
		_target = GameObject.FindGameObjectWithTag("CamTarget");
		_rigidbody = GetComponent<Rigidbody>();
		_targetRigibody = _target.GetComponent<Rigidbody>();
	}

	void LateUpdate() {

		if (!isLocalPlayer) {
			return;
		}

		_targetRigibody.position = Vector3.Lerp(transform.position, _targetRigibody.position, Time.deltaTime * 50);
	}

	void OnGUI() {
		//GUI.Label(new Rect(10, 10, 200, 20), "score: ");
	}
}
