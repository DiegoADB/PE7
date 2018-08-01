using UnityEngine;
using System.Collections;

public enum AUDIOSOUNDS
{
    CHOQUE,
    DERROTA,
    DESLIZAR,
    FRENAR,
    GIRO,
    IDLE,
    PASOS,
    PASOSREVERSA,
    VICTORIA
}

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
    [HideInInspector]
    public  Rigidbody myRB;
    private SCR_CharacterStats myStats; //Referencia de los stats del jugador para determinar que tipo de pinguino es
    private Quaternion activeModelRotation; //Rotacion del modelo
    private bool collidedWithObstacle;
    //Bool para checar en que momento esta tocando el suelo el jugador
    public bool isGrounded;
    public Vector3 savedPosition;
    private Vector3 normalVector;    //El vector normal a la superficie
    private float boostTimer = 0;
    private Animator activeModelAnim;
    private float timerPosition;
    private bool cameraRotateAroundPlayer = true;
    private bool breaking = false;
    [HideInInspector]
    public bool isIA = false;

    [Header("Audio")]
    public AudioClip[] clips;
    public AudioSource audioSource;

    //Input del jugador
    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public bool aButton;
    [HideInInspector]
    public bool bButton;
    [HideInInspector]
    public bool drifting;

    [HideInInspector]
    public bool chocado = false;

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
    public bool mayhemState = false;
    public Vector3 laFuerza;
    public Vector3 laDireccion;

    [Header("Particles")]
    public GameObject sparks;
    public GameObject yaw;

    public GameObject[] addOns;

    public void RandomAddOn()
    {
        int randomAddOn = Random.Range(0, 12);
        for (int i = 0; i < addOns.Length; i++)
        {
            addOns[i].SetActive(false);
        }
        addOns[randomAddOn].SetActive(true);
    }


    //Inicializamos a nuestro jugador
    public void MyStart()
    {
        RandomAddOn();
        timerPosition = 0.0f;
        myRB = GetComponent<Rigidbody>();   //Referencia del RB
        activeModelAnim = GetComponent<Animator>(); //Animator que se encuentra en el modelo activo
        myStats = GetComponent<SCR_CharacterStats>();   //Referencia de las stats del jugador. Varia en las clases de los pinguinos
        savedPosition = this.transform.position;
    }

    //Usamos OnTriggerStay y Exit para detectar cuando estamos en el suelo o algun tipo de superficie
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //Usamos OnTriggerStay y Exit para detectar cuando estamos en el suelo o algun tipo de superficie
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle") && !collidedWithObstacle && currentSpeed > 5)
        {
            currentSpeed = 0;
            myRB.velocity = Vector3.zero;
            Debug.Log("Collide");
            collidedWithObstacle = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle") && collidedWithObstacle)
            collidedWithObstacle = false;
    }

    //Update en el que registramos los inputs y efectos externos
    public void MyUpdate()
    {
        GetInput(playerPrefix); //Captura de movimiento
        BoostTimerManager();    //Manejo del boost
        SaveLastPosition();     //Guarda la ultima posicion en la que estuvo el jugador antes de caer
    }
    //Fixed Update para calcular fisica y movimiento de camara
    public void MyFixedUpdate()
    {
        CharacterMovement(Time.fixedDeltaTime); //Motor del jugador
        if(!isIA)
        {
            CameraPlacement();  //Comportamiento de la camara
        }
    }

    void SaveLastPosition()
    {
        float distanceToGround = 1.2f;
        Vector3 origin = transform.position + new Vector3(0, 0.1f, 0);
        Vector3 direction = -transform.up;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distanceToGround))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                timerPosition += Time.deltaTime;
                if (timerPosition >= 2.0f)
                {
                    savedPosition = hit.point;
                    timerPosition = 0;
                }
            }
            else
                timerPosition = 0;
        }
        else
            timerPosition = 0;
        Debug.DrawRay(origin, direction * distanceToGround, Color.blue);
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
            if (currentSpeed < 0)
                myRotation = Quaternion.LookRotation(transform.forward);
            activeModel.rotation = Quaternion.Slerp(activeModel.rotation, myRotation, _delta * 10);
            return;
        }


        //Checamos si estamos derrapando
        DriftingBehaviour();
        //Acelerar
        if (aButton && mayhemState == false)
            currentSpeed += _delta * acceleration;
        //Frenar e ir en reversa
        if (bButton && mayhemState == false)
            currentSpeed -= _delta * acceleration * 3;

        //Break
        if (bButton && currentSpeed > 0)
            breaking = true;
        else
            breaking = false;

        activeModelAnim.SetBool("Break", breaking);

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
        if(mayhemState)
        {
            if(currentSpeed > 0)
            {
                currentSpeed -= 2 * Time.deltaTime;
                myStats.handling = 0.5f;
            }
            else
            {
                currentSpeed = 0;
            }
        }

        //Checamos si estamos acelerando o en reversa para dar el control de movimiento adecuado
        if (currentSpeed == 0 && chocado == false)
        {
            if (isIA == false)
            {
                verticalInput = Input.GetAxis(playerPrefix + "Vertical");
            }
            //if (verticalInput < 0)
            //    verticalInput = 0;

            Vector3 myVelocity = steerVector * verticalInput * walkingSpeed;
            myRB.velocity = new Vector3(myVelocity.x, myRB.velocity.y, myVelocity.z);    //Asignamos la velocidad a nuestro RB
            Quaternion pingoRotation = Quaternion.LookRotation(steerVector);
            activeModel.rotation = Quaternion.Slerp(activeModel.rotation, pingoRotation, _delta *5);
        }
        else
        {
            //Asignamos la velocidad a nuestro RB
            Vector3 myVelocity = CrashCheck();
            //if (myRB.velocity.magnitude <= 0.005f)
            //    currentSpeed = 0;
            if(!chocado)
                myRB.velocity = new Vector3(myVelocity.x, myRB.velocity.y, myVelocity.z);
        }

    }

    Vector3 CrashCheck()
    {
        Vector3 myVelocity = transform.forward * currentSpeed * myStats.speed;
        //float distanceToGround = 1.0f;
        //Vector3 origin = transform.position + new Vector3(0, 1.0f, 0);
        //Vector3 direction = activeModel.forward;

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
        activeModelAnim.SetFloat("CurrentSpeed", currentSpeed, 0.05f, Time.deltaTime);
        activeModelAnim.SetFloat("Horizontal", horizontalInput, 0.05f, Time.deltaTime);
        activeModelAnim.SetFloat("Vertical", Input.GetAxis(playerPrefix + "Vertical"), 0.05f, Time.deltaTime);
    }
    //Funcion que detecta cuando el jugador esta derrapando
    void DriftingBehaviour()
    {
        activeModelAnim.SetBool("Drifting", drifting);
        if (drifting && currentSpeed > 5)
        {
            float driftDirection = Mathf.Sign(horizontalInput);
            horizontalInput = driftDirection * 10 * myStats.handling;
            sparks.SetActive(true);
        }
        else
            sparks.SetActive(false);
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
        if (isIA == false)
        {
            verticalInput = Mathf.Abs(Input.GetAxis(_playerPrefix + "Vertical"));
        }
        verticalInput = Mathf.Clamp(verticalInput, 0.5f, 1.0f);
        if (!drifting)
        {
            //Debug.Log(myStats);
            if (isIA == false)
            {
                horizontalInput = (Input.GetAxisRaw(_playerPrefix + "Horizontal") * steerForce * myStats.handling);
            }
        }
  
        aButton = Input.GetButton(_playerPrefix + "A");
        bButton = Input.GetButton(_playerPrefix + "B");

        //Drifting
        if (Input.GetAxisRaw(_playerPrefix + "RTrigger") != 0 || Input.GetAxisRaw(_playerPrefix + "LTrigger") != 0 || Input.GetButton(_playerPrefix + "RTrigger") || Input.GetButton(_playerPrefix + "LTrigger"))
            drifting = true;
        else
            drifting = false;
    }


    void SetNewCameraPos(int _newSpeed)
    {
        if (_newSpeed != cameraDirection)
            cameraRotateAroundPlayer = true;

        cameraDirection = _newSpeed;
    }

    //Manejamos la posicion de la camara
    void CameraPlacement()
    {
        float speed = 1.5f;
        Vector3 desiredPosition = (-cameraDirection * (transform.forward * 4) + (transform.up * 2)) + transform.position;
        if (currentSpeed < 0)
            SetNewCameraPos(-1);
        else
            SetNewCameraPos(1);

        if (currentSpeed > 5)
            speed = 5;
        else
            speed = 1.5f;
        if (cameraRotateAroundPlayer)
        {
            desiredPosition = ((transform.right * 10) + (transform.up * 2)) + transform.position;
            if ((desiredPosition - mainCamera.position).magnitude < 5)
                cameraRotateAroundPlayer = false;
        }
        else
        {
            desiredPosition = (-cameraDirection * (transform.forward * 4) + (transform.up * 2)) + transform.position;
        }

        mainCamera.LookAt(transform.position);
        mainCamera.position = Vector3.Slerp(mainCamera.transform.position, desiredPosition, Time.fixedDeltaTime * speed);
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
                maxForwardSpeed -= Time.fixedDeltaTime * 2;
            }
            if (maxForwardSpeed < maxDefaultSpeed)
                maxForwardSpeed = maxDefaultSpeed;
            boostTimer = 0;
        }
        if (currentSpeed > maxDefaultSpeed)
            yaw.SetActive(true);
        else
            yaw.SetActive(false);
    }

    //Getter de RigidBody
    public Rigidbody GetMyRB()
    {
        return myRB;
    }
}