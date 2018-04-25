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
    private SCR_CharacterStats myStats; //Referencia de los stats del jugador para determinar que tipo de pinguino es
    private Quaternion activeModelRotation; //Rotacion del modelo
    //Bool para checar en que momento esta tocando el suelo el jugador
    private bool isGrounded;
    private Vector3 normalVector;    //El vector normal a la superficie
    private float boostTimer = 0;
    private Animator activeModelAnim;

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
    public float maxDefaultSpeed = 20;
    public float maxBoostAmount = 30;

    [Header("Camera")]
    public Transform mainCamera;

    [Header("Battle Variables")]
    public float maxForwardSpeed = 20;  //Velocidad maxima cuando se va hacia adelante
    public float maxReverseSpeed = -10; //Velocidad maxima cuando se va en reversa
    public float currentSpeed;  //Velocidad actual del jugador
    public float acceleration = 5;  //Que tan rapido alcanza la velocidad maxima el jugador
    public float steerForce = 0.5f; //Que tan aguda es la vuelta 
    public float walkingSpeed = 1.5f;   //Velocidad del pinguino cuando esta caminando 
    public Vector3 steerVector;    //Direccion a la que se mueve el volante, se puede invertir
    public float cameraDirection = 1;   //Direccion en la que se pone la camara, 1 para atras del jugador y -1 para adelante

    //Inicializamos a nuestro jugador
    public void MyStart()
    {
        myRB = GetComponent<Rigidbody>();   //Referencia del RB
        activeModelAnim = activeModel.GetComponent<Animator>(); //Animator que se encuentra en el modelo activo
        myStats = GetComponent<SCR_CharacterStats>();   //Referencia de las stats del jugador. Varia en las clases de los pinguinos
    }

    //Usamos OnTriggerStay y Exit para detectar cuando estamos en el suelo o algun tipo de superficie
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = true;
    }

    //Usamos OnTriggerStay y Exit para detectar cuando estamos en el suelo o algun tipo de superficie
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = false;
    }

    //Update en el que registramos los inputs y efectos externos
    public void MyUpdate()
    {
        GetInput(playerPrefix); //Captura de movimiento
        BoostTimerManager();    //Manejo del boost
    }
    //Fixed Update para calcular fisica y movimiento de camara
    public void MyFixedUpdate()
    {
        CharacterMovement(Time.fixedDeltaTime); //Motor del jugador
        CameraPlacement();  //Comportamiento de la camara
    }

    //Funcion que se encarga de manejar el movimiento del jugador
    void CharacterMovement(float _delta)
    {
        RaycastManager();   //Funcion donde se maneja la posicion del jugador con respecto al mundo
        AnimationManager();   //Animaciones
        if (!isGrounded)
        {
            if (myRB.velocity == Vector3.zero)
                myRB.velocity = transform.forward;
            Quaternion myRotation = Quaternion.LookRotation(myRB.velocity);
            activeModel.rotation = Quaternion.Slerp(activeModel.rotation, myRotation, _delta * 10);
            return;
        }
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
        activeModel.rotation = Quaternion.Slerp(activeModel.rotation, activeModelRotation, _delta * 10);
        //Ponemos los limites de la velocidad
        currentSpeed = Mathf.Clamp(currentSpeed, maxReverseSpeed, maxForwardSpeed);
       
        //Checamos si estamos acelerando o en reversa para dar el control de movimiento adecuado
        if (currentSpeed <= 0.1f && currentSpeed >= -0.1f)
        {
            verticalInput = Input.GetAxisRaw(playerPrefix + "Vertical");
            Vector3 myVelocity = steerVector * verticalInput * walkingSpeed;
            myRB.velocity = new Vector3(myVelocity.x, myRB.velocity.y, myVelocity.z);    //Asignamos la velocidad a nuestro RB
            Quaternion pingoRotation = Quaternion.LookRotation(steerVector);
            activeModel.rotation = Quaternion.Slerp(activeModel.rotation, pingoRotation, _delta *5);
        }
        else
        {
            //Asignamos la velocidad a nuestro RB
            Vector3 myVelocity = CrashCheck();
            if (myRB.velocity.magnitude <= 0.01f)
                currentSpeed = 0;
            myRB.velocity = new Vector3(myVelocity.x, myRB.velocity.y, myVelocity.z);
        }

    }

    Vector3 CrashCheck()
    {
        Vector3 myVelocity = transform.forward * currentSpeed * myStats.speed;
        float distanceToGround = 1.0f;
        Vector3 origin = transform.position + new Vector3(0, 1.0f, 0);
        Vector3 direction = activeModel.forward;

        //RaycastHit hit;
        //if (Physics.Raycast(origin, direction, out hit, distanceToGround) && currentSpeed > 10)
        //{
        //    currentSpeed = 0;
        //    myVelocity = Vector3.zero;
        //}
        //Debug.DrawRay(origin, direction * distanceToGround, Color.blue);

        return myVelocity;
    }

    //Aqui iran todas las animaciones
    void AnimationManager()
    {
        MovementAnimations();
    }

    //Funcion que maneja las animaciones de movimiento
    void MovementAnimations()
    {
        activeModelAnim.SetBool("Grounded", isGrounded);
        activeModelAnim.SetFloat("CurrentSpeed", currentSpeed);
        activeModelAnim.SetFloat("Horizontal", horizontalInput);
    }
    //Funcion que detecta cuando el jugador esta derrapando
    void DriftingBehaviour()
    {
        if (drifting)
        {
            float driftDirection = Mathf.Sign(horizontalInput);
            horizontalInput = driftDirection * 10 * myStats.handling;
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
        if (!drifting)
            horizontalInput = (Input.GetAxisRaw(_playerPrefix + "Horizontal") * steerForce * myStats.handling);
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
    public void RaycastManager()
    {
        float distanceToGround = 1.2f;
        Vector3 origin = transform.position + new Vector3(0, 0.1f, 0);
        Vector3 direction = -transform.up;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distanceToGround))
        {
            normalVector = hit.normal;  //Capturamos el vector normal a la superficie
            activeModelRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            //transform.position = hit.point;
        }
        Debug.DrawRay(origin, direction * distanceToGround, Color.red);
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

    //Getter de RigidBody
    public Rigidbody GetMyRB()
    {
        return myRB;
    }
}