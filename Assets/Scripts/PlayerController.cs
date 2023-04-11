using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] inventory = {};
    public GameObject gun, sword;

    public Camera playerCamera;
    public Rigidbody playerRigid;
    public KeyCode left, right, forward, backward, jump, sprint, crouch, interact, reload;
    private float forwardSpeed, backwardSpeed, leftSpeed, rightSpeed, rotationX, aimSpeed, aimLimit;
    private Vector3 accelerationVector = Vector3.zero;
    private bool isGrounded, isJumping = false;
    // Start is called before the first frame update
    void Awake()
    {
        left = KeyCode.A;
        right = KeyCode.D;
        forward = KeyCode.W;
        backward = KeyCode.S;
        jump = KeyCode.Space;
        sprint = KeyCode.LeftShift;
        crouch = KeyCode.C;
        interact = KeyCode.E;
        reload = KeyCode.R;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        aimSpeed = 3f;
        aimLimit = 85f;
        rotationX = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        if(Input.GetKeyDown(forward) )
        {
            forwardSpeed = 1;
        }
        if(Input.GetKeyUp(forward))
        {
            forwardSpeed = 0;
        }
        if(Input.GetKeyDown(backward))
        {
            backwardSpeed = -1;
        }
        if(Input.GetKeyUp(backward))
        {
            backwardSpeed = 0;
        }
        if(Input.GetKeyDown(left))
        {
            leftSpeed = -1;
        }
        if(Input.GetKeyUp(left))
        {
            leftSpeed = 0;
        }
        if(Input.GetKeyDown(right))
        {
            rightSpeed = 1;
        }
        if(Input.GetKeyUp(right))
        {
            rightSpeed = 0;
        }
        accelerationVector.z = forwardSpeed + backwardSpeed;
        accelerationVector.x = leftSpeed + rightSpeed;
        accelerationVector = Vector3.ClampMagnitude(accelerationVector, 1);

        if(Input.GetKeyDown(jump))
        {
            if(isGrounded)
            {
                isJumping = true;
            }
        }
        #endregion

        #region Aim
        rotationX += -Input.GetAxis("Mouse Y") * aimSpeed;
        rotationX = Mathf.Clamp(rotationX, -aimLimit, aimLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * aimSpeed, 0);
        #endregion

        #region Interact
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f)) {
            if(Input.GetKeyDown(interact))
            {
                Interact(hit.transform.gameObject);
                // Debug.Log(hit.transform.gameObject);
            }
        }
        #endregion

        #region Shoot
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            gun.GetComponent<GunController>().shoot();
        }
        #endregion

        #region Reload
        if(Input.GetKeyDown(reload))
        {
            gun.GetComponent<GunController>().reload();
        }
        #endregion
    }

    void FixedUpdate()
    {
        var len = -1.3f;
        if(Physics.Raycast(transform.position, new Vector3(0, len, 0), out RaycastHit hitInfo, -1f * len))
        {
            Debug.DrawRay(transform.position, new Vector3(0, len, 0), Color.green);
            transform.position = transform.position - new Vector3(0, len + hitInfo.distance + 0.05f, 0);
            playerRigid.velocity = playerRigid.velocity + new Vector3(0, -1f * playerRigid.velocity.y, 0);
            isGrounded = true;
        } else {
            Debug.DrawRay(transform.position, new Vector3(0, len, 0), Color.red);
            isGrounded = false;
        }

        if(isGrounded)
        {
            playerRigid.AddRelativeForce(accelerationVector * 100, ForceMode.Acceleration);
            playerRigid.velocity = AdjustVelocityToSlope(playerRigid.velocity); 
            playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, 5);
            playerRigid.velocity = playerRigid.velocity - (playerRigid.velocity * 9f * Time.fixedDeltaTime);
            if(isJumping)
            {
                playerRigid.AddForce(playerRigid.transform.up * 9, ForceMode.Impulse);
                isJumping = false;
            }
        }
        else
        {
            playerRigid.AddRelativeForce(accelerationVector * 5, ForceMode.Acceleration);
            playerRigid.AddRelativeForce(new Vector3(0, -22f, 0), ForceMode.Acceleration);
        }
    //MOVEMENT PHYSICS CALCULATION
    }

    private Vector3 AdjustVelocityToSlope(Vector3 v)
    {
        var ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * v;


            if(adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }
        return v;
    }

    void Interact(GameObject interactObject)
    {
        if(interactObject.tag == "Door")
        {
            Debug.Log("Using Door...");
            interactObject.GetComponent<DoorController>().useDoor();
        }
    }

    void getInventorySlot(int itemNumber)
    {
        //not sure, look this up
        //press number, select that number in array, set that object to active
    }
}
