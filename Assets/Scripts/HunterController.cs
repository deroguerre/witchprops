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

    public GameObject hunterCamera;

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

    private void Awake()
    {
        //_mainCamera = GameObject.Find("CameraPrincipal").GetComponent<Camera>();
        //_mainCamera.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        // init camera components

        //GameObject camGO = Instantiate(cameraPrefab);
        //camGO.GetComponent<MSCameraController>().target = this.gameObject.transform;
        //camGO.SetActive(true);
        //_mainCamera = camGO.GetComponent<Camera>();

        //_camTarget = GameObject.FindGameObjectWithTag("CamTarget");
        //_camTargetRb = _camTarget.GetComponent<Rigidbody>();
        //setupCamera();


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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 movement = move * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // get axis value from inputs
        //Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //input = Vector2.ClampMagnitude(input, 1);

        ////player movements
        //Vector3 movement = input * (Time.fixedDeltaTime * moveSpeed);
        //transform.position += movement;

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
        //Vector3 mouseWorldPosition =
        //    _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //transform.LookAt(mouseWorldPosition);
        //transform.rotation =
        //    Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
            return;

        //_camTargetRb.position = Vector3.Lerp(transform.position, _camTargetRb.position, Time.deltaTime * 50);
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = (GameObject) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = hunterCamera.transform.forward * 12.0f;
        Physics.IgnoreCollision(bullet.GetComponent<SphereCollider>(), this.gameObject.GetComponent<SphereCollider>());
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2);
    }

    private void setupCamera()
    {
        //Camera hunterCamera = GameObject.Find("HunterCamera").GetComponent<Camera>();
        //this.gameObject.GetComponent<MSCameraController>().cameras[0]._camera = hunterCamera;

        //MSCameraController cameraController = Camera.main.GetComponent<MSCameraController>();
        //cameraController.target = this.gameObject.transform;
        //Camera.main.transform.parent = this.gameObject.transform;
        //Destroy(GameObject.Find("CamTarget"));
        //cameraController.cameras[0].rotationType = MSACC_CameraType.TipoRotac.FirstPerson;

        //Camera.main.GetComponent<MSCameraController>().cameras[0].rotationType = MSACC_CameraType.TipoRotac.FirstPerson;

        //Camera hunterCamera = Instantiate(hunterCameraPrefab);
        //hunterCamera.GetComponent<MSCameraController>().target = this.gameObject.transform;
        //Camera.main.gameObject.SetActive(false);
        //hunterCameraPrefab.gameObject.SetActive(true);

        //GameObject.Find("MainCamera").SetActive(false);
        //GameObject.Find("FPSCamera").SetActive(true);

        //return hunterCamera;
    }
}