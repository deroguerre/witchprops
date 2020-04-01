using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public float movePower = 1;
	public float maxSpeed = 2;
	public float jumpHeight = 1;
	public float lowJumpMultiplier = 2f;
	public float fallMultiplier = 2.5f;
	public bool isGrounded;

	private Camera _mainCamera;
	private Vector3 _maxVelocity;
	private Rigidbody _rigidbody;

	private GameObject _camTarget;
	private Rigidbody _camTargetRb;

	// Executed only on the local player
	public override void OnStartLocalPlayer() {

		//base.OnStartLocalPlayer();

		GetComponent<MeshRenderer>().material.color = Color.blue;

		_mainCamera = Camera.main;
		_rigidbody = GetComponent<Rigidbody>();
		_maxVelocity = new Vector3(maxSpeed, maxSpeed, maxSpeed);

		_camTarget = GameObject.FindGameObjectWithTag("CamTarget");
		_camTargetRb = _camTarget.GetComponent<Rigidbody>();
	}

	private void Update() {

		if (!isLocalPlayer) {
			return;
		}

		//Jump
		if (Input.GetButtonDown("Jump")) {
			_rigidbody.AddForce(Vector3.up * jumpHeight * 100);
		}

		//Falling
		if (_rigidbody.velocity.y < 0) {
			_rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (_rigidbody.velocity.y > 0 && !Input.GetButton("Jump")) {
			_rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	private void FixedUpdate() {

		if (!isLocalPlayer) {
			return;
		}

		//get axis value from inputs
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		input = Vector2.ClampMagnitude(input, 1);

		//move player to camera direction
		if (_mainCamera != null) {

			Vector3 camF = _mainCamera.transform.forward;
			Vector3 camR = _mainCamera.transform.right;

			camF.y = 0;
			camR.y = 0;
			camF = camF.normalized;
			camR = camR.normalized;

			Vector3 movement = (camF * input.y + camR * input.x) * Time.deltaTime * movePower * 100;
			_rigidbody.AddForce(movement);

			//Max speed movement
			if (_rigidbody.velocity.magnitude > maxSpeed) {
				_rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
			}
		}
	}

	void LateUpdate() {

		if (!isLocalPlayer) {
			return;
		}

		_camTargetRb.position = Vector3.Lerp(transform.position, _camTargetRb.position, Time.deltaTime * 50);
	}

}
