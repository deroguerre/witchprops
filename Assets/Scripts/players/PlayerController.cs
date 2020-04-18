using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

public class PlayerController : NetworkBehaviour
{
    public GameObject cameraPrefab;
    public float movePower = 1;
    public float maxSpeed = 2;
    public float jumpHeight = 1;
    public float lowJumpMultiplier = 2f;
    public float fallMultiplier = 2.5f;
    public GameObject[] allAvailableProps;

    /**
	 * Camera components
	 */
    private Camera _mainCamera;

    private GameObject _camTarget;
    private Rigidbody _camTargetRb;

    /*
     * Player components
     */
    private Transform _transform;
    private Rigidbody _rigidbody;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    private int _propsIterator = 0;

    private float _jumpCooldown = 0.5f;
    private float _nextJump;
    private float _distToGround;

    // Executed only on the local player
    public override void OnStartLocalPlayer()
    {
        // GetComponent<MeshRenderer>().material.color = Color.blue;

        // init camera components
        _camTarget = GameObject.FindGameObjectWithTag("CamTarget");
        _camTargetRb = _camTarget.GetComponent<Rigidbody>();
        GameObject camGO = Instantiate(cameraPrefab);
        camGO.GetComponent<MSCameraController>().target = _camTarget.transform;
        camGO.SetActive(true);
        _mainCamera = camGO.GetComponent<Camera>();

        // init player components
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();

        //Cursor.lockState = CursorLockMode.Locked;
        _distToGround = gameObject.GetComponent<Collider>().bounds.extents.y;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        // jump
        if (Input.GetButtonDown("Jump") && IsGrounded() && Time.time > _nextJump)
        {
            _nextJump = Time.time + _jumpCooldown;
            _rigidbody.AddForce(Vector3.up * (jumpHeight * 100));
        }

        // falling
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if (_rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            CmdChangeModel();
            _distToGround = gameObject.GetComponent<Collider>().bounds.extents.y;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        // get axis value from inputs
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        // move player to camera direction
        Transform camTransform = _mainCamera.transform;
        Vector3 camF = camTransform.forward;
        Vector3 camR = camTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 movement = (camF * input.y + camR * input.x) * (Time.deltaTime * movePower * 100);
        _rigidbody.AddForce(movement);

        // max speed movement
        if (_rigidbody.velocity.magnitude > maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
        }
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
            return;

        _camTargetRb.position = Vector3.Lerp(transform.position, _camTargetRb.position, Time.deltaTime * 50);
    }

    [Command]
    private void CmdChangeModel()
    {
        RpcChangeMesh();
    }

    [ClientRpc]
    private void RpcChangeMesh()
    {
        // prevent object to pass through terrain/objects
        gameObject.transform.position += new Vector3(0, 0.3f, 0);

        // apply props mesh to player mesh
        gameObject.GetComponent<MeshFilter>().sharedMesh =
            allAvailableProps[_propsIterator].GetComponent<MeshFilter>().sharedMesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh =
            allAvailableProps[_propsIterator].GetComponent<MeshFilter>().sharedMesh;

        if (_propsIterator == allAvailableProps.Length - 1)
        {
            _propsIterator = 0;
        }
        else
        {
            _propsIterator++;
        }
    }
}