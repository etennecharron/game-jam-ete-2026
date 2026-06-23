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
    public float speedMult;
    public float groundDrag;
    private float currentMaxSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool runWhileJumping;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

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
        currentMaxSpeed = moveSpeed;

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
        {
            if(Input.GetKey(sprintKey)) rb.AddForce(moveDirection.normalized * moveSpeed * speedMult * 10f, ForceMode.Force);
            else rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in water
        else if (drowning)
        {
            vel.y = Mathf.Lerp(vel.y, -2f, 5f * Time.fixedDeltaTime);
            rb.linearVelocity = vel;
        }



    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (grounded)
        {
            currentMaxSpeed = Input.GetKey(sprintKey)
                ? moveSpeed * speedMult
                : moveSpeed;
        }

        if (flatVel.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentMaxSpeed;
            rb.linearVelocity = new Vector3(
                limitedVel.x,
                rb.linearVelocity.y,
                limitedVel.z
            );
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