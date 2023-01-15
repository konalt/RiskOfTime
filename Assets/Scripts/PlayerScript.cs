using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float gravity = -0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundCheckSize = 0.01f;
    public float moveSpeed = 10f;
    public float jumpHeight = 8f;
    public float dashDistance = 3f;
    public float dashFriction = -0.1f;
    public float dashHeight = 0.1f;
    public new Camera camera;

    [SerializeField] private float yVelocity = 0f;
    private new SpriteRenderer renderer;
    enum DashState
    {
        None,
        Left,
        Right
    }
    [SerializeField] private DashState dashState = DashState.None;
    [SerializeField] private float dashVelocity = 0f;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void SteppedTranslate(Vector2 vector, int steps = 4)
    {
        for (int i = 0; i < steps; i++)
        {
            Vector2 steppedVector = vector / steps;
            bool touch = Physics2D.OverlapBox((Vector2)groundCheck.position + steppedVector, new Vector2(renderer.bounds.size.x, groundCheckSize / 2), 0, groundMask) != null;
            transform.Translate(steppedVector);
            if (touch && i != 0) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizMove = Input.GetAxisRaw("Horizontal");

        transform.Translate(Time.deltaTime * horizMove * moveSpeed * Vector2.right);

        bool grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(renderer.bounds.size.x, groundCheckSize / 2), 0, groundMask) != null;
        if (!grounded) yVelocity += gravity;
        if (grounded) yVelocity = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jumpHeight;
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                dashState = DashState.Left;
                dashVelocity = dashDistance;
                yVelocity = dashHeight;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                dashState = DashState.Right;
                dashVelocity = dashDistance;
                yVelocity = dashHeight;
            }

        }
        SteppedTranslate(Time.deltaTime * yVelocity * Vector2.up, 16);
        if (dashVelocity > 0)
        {
            dashVelocity += dashFriction;
            if (dashVelocity <= 0) {
                dashVelocity = 0;
                dashState = DashState.None;
            }
        }
        switch (dashState)
        {
            case DashState.Right:
                SteppedTranslate(Time.deltaTime * dashVelocity * Vector2.right, 4);
                break;
            case DashState.Left:
                SteppedTranslate(Time.deltaTime * dashVelocity * Vector2.left, 4);
                break;
        }
        camera.transform.position += (transform.position.x - camera.transform.position.x) * Vector3.right; // dumb unity shitt !!!
    }
}