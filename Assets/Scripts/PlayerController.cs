using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Camera mainCamera;
	public float movePower = 1;
	public float maxSpeed = 2;
	public float jumpHeight = 1;
	public float lowJumpMultiplier = 2f;
	public float fallMultiplier = 2.5f;
	public bool isGrounded;

	private Vector3 maxVelocity;

	private Rigidbody _rigidbody;
	private Transform camTransform;
	private Transform camTarget;

	void Start() {
		_rigidbody = GetComponent<Rigidbody>();
		camTarget = gameObject.transform.Find("CamTarget");
		maxVelocity = new Vector3(maxSpeed, maxSpeed, maxSpeed);
	}

	void Update() {

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

		//Debug.Log(Input.GetAxis("Dash"));

		//if (Input.GetButton("LB")) {
		//	Debug.Log("LB pressed");

		//	if (gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.minDistance > 0.1f) {
		//		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.minDistance -= 0.5f;
		//		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.maxDistance -= 0.5f;
		//	}
		//	//cameras[index]._camera.fieldOfView -= _scrollInputMSACC * CameraSettings.firstPerson.speedScroolZoom * 50.0f;
		//}

		//if (Input.GetButton("RB")) {
		//	Debug.Log("RB pressed");
		//	if (gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.minDistance < 1000) {
		//		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.minDistance += 0.5f;
		//		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.maxDistance += 0.5f;
		//	}
		//}

		//if (Input.GetAxis("Dash") < -0.1f) {
		//	//Debug.Log("Left Dash");
		//}

		//if (Input.GetAxis("Dash") > 0.1f) {
		//	//Debug.Log("Right Dash");
		//	moveSpeed = 100f;
		//	//thisRigibody.AddForce(Vector3.up * jumpHeight * 100);
		//} else {
		//	moveSpeed = 30f;
		//}

	}

	private void FixedUpdate() {
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		input = Vector2.ClampMagnitude(input, 1);

		//move player to camera direction
		if (mainCamera != null) {

			Vector3 camF = mainCamera.transform.forward;
			Vector3 camR = mainCamera.transform.right;

			camF.y = 0;
			camR.y = 0;
			camF = camF.normalized;
			camR = camR.normalized;

			Vector3 movement = (camF * input.y + camR * input.x) * Time.deltaTime * movePower * 100;
			_rigidbody.AddForce(movement);

			//Max speed movement
			if(_rigidbody.velocity.magnitude > maxSpeed) {
				_rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
			}
		}

		//if (camTarget != null) {
		//	camTarget.transform.position = gameObject.transform.position;
		//}
	}

	//private void OnCollisionEnter(Collision other) {

	//	if (other.gameObject.tag == "Ground") {
	//		isGrounded = true;
	//	}
	//}

	//private void OnCollisionExit(Collision other) {
	//	if (other.gameObject.tag == "Ground") {
	//		isGrounded = false;
	//	}
	//}

}
