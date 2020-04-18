using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuParralax : MonoBehaviour
{
    // Start is called before the first frame update
    public float xParallaxPower = 0;
    public float yParallaxPower = 0;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        Vector3 newPosition = new Vector3(x * (0.0001f * xParallaxPower), y * (0.0001f * yParallaxPower), 0);
        gameObject.transform.position = initialPosition + newPosition;
        // _camTargetRb.position = Vector3.Lerp(transform.position, _camTargetRb.position, Time.deltaTime * 50);
    }
}

