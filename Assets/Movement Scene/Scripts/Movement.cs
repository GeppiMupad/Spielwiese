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
    [SerializeField] private float walkAcceleration;

    [Tooltip("Acceleration for Sprinting")]
    [SerializeField] private float sprintAcceleration;

    [Tooltip("Decceleration if player sprints")]
    [SerializeField] private float sprintDeceleration;

    [Tooltip("Decceleration if player walks")]
    [SerializeField] private float walkDeceleration;

    // Acceleration

    private float maxSpeed;  
    private float currentAcceleration;
    private float currentSpeed = 0f;

 
    // Slow Player Down

    private float decelerate;
    private Vector3 lastDirection; 

    private bool hasSprinted = false; // change value of Decceleration - sollte mit FSM wegfallen

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

    private CharacterController playerController;

    private GetInput playerInput;

    private Camera myCamera; // passiert bisher nix mit

    private void Awake()
    {
        myCamera = Camera.main;
    }

    private void Start()
    {
        playerController = GetComponent<CharacterController>();

        playerInput = GetComponent<GetInput>();
    }

    void Update()
    {
        JumpAndGravity();

        RotateCamera();

        PlayerMovement();
    }

    private void JumpAndGravity()
    {
        if (playerController.isGrounded == true)
        {
            hasJumped = false;
        }

        if (playerInput.isJumping == true && playerController.isGrounded == true)
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

    /// <summary>
    /// Accelerate when starts moving, Deccelerate when stop moving , Change between Sprints and noraml walk
    /// </summary>
    private void PlayerMovement()
    {
        // Set Speed, the player is walking with
        if (playerInput.isSprinting == true && playerInput.walk.y > 0)
        {
            maxSpeed = sprintSpeed;
            currentAcceleration = sprintAcceleration;
            hasSprinted = true;
        }
        else
        {
            maxSpeed = normalSpeed;
            currentAcceleration = walkAcceleration;
        }

        // Get direction the player is walking
        Vector3 moveDirection = new Vector3(playerInput.walk.x, 0.0f, playerInput.walk.y).normalized;

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
       
            // save last direction for decceleration
        if (moveDirection != new Vector3(0, 0, 0))
        {
            decelerate = currentSpeed;
            lastDirection = moveDirection;

            //lastDirection = new Vector3(0 , moveDirection.x , moveDirection.z);
            //lastMove = cineMashineTransform.forward * lastMove.z + cineMashineTransform.right * lastMove.x;
        }

        if (moveDirection == new Vector3(0, 0, 0))
        {
            // slow player down over time, after no move input is given ( change deccleration based on speed )
            if (maxSpeed == sprintSpeed || hasSprinted == true)
            {     
                decelerate -= sprintDeceleration * Time.deltaTime;
   
               // deccelerate = Mathf.Min(deccelerate, tempFloat); // überhaupt nötig ? 
            }
            else
            {
                decelerate -= walkDeceleration * Time.deltaTime;

               // deccelerate = Mathf.Min(deccelerate, 10);
            }

            if (decelerate > 0)
            {
                playerController.Move(lastDirection * (currentSpeed - decelerate * Time.deltaTime) * -1 + new Vector3(0f, jumpDirection.y * Time.deltaTime, 0f));
            }
            else
            {
                hasSprinted = false;
            }
        }

        playerController.Move(moveDirection * (currentSpeed * Time.deltaTime) + new Vector3(0f, jumpDirection.y * Time.deltaTime, 0f));

        if (moveDirection != new Vector3(0, 0, 0) && playerInput.isSprinting == true)
        {
            // The Arms of the player has to shake instead of the camera
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
            cineMashineTransform.transform.rotation = Quaternion.Euler(((playerInput.rotation.y / rotationSpeedY) * -1) / 2, playerInput.rotation.x / rotationSpeedX, 0);
            gameObject.transform.rotation = cineMashineTransform.transform.rotation;
        }
        else
        {
            cineMashineTransform.transform.rotation = Quaternion.Euler(((playerInput.rotation.y / rotationSpeedY) * -1) / 2, playerInput.rotation.x / rotationSpeedX, 0);
            gameObject.transform.rotation = cineMashineTransform.transform.rotation;
        }

        // 50 x , - 50 x -> Maximale Rotations position
    }



    /* 
     
    - Muss noch machen, dass sobald man aufhört zu gehen man kruzen "bremsweg" hat 

    - Wenn man decceleratet : 


     -> Richtiung muss noch angepasst werden -> Die richtung , in die decceleratet wird ist Statisch und verändert sich nicht mit dem Kamera Movement
     -> WEnn man Sprintet und dann deceleratet fühlt es sich noch nicht ganz so gut an

     - Rotaion von meinem Object Anpassen / -> Eig irrelevant sofern es Single Player ist 
        - Gucken wies bei R6 bspw ist -> nur wichtig bei Multiplayer

     - Meine Kamera muss sich beim Sprinten etwas bewegen / wackeln 

     - Regeneration & Ausdauer ?
     
     - Code Ausblendungen ( // ) überarbeiten

     - Variablen in einer richtigen Reihenfolge sortieren
     
     */
}
