using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed = 0.5f;

    void Update() {
        transform.Rotate(-Input.GetAxis("Vertical") * speed, Input.GetAxis("Horizontal") * speed, 0f);
    }
}
