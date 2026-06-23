using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsWater;
    bool grounded;
    bool drowning;


    public Volume volume;
    private Vignette vignette;
    private LiftGammaGain lift;
        private Coroutine vignetteRoutine;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lift);
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
        

    }
    

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void IsDrowining(bool drowning)
    {

        if (vignetteRoutine != null)
            StopCoroutine(vignetteRoutine);

        if (drowning)
        {
            lift.active = true;
            vignetteRoutine = StartCoroutine(AnimateVignette(0.6f, 5f));
        }

        else
        {
            lift.active = false;
            vignetteRoutine = StartCoroutine(AnimateVignette(0f, 1f));
        }
    }



    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        Vector3 vel = rb.linearVelocity;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded && !drowning)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // in water
        else if (drowning)
        {
            vel.y = Mathf.Lerp(vel.y, -2f, 5f * Time.fixedDeltaTime); // limite la chute
            rb.linearVelocity = vel;
        }



    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if ((whatIsWater.value & (1 << other.gameObject.layer)) != 0)
        {
            IsDrowining(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((whatIsWater.value & (1 << other.gameObject.layer)) != 0)
        {
            IsDrowining(false);
        }
    }

    private IEnumerator AnimateVignette(float targetIntensity, float duration)
    {
        float startIntensity = vignette.intensity.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            vignette.intensity.value = Mathf.Lerp(
                startIntensity,
                targetIntensity,
                elapsed / duration
            );

            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}