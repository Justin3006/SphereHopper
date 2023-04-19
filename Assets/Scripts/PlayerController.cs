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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //SECTION: Movement
        // Handle basic movement.
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 movDir = new Vector3(xMove, 0, zMove).normalized;

        motor.Move(movDir);


        // Handle body rotation.
        float yRotate = Input.GetAxis("Mouse X");
        motor.Rotate(yRotate);


        // Handle cam rotation.
        float xRotate = Input.GetAxis("Mouse Y");
        motor.RotateCam(xRotate);


        // Handle advanced movement.
        if (Input.GetButtonDown("Jump"))
            motor.Jump();

        if (Input.GetButtonDown("Dash"))
            motor.Dash();

        if (Input.GetButtonDown("Walk"))
            motor.Walk();


        //SECTION: Combat
        // Handle combat.
        if (Input.GetButtonDown("Fire1"))
            motor.Attack();
        else if (Input.GetButtonDown("Fire2"))
            motor.Parry();

        // Handle Lock On.
        //TODO: Replace with generic Button Input
        if (Input.GetButtonDown("Lock On")) 
        {
            motor.LockOn();
        }

        // Handle Abilities.
        if (Input.GetButtonDown("Ability")) 
        {
            motor.ActivateAbility();
        }

        //TODO: replace with Mouse ScrollWheel?
        if (Input.GetButtonDown("Ability Switch Up"))
            motor.SwitchAbility(1);
        if (Input.GetButtonDown("Ability Switch Down"))
            motor.SwitchAbility(-1);


        //SECTION: Miscellaneous
        // Handle Interactions.
        if (Input.GetButtonDown("Interact"))
            motor.Interact();
    }
}
