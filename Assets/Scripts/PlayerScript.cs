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
    public new Camera camera;

    [SerializeField] private float yVelocity = 0f;
    private new SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void SteppedTranslate(Vector2 vector, int steps = 4)
    {
        for (int i = 0; i < steps; i++)
        {
            Vector2 steppedVector = vector / steps;
            bool grounded = Physics2D.OverlapBox((Vector2)groundCheck.position + steppedVector, new Vector2(renderer.bounds.size.x, groundCheckSize / 2), 0, groundMask) != null;
            transform.Translate(steppedVector);
            if (grounded && i != 0) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizMove = Input.GetAxisRaw("Horizontal");

        transform.Translate(Time.deltaTime * horizMove * moveSpeed * Vector2.right);

        bool grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(renderer.bounds.size.x, groundCheckSize / 2), 0, groundMask) != null;
        yVelocity += gravity;
        if (grounded) yVelocity = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jumpHeight;
        }
        SteppedTranslate(Time.deltaTime * yVelocity * Vector2.up, 4);

        camera.transform.position += (transform.position.x - camera.transform.position.x) * Vector3.right; // dumb unity shitt !!!
    }
}
