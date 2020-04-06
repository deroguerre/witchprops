using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;

#pragma warning disable 618

public class HunterController : NetworkBehaviour
{
    public float moveSpeed = 4;
    public float maxYPushForce = 12.0f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

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

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        // init camera components
        _mainCamera = Camera.main;
        _camTarget = GameObject.FindGameObjectWithTag("CamTarget");
        _camTargetRb = _camTarget.GetComponent<Rigidbody>();

        // init player components
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    public int time = 0;

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

        // player movements
        Vector3 movement = (camF * input.y + camR * input.x) * (Time.fixedDeltaTime * moveSpeed);
        transform.position += movement;
        
        // player floating above ground and Y position adjustment
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);
        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit))
        {
            // float minDistance = 1.0f;
            // float maxDistance = 2f;
            // float maxForce = 25.0f;
            //
            // float fractionalPosition = (maxDistance - hit.distance) / (maxDistance - minDistance);
            // float force = fractionalPosition * maxForce;
            // _rigidbody.AddForce( Vector3.up * force);

            if (hit.distance < 1f)
            {
                if (hit.distance < 0.8f)
                {
                    _rigidbody.AddForce(Vector3.up * maxYPushForce / hit.distance);
                    return;
                }

                _rigidbody.velocity = _rigidbody.velocity * 0.95f * Time.fixedDeltaTime;
            }
            else if (hit.distance > 1f)
            {
                _rigidbody.AddForce(Vector3.down * maxYPushForce / hit.distance);
            }
        }

        // if(!Input.anyKey)
        // {
        //     time++;
        // } else {
        //     time = 0;
        // }
        // if(time >= 100) {
        //     Debug.Log("100 frames passed with no input");
        //     _rigidbody.velocity = _rigidbody.velocity * 0.95f * Time.fixedDeltaTime;
        // }
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }

        // player rotation to look at center of screen
        Vector3 mouseWorldPosition =
            _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.LookAt(mouseWorldPosition);
        transform.rotation =
            Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
            return;

        _camTargetRb.position = Vector3.Lerp(transform.position, _camTargetRb.position, Time.deltaTime * 50);
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = (GameObject) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * -12.0f;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2);
    }
}