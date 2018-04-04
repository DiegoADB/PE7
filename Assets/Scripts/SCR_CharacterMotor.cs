//Made by Diego Diaz
using UnityEngine;
using System.Collections;

public class SCR_CharacterMotor : MonoBehaviour 
{
    /// <summary>
    /// 
    /// Controlador de movimiento o Motor de los pinguinos
    ///     Controles: 
    /// Boton A: Acelerar
    /// Boton B: Frenar / Reversa
    /// Palanca Izquierda: Dar Direccion
    /// R o L Trigger: Drift
    /// 
    /// </summary>


    //Referencia del rigidbody
    private Rigidbody myRB;
    //Bool para checar en que momento esta tocando el suelo el jugador
    private bool isGrounded;
    private Vector3 normalVector;    //El vector normal a la superficie
    private float boostTimer = 0;

    //Input del jugador
    private float verticalInput;    
    private float horizontalInput;  
    private bool aButton;
    private bool bButton;
    private bool drifting;

    //Controlador
    [Header("Controller")]
    public string playerPrefix = "P1_";
    public Transform activeModel;

    //Stats Default del jugador
    [Header("Default Stats")]
    public float maxDefaultSpeed = 15;
    public float maxBoostAmount = 30;

    [Header("Camera")]
    public Transform mainCamera;

    [Header("Battle Variables")]
    public float maxForwardSpeed = 15;  //Velocidad maxima cuando se va hacia adelante
    public float maxReverseSpeed = -10; //Velocidad maxima cuando se va en reversa
    public float currentSpeed;  //Velocidad actual del jugador
    public float acceleration = 10;  //Que tan rapido alcanza la velocidad maxima el jugador
    public float steerForce = 0.5f; //Que tan aguda es la vuelta 
    public Vector3 steerVector;    //Direccion a la que se mueve el volante, se puede invertir
    public float cameraDirection = 1;   //Direccion en la que se pone la camara, 1 para atras del jugador y -1 para adelante

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();   //Referencia del RB
    }

    private void Update()
    {
        GetInput(playerPrefix); //Captura de movimiento
        BoostTimerManager();    //Manejo del boost
    }
    private void FixedUpdate()
    {
        CharacterMovement(Time.fixedDeltaTime); //Motor del jugador
        CameraPlacement();  //Comportamiento de la camara
    }

    //Funcion que se encarga de manejar el movimiento del jugador
    void CharacterMovement(float _delta)
    {
        //Checamos si esta en el suelo
        isGrounded = IsGrounded();
        Debug.DrawRay(transform.position, myRB.velocity * 10, Color.blue);
        if (myRB.velocity == Vector3.zero)
            myRB.velocity = transform.forward;
        Quaternion myRotation = Quaternion.LookRotation(myRB.velocity);
        activeModel.rotation = myRotation;
        if (!isGrounded)
            return;

        //Checamos si estamos derrapando
        DriftingBehaviour();
        //Acelerar
        if (aButton)
            currentSpeed += _delta * acceleration;
        //Frenar e ir en reversa
        if (bButton)
            currentSpeed -= _delta * acceleration * 2;

        //Nos detenemos si dejamos de movernos
        if (!aButton && !bButton)
        {
            if (currentSpeed > 0)
                currentSpeed -= _delta * 2;
            if (currentSpeed < 0)
                currentSpeed += _delta * 2;
        }

        //Manejamos a que direccion queremos ir
        steerVector = ((verticalInput * transform.forward) + (horizontalInput * transform.right)).normalized;
        if (steerVector == Vector3.zero)
            steerVector = transform.forward;
        Quaternion steerDirection = Quaternion.LookRotation(steerVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, steerDirection, _delta);

        //Ponemos los limites de la velocidad
        currentSpeed = Mathf.Clamp(currentSpeed, maxReverseSpeed, maxForwardSpeed);

        //Checamos si estamos acelerando o en reversa para dar el control de movimiento adecuado
        if (currentSpeed <= 0.1f && currentSpeed >= -0.1f)
        {
            verticalInput = Input.GetAxisRaw(playerPrefix + "Vertical");
            myRB.velocity = steerVector * verticalInput;    //Asignamos la velocidad a nuestro RB
        }
        else
        {
            //Asignamos la velocidad a nuestro RB
            myRB.velocity = GetRealForward() * currentSpeed;
        }

    }


    //Funcion que detecta cuando el jugador esta derrapando
    void DriftingBehaviour()
    {
        if (drifting)
        {
            float driftDirection = Mathf.Sign(horizontalInput);
            horizontalInput = driftDirection * 10;
        }
    }

    //Funcion que regresa el vector frontal del jugador en todo momento que se encuentre en el suelo
    Vector3 GetRealForward()
    {
        if (!isGrounded)
            return transform.forward;
        else
        {
            Vector3 realForward = Vector3.Cross(-normalVector, transform.right).normalized;   //Calculamos el vector frontal
            return realForward;
        }
    }

    //Se captura el input del jugador
    void GetInput(string _playerPrefix)
    {
        verticalInput = Mathf.Abs(Input.GetAxisRaw(_playerPrefix + "Vertical"));
        verticalInput = Mathf.Clamp(verticalInput, 0.5f, 1.0f);
        if(!drifting)
            horizontalInput = (Input.GetAxisRaw(_playerPrefix + "Horizontal") * steerForce);
        aButton = Input.GetButton(_playerPrefix + "A");
        bButton = Input.GetButton(_playerPrefix + "B");

        //Drifting
        if (Input.GetAxisRaw(_playerPrefix + "RTrigger") != 0 || Input.GetAxisRaw(_playerPrefix + "LTrigger") != 0 || Input.GetButton(_playerPrefix + "RTrigger") || Input.GetButton(_playerPrefix + "LTrigger"))
            drifting = true;
        else
            drifting = false;
    }

    //Manejamos la posicion de la camara
    void CameraPlacement()
    {
        Vector3 desiredPosition = (-cameraDirection * (transform.forward * 4) + (transform.up * 2)) + transform.position;
        mainCamera.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.fixedDeltaTime * 5);
        mainCamera.LookAt(transform.position);
        //Acomodamos la camara segun la direccion en la que estemos moviendonos
        if (currentSpeed < 0)
            cameraDirection = -1;
        else
            cameraDirection = 1;
    }

    //Lanzamos un raycast hacia abajo para determinar si estamos en el suelo
    public bool IsGrounded()
    {
        bool grounded = false;
        float distanceToGround = 0.1f;
        Vector3 origin = transform.position;
        Vector3 direction = -Vector3.up;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distanceToGround))
        {
            grounded = true;
            normalVector = hit.normal;  //Capturamos el vector normal a la superficie
            Vector3 targetPosition = hit.point;
        }
        Debug.DrawRay(origin, direction * distanceToGround, Color.red);
        return grounded;
    }

    //Impulso de velocidad
    public void SpeedBoost(float _duration)
    {
        maxForwardSpeed += 10;
        if (maxForwardSpeed > maxBoostAmount)
            maxForwardSpeed = maxBoostAmount;

        currentSpeed = maxForwardSpeed;
        boostTimer = _duration;
    }

    //Manejamos el boost que recibimos en paneles y otros bonos 
    void BoostTimerManager()
    {
        boostTimer -= Time.deltaTime;
        if (boostTimer <= 0)
        {
            if (maxForwardSpeed > maxDefaultSpeed)
            {
                maxForwardSpeed -= Time.fixedDeltaTime;
            }
            if (maxForwardSpeed < maxDefaultSpeed)  
                maxForwardSpeed = maxDefaultSpeed;
            boostTimer = 0;
        }
    }
}