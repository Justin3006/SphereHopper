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

    [SerializeField]
    float jumpPower = 10;
    bool jump;

    [SerializeField]
    float dashSpeed = 20;
    [SerializeField]
    float dashDurationMax = 0.25f;
    float dashDuration;
    [SerializeField]
    float dashCDMax = 1;
    float dashCD;
    bool dash;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move position
        rb.MovePosition(transform.position + transform.rotation * (Time.fixedDeltaTime * movSpeed * movDir));
        
        //rotate body
        //TODO: Change Rotate to Lerp for smoother controlls
        transform.Rotate(0, Time.fixedDeltaTime * rotSpeed * rotDir, 0);

        //rotate cam 
        float camRotChange = -Time.fixedDeltaTime * camRotSpeed * camRotDir;
        float newCamRot = camRotChange + cam.transform.rotation.eulerAngles.x;
        
        if (newCamRot < 270 && newCamRot >= 180)
            cam.transform.LookAt(cam.transform.position + transform.up, -transform.forward);
        else if (newCamRot > 90 && newCamRot < 180)
            cam.transform.LookAt(cam.transform.position - transform.up, transform.forward);
        else
            //TODO: Change Rotate to Lerp for smoother controlls
            cam.transform.Rotate(camRotChange, 0, 0);

        //jump
        if (jump && Physics.Raycast(transform.position + 0.01f * transform.up, -transform.up, 0.1f))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
        }

        jump = false;

        //dash
        if (dash && dashCD <= 0) 
        {
            dashDuration = dashDurationMax;
            dashCD = dashCDMax;
        }
        if (dashDuration > 0)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * dashSpeed * movDir);
            dashDuration -= Time.fixedDeltaTime;
        }
        if (dashCD > 0) 
        {
            dashCD -= Time.fixedDeltaTime;
        }
        dash = false;
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

    public void Jump() 
    {
        jump = true;
    }

    public void Dash() 
    {
        dash = true;
    }
}
