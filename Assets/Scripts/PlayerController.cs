using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //Handle basic movement
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 movDir = new Vector3(xMove, 0, zMove).normalized;

        motor.Move(movDir);

        //Handle body rotation
        float yRotate = Input.GetAxis("Mouse X");
        motor.Rotate(yRotate);

        //Handle cam rotation
        float xRotate = Input.GetAxis("Mouse Y");
        motor.RotateCam(xRotate);

        //TODO: Jump, Dash, other Interactions
    }
}
