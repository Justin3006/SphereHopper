using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    [SerializeField]
    float movSpeed = 7;
    Vector3 movDir;

    [SerializeField]
    float rotSpeed = 360;
    float rotDir;

    [SerializeField]
    float camRotSpeed = 360;
    float camRotDir;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.rotation * (Time.fixedDeltaTime * movSpeed * movDir));
        //TODO: Change Rotate to Lerp for smoother controlls
        transform.Rotate(0, Time.fixedDeltaTime * rotSpeed * rotDir, 0);
        //TODO: Add Rotation Constraints to the Camera
        cam.transform.Rotate(-Time.fixedDeltaTime * camRotSpeed * camRotDir, 0, 0);
    }

    public void Move(Vector3 movDir)
    {
        this.movDir = movDir;
    }

    public void Rotate(float rotDir) 
    {
        this.rotDir = rotDir;
    }

    public void RotateCam(float camRotDir) 
    {
        this.camRotDir = camRotDir;
    }
}
