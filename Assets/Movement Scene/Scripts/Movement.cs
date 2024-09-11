using UnityEngine;


public class Movement : MonoBehaviour
{
    #region Walk & Sprint

    [Header("Walk & Sprint")]
    [Space(3)]
    [Tooltip("Normal walk Speed")]
    [SerializeField] private float normalSpeed;

    [Tooltip("Sprint Speed")]
    [SerializeField] private float sprintSpeed;

    [Tooltip("Acceleration for walking")]
    [SerializeField] private float normalAcceleration;

    [Tooltip("Acceleration for Sprinting")]
    [SerializeField] private float sprintAcceleration;

    private float maxSpeed;
    private float currentAcceleration;
    private float currentSpeed = 0f;

    #endregion

    #region Jump & Grounded

    [Header("Jump & Gravity")]
    [Space(3)]
    [Tooltip("Jump Height")]
    [SerializeField] private float jumpHeight;

    [Tooltip("Gravity if player has Jumped")]
    [SerializeField] private float jumpGravity;

    [Tooltip("Gravity if player falls down of something")]
    [SerializeField] private float normalGravity;

    private Vector3 jumpDirection = Vector3.zero;

    private bool hasJumped = false;

    #endregion

    #region Camera & Rotation

    [Space(5)]
    [Header("Camera")]
    [Space(5)]

    [Tooltip("Rotation Y get devided by 2 -> feels better")]
    [SerializeField] private float rotationSpeedY;

    [SerializeField] private float rotationSpeedX;

    [Tooltip("Put Virtual Camera inhere ( Cinemachine logic of Cam )")]
    [SerializeField] private Transform cineMashineTransform;


    #endregion

    private CharacterController myController;

    private GetInput myInput;

    private void Start()
    {
        myController = GetComponent<CharacterController>();

        myInput = GetComponent<GetInput>();
    }

    void Update()
    {
        JumpAndGravity();

        RotateCamera();

        PlayerMovement();
    }

    private void JumpAndGravity()
    {
        if (myController.isGrounded == true)
        {
            hasJumped = false;
        }

        if (myInput.isJumping == true && myController.isGrounded == true)
        {
            jumpDirection.y = jumpHeight;
            hasJumped = true;
        }

        // change between smallGravity & Gravity
        if (hasJumped == true)
        {
            jumpDirection.y -= jumpGravity * Time.deltaTime;
        }
        else
        {
            jumpDirection.y -= normalGravity * Time.deltaTime;
        }
    }

    private void PlayerMovement()
    {
        // Set Speed, the player is walking with
        if (myInput.isSprinting == true && myInput.walk.y > 0)
        {
            maxSpeed = sprintSpeed;
            currentAcceleration = sprintAcceleration;
        }
        else
        {
            maxSpeed = normalSpeed;
            currentAcceleration = normalAcceleration;
        }


        // Get direction the player is walking
        Vector3 moveDirection = new Vector3(myInput.walk.x, 0.0f, myInput.walk.y).normalized;

        // Create acceleration
        if (moveDirection.magnitude > 0)
        {
           // currentSpeed += acceleration * Time.deltaTime;

            currentSpeed += currentAcceleration * Time.deltaTime;

            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
        else  // no vector lenght, no speed
        {
            currentSpeed = 0;
        }

        // move into direction the camera is looking at
        moveDirection = cineMashineTransform.forward * moveDirection.z + cineMashineTransform.right * moveDirection.x;

        myController.Move(moveDirection * (currentSpeed * Time.deltaTime) + new Vector3(0f, jumpDirection.y * Time.deltaTime, 0f));
    }

    /// <summary>
    /// Noch unzufrieden ? irgendwie fehlt da das gewisse etwas, aber auch irgendwie nicht lol
    /// </summary>
    private void RotateCamera()
    {
        cineMashineTransform.transform.rotation = Quaternion.Euler(((myInput.rotation.y / rotationSpeedY) * -1) / 2, myInput.rotation.x / rotationSpeedX, 0);
        gameObject.transform.rotation = cineMashineTransform.transform.rotation;
    }



    /* 
     
     - Rotaion von meinem Object Anpassen
        - Gucken wies bei R6 bspw ist -> nur wichtig bei Multiplayer

     - Meine Kamera muss sich beim Sprinten etwas bewegen / wackeln 

     - Regeneration & Ausdauer ?
     
     */
}
