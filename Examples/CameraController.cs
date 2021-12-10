using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed = 0.1f;

    void Update() {
        transform.Translate(-Input.GetAxis("Horizontal")*speed, 0f, -Input.GetAxis("Vertical")*speed);
    }
}
