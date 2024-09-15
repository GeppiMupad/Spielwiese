using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Tooltip("Decceleration after walking / sprinting -> slowing down player")]
    [SerializeField] private float currentDecceleration; // 20f


    // Acceleration

    private float maxSpeed;  
    private float currentAcceleration;
    private float currentSpeed = 0f;

 
    // Slow Player Down

    private float deccelerate;
    private Vector3 lastMove; 

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


    public float limitAngleX = 45;
    public float limitAngleY = 180;

    private float angleX;
    private float angleY;

    #endregion

    private CharacterController myController;

    private GetInput myInput;


    private Camera myCamera; // passiert bisher nix mit
    private void Awake()
    {
        myCamera = Camera.main;
    }

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

        if (moveDirection != new Vector3(0, 0, 0))
        {
            deccelerate = currentSpeed;
            lastMove = moveDirection;
            lastMove = cineMashineTransform.forward * lastMove.z + cineMashineTransform.right * lastMove.x;
        }


        if (moveDirection == new Vector3(0, 0, 0))
        {
            // slow player down over time, after no move input is given
            deccelerate -= currentDecceleration * Time.deltaTime;

            deccelerate = Mathf.Min(deccelerate, 10);

            if (deccelerate > 0)
            {
                myController.Move(lastMove * (currentSpeed - deccelerate * Time.deltaTime) * -1 + new Vector3(0f, jumpDirection.y * Time.deltaTime, 0f));
            }
        }

        myController.Move(moveDirection * (currentSpeed * Time.deltaTime) + new Vector3(0f, jumpDirection.y * Time.deltaTime, 0f));

        if (moveDirection != new Vector3(0, 0, 0) && myInput.isSprinting == true)
        {
            ShakeCamera();
        }
    }


    private void ShakeCamera()
    {
        // wenn player geittet wird muss camera geshaked werden
        // wenn player sprinten muss kamera leicht hoch und runter gehen
    }

    /// <summary>
    /// Noch unzufrieden ? irgendwie fehlt da das gewisse etwas, aber auch irgendwie nicht lol
    /// Will hier aber erstmal gucken wie es ist, wenn meine Map steht 
    /// </summary>
    private void RotateCamera()
    {

        if (this.gameObject.transform.rotation.x > -50f || this.gameObject.transform.rotation.x < 50f)
        {
            cineMashineTransform.transform.rotation = Quaternion.Euler(((myInput.rotation.y / rotationSpeedY) * -1) / 2, myInput.rotation.x / rotationSpeedX, 0);
            gameObject.transform.rotation = cineMashineTransform.transform.rotation;
        }
        else
        {
            cineMashineTransform.transform.rotation = Quaternion.Euler(((myInput.rotation.y / rotationSpeedY) * -1) / 2, myInput.rotation.x / rotationSpeedX, 0);
            gameObject.transform.rotation = cineMashineTransform.transform.rotation;
        }

        // 50 x , - 50 x

        // cineMashineTransform.transform.rotation = Quaternion.Euler(((myInput.rotation.y / rotationSpeedY) * -1) / 2, myInput.rotation.x / rotationSpeedX, 0);

    }



    /* 
     
    - Muss noch machen, dass sobald man aufhört zu gehen man kruzen "bremsweg" hat 
     -> Richtiung muss noch angepasst werden -> Die richtung , in die decceleratet wird ist Statisch und verändert sich nicht mit dem Kamera Movement
     -> WEnn man Sprintet und dann deceleratet fühlt es sich noch nicht ganz so gut an

     - Rotaion von meinem Object Anpassen / -> Eig irrelevant sofern es Single Player ist 
        - Gucken wies bei R6 bspw ist -> nur wichtig bei Multiplayer

     - Meine Kamera muss sich beim Sprinten etwas bewegen / wackeln 

     - Regeneration & Ausdauer ?
     
     */
}
