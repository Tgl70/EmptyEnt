using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        // Double jump variables
        private bool jump1;
        private bool jump2;

        private bool wasOnVine;
        private int timeCount;
        private Vector3 vineVelocity;
        private bool jumpDampener;

        private bool isGrounded; // is on a slope or not
        public float slideFriction = 0.3f; // ajusting the friction of the slope
        private Vector3 hitNormal; //orientation of the slope.

        // Vine controls
        bool onVine;
        public GameObject vineBottom;

        private float slideSpeed = 1;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            jump1 = false;
            jump2 = false;
            onVine = false;
            wasOnVine = false;
            jumpDampener = true;
            timeCount = 0;
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed

            if (m_CharacterController.isGrounded)
            {
                jump1 = true;
                jump2 = true;
                m_MoveDir.y = -m_StickToGroundForce;
                jumpDampener = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log(this.transform.position);
                Debug.Log(vineBottom.transform.position);
                Debug.Log(-this.transform.position + vineBottom.transform.position);
            }

            if (onVine && Input.GetKeyDown(KeyCode.Q))
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                onVine = false;
                transform.parent = null;
                wasOnVine = true;
                timeCount = 10;
                vineVelocity = vineBottom.GetComponent<Rigidbody>().velocity;

                Vector3 move = vineBottom.transform.position - this.transform.position;
                m_CharacterController.SimpleMove(move);
            }

            if (onVine)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                vineBottom.GetComponent<Rigidbody>().AddForce(transform.forward * vertical * 4, ForceMode.Acceleration);
                vineBottom.GetComponent<Rigidbody>().AddForce(transform.right * horizontal * 1, ForceMode.Acceleration);
                this.transform.position = vineBottom.transform.position + new Vector3(0, -1, 0);
            }

            if (Input.GetButtonDown("Jump") && !onVine)
            {
                m_Jump = true;
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            Vector3 desiredMove = new Vector3(0, 0, 0);
            if (!onVine)
            {
                // always move along the camera forward as it is the direction that it being aimed at
                desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;
            }  

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;

            if (m_Jump && !onVine)
            {
                jumpDampener = true;
                if (jump1)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    jump1 = false;
                    m_Jumping = true;
                }
                else if (jump2)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    jump2 = false;
                    m_Jumping = true;
                }
                m_Jump = false;
            }

            if (!m_CharacterController.isGrounded)
            {
                if (jumpDampener)
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime * 9000000;
                }
            }

            if (!isGrounded)
            {
                m_MoveDir.x += ((1f - hitNormal.y) * hitNormal.x) * slideSpeed;
                m_MoveDir.z += ((1f - hitNormal.y) * hitNormal.z) * slideSpeed;
            }

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            isGrounded = Vector3.Angle(Vector3.up, hitNormal) <= m_CharacterController.slopeLimit;

            if (timeCount > 0)
            {
                jumpDampener = true;
                Vector3 move = (new Vector3(0.0f, 7f, 0.0f) + (vineVelocity)) * 20f * Time.fixedDeltaTime;
                m_CharacterController.Move(move);
                Debug.Log(move);
                wasOnVine = false;
                timeCount--;
            }

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y;
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y;
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            hitNormal = hit.normal;

            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (hit.gameObject.tag == "vine" && Input.GetKey(KeyCode.E))
            {
                onVine = true;
                transform.parent = hit.gameObject.transform;
                vineBottom = hit.gameObject;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
