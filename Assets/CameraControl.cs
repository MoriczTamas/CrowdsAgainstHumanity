using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public float moveSpeed = 5;
    public float rotateSpeed = 2.0f;
    private bool mouseEnabled = true;
    void Update () {
        if (Input.GetKey(KeyCode.B))
        {
            mouseEnabled = !mouseEnabled;
        }
        if (mouseEnabled)
        {
            float dt = Time.deltaTime;
            Vector3 move = new Vector3(0, 0, 0);
            if (Input.GetAxis("Horizontal") < 0)
            {
                move = move + new Vector3(-1, 0, 0);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                move = move + new Vector3(1, 0, 0);
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                move = move + new Vector3(0, 0, 1);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                move = move + new Vector3(0, 0, -1);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                move = move + new Vector3(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                move = move + new Vector3(0, -1, 0);
            }
            move.Normalize();
            transform.Translate(move * dt * moveSpeed);

            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.RotateAround(Vector3.up, dt * rotateSpeed);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.RotateAround(Vector3.up, -dt * rotateSpeed);
            }
            Vector3 rightVector = transform.rotation * Vector3.right;
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.RotateAround(rightVector, -dt * rotateSpeed);
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                transform.RotateAround(rightVector, dt * rotateSpeed);
            }
        }

	}
}
