using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // speed is parsecs per second
    // 31,540,000 seconds in a year
    // 1 parsec = 3.26156 light years
    // default speed is 1 light year per second (or 31.54 million times the speed of light)
    public float speed = 1f/3.26156f;

    void Update() {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0f);
        }
    }
}
