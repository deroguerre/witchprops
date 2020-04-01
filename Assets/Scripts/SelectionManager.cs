using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SelectionManager : NetworkBehaviour {

	[SerializeField] private Material highlightMaterial;

	public GameObject[] allAvailableProps;

	private int propsIterator = 0;

	// Update is called once per frame
	void Update()
    {

		RaycastHit[] hits;

		//Vector3 raycastDirection = Input.mousePosition - Camera.main.transform.position;
		//Debug.DrawRay(Camera.main.transform.position, raycastDirection * -1, Color.red);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0F);

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit hit = hits[i];
			if(hit.transform.tag == "Selectable" && Input.GetButtonDown("Fire2")) {
				Debug.Log("pressed");
				CmdChangeModel();
			}
		}

		if(Input.GetButtonDown("Fire2")) {
			CmdChangeModel();
		}
	}

	[Command]
	private void CmdChangeModel() {
		RpcChangeMesh();
	}

	[ClientRpc]
	private void RpcChangeMesh() {

		GameObject player = GameObject.FindWithTag("Player");
		player.transform.GetComponent<Transform>().position += new Vector3(0,0.1f,0);

		player.GetComponent<MeshFilter>().sharedMesh = allAvailableProps[propsIterator].GetComponent<MeshFilter>().sharedMesh;
		player.GetComponent<MeshCollider>().sharedMesh = allAvailableProps[propsIterator].GetComponent<MeshFilter>().sharedMesh;

		if (propsIterator == allAvailableProps.Length - 1) {
			propsIterator = 0;
		} else {
			propsIterator++;
		}

	}
}
