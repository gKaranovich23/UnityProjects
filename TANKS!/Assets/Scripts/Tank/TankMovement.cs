using UnityEngine;

public class TankMovement : MonoBehaviour
{
    //m_ denotes class/global variables to be used anywhere within script
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         

    //called when the game is opened
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    //called when script is turned on
    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false; //kinematic means no forces will be applied when on. Want physics but don't want the effects of physic forces.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    //called when script is turned on
    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }

    //occurs before any updates occur
    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    
    //runs every frame. Is used to store player's input only and variable initialization which can also be in Awake()
    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) //if tank is not moving
        {
            if(m_MovementAudio.clip == m_EngineDriving)//if the audio source is engine driving set to idle
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); //changing the pitch of the audio
                m_MovementAudio.Play(); //when you change clips on audio source through code you must call the play function after 
            }
        }
        else //else the tank is moving
        {
            if (m_MovementAudio.clip == m_EngineIdling)//if the audio source is engine idling set to driving
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); 
                m_MovementAudio.Play(); 
            }
        }
    }

    //runs every physics step. Involve events that deal with physics
    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        //Quaternion stores rotation
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f); //changes format of turn into Quaternion

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}