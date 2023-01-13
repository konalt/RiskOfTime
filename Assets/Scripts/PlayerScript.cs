using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float gravity = -0.1f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundCheckSize = 0.01f;
    public float moveSpeed = 10f;
    public new Camera camera;

    private float yVelocity = 0f;
    private new SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizMove = Input.GetAxisRaw("Horizontal");

        transform.Translate(Time.fixedDeltaTime * horizMove * moveSpeed * Vector2.right);

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundMask) != null;
        yVelocity += gravity;
        if (grounded) yVelocity = 0;
        if (!grounded) transform.Translate(Time.fixedDeltaTime * yVelocity * Vector2.up);

        camera.transform.position += (transform.position.x - camera.transform.position.x) * Vector3.right;
    }
}
