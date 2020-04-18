using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;

#pragma warning disable 618

public class HunterController : NetworkBehaviour
{
    public GameObject hunterCamera;

    public float moveSpeed = 4;
    public float maxYPushForce = 12.0f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    /*
    * Player components
    */
    private Transform _transform;
    private Rigidbody _rigidbody;
    
    //JUMP
    public float jumpHeight = 1;
    public float lowJumpMultiplier = 2f;
    public float fallMultiplier = 2.5f;
    private float _jumpCooldown = 0.5f;
    private float _nextJump;
    private float _distToGround;
    private bool isJumping = false;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        // init player components
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //player movements
        Vector3 move = _transform.right * x + _transform.forward * z;
        Vector3 movement = move * (moveSpeed * Time.deltaTime);
        _transform.position += movement;

        // player floating above ground and Y position adjustment
        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out RaycastHit hit) && isJumping == false)
        {
            if (hit.distance < 1f)
            {
                if (hit.distance < 0.8f)
                {
                    _rigidbody.AddForce(Vector3.up * maxYPushForce / hit.distance);
                    return;
                }

                _rigidbody.velocity = _rigidbody.velocity * (0.95f * Time.fixedDeltaTime);
            }
            else if (hit.distance > 1f)
            {
                _rigidbody.AddForce(Vector3.down * maxYPushForce / hit.distance);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 1f);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (IsGrounded())
        {
            Debug.Log("is Grounded");
        }
        // jump
        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && Time.time > _nextJump)
        {
            isJumping = true;
            Debug.Log("Jump");
            _nextJump = Time.time + _jumpCooldown;
            _rigidbody.AddForce(Vector3.up * (jumpHeight * 100));
        }

        if (isJumping)
        {
            if (Time.time > _nextJump)
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = hunterCamera.transform.forward * 12.0f;
        Physics.IgnoreCollision(
            collider1: bullet.GetComponent<SphereCollider>(),
            collider2: gameObject.GetComponent<SphereCollider>());
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2);
    }
}