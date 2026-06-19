using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 7f;
    [SerializeField] float sideSpeed = 7.5f;

    Rigidbody rb;

    float currentSideInput;
    float sideVelocity;
    float moveInput;

    float originalForwardSpeed;
    float speedMultiplier = 1f;
    float controlMultiplier = 1f;
    float effectEndsAt;
    float pushVelocity;
    float pushEndsAt;
    bool canMove = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalForwardSpeed = forwardSpeed;
    }

    public void ActivateSpeedBoost(float boostSpeed, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boostSpeed, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostSpeed, float duration)
    {
        forwardSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        forwardSpeed = originalForwardSpeed;
    }

    public void ApplySlow(float speedAmount, float controlAmount, float duration)
    {
        speedMultiplier = speedAmount;
        controlMultiplier = controlAmount;
        effectEndsAt = Time.time + duration;
    }

    public void ApplyPush(float amount, float duration)
    {
        pushVelocity = amount;
        pushEndsAt = Time.time + duration;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!canMove && rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (Time.time > effectEndsAt)
        {
            speedMultiplier = 1f;
            controlMultiplier = 1f;
        }

        if (Time.time > pushEndsAt)
        {
            pushVelocity = 0f;
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        currentSideInput = Mathf.SmoothDamp(
            currentSideInput,
            moveInput,
            ref sideVelocity,
            0.08f
        );
        
        Vector3 movement = new Vector3(
            currentSideInput * sideSpeed * controlMultiplier + pushVelocity,
            rb.linearVelocity.y,
            forwardSpeed * speedMultiplier
        );

        rb.linearVelocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal -= 1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal += 1f;
        }

        moveInput = Mathf.Clamp(horizontal, -1f, 1f);
    }
}
