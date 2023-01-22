using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float moveSmoothing = 0.1f;
    public float dashForce = 2000f;
    public float dashJump = 300f;
    public float jumpForce = 300f;
    public float jumpHoldFactor = 0.2f;
    public float wallSlideSpeed = 10f;
    public float wallJumpHorizontalFactor = 3f;
    [Header("Checks")]
    public LayerMask groundMask;
    public float groundWidth = 1f;
    public Transform floorCheck;
    public Transform ceilingCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    [Header("Weaponry")]
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletOffset;

    public bool floor { get; private set; }
    public bool ceiling { get; private set; }
    public bool leftWall { get; private set; }
    public bool rightWall { get; private set; }

    private Rigidbody2D rb;
    private new SpriteRenderer renderer;
    private new Camera camera;
    private Vector3 currentVelocity = Vector3.zero;
    private bool disableGravityWhenHoldingSpacebar = false;
    private float grav;
    private new AnimationComponent animation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<AnimationComponent>();
        camera = Camera.main;
        grav = rb.gravityScale;
    }

    void DrawRect(Rect rect, bool green)
    {
        Color color = green ? Color.green : Color.red;
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y), color);
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x, rect.y + rect.height), color);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x + rect.width, rect.y), color);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), color);
    }

    bool CheckBox(Vector2 pos, Vector2 size)
    {
        bool detect = Physics2D.OverlapBox(pos, size, 0, groundMask) != null;
        Rect rect = Rect.zero;
        rect.x = pos.x - size.x / 2;
        rect.y = pos.y - size.y / 2;
        rect.width = size.x;
        rect.height = size.y;
        DrawRect(rect, detect);
        return detect;
    }

    private void Update()
    {
        floor = CheckBox(floorCheck.position + groundWidth / 4 * Vector3.down, new Vector2(renderer.bounds.size.x * 0.95f, groundWidth / 2));
        ceiling = CheckBox(ceilingCheck.position + groundWidth / 4 * Vector3.up, new Vector2(renderer.bounds.size.x * 0.95f, groundWidth / 2));
        leftWall = CheckBox(leftWallCheck.position + groundWidth / 4 * Vector3.left, new Vector2(groundWidth / 2, renderer.bounds.size.y * 0.95f));
        rightWall = CheckBox(rightWallCheck.position + groundWidth / 4 * Vector3.right, new Vector2(groundWidth / 2, renderer.bounds.size.y * 0.95f));

        float horiz = Input.GetAxisRaw("Horizontal");

        if (disableGravityWhenHoldingSpacebar && Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = 0;
        } else
        {
            rb.gravityScale = grav;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            disableGravityWhenHoldingSpacebar = false;
            CancelInvoke(nameof(ReEnableGravityAfterSpacebar));
        }

        Vector2 move = Vector3.SmoothDamp(rb.velocity, new Vector2(horiz * 10f, rb.velocity.y), ref currentVelocity, moveSmoothing);

        rb.velocity = move;

        if (horiz < 0 && !floor && leftWall ||
            horiz > 0 && !floor && rightWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            rb.AddForce(new Vector2(dashForce * Input.GetAxisRaw("Horizontal"), dashJump));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (floor ||
                leftWall ||
                rightWall)
            {
                disableGravityWhenHoldingSpacebar = true;
                Invoke(nameof(ReEnableGravityAfterSpacebar), jumpHoldFactor);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(jumpForce * Vector2.up + 
                    ((leftWall || rightWall) ? jumpForce * wallJumpHorizontalFactor : 0) * Vector2.right);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mp.y = transform.position.y;
            Shoot(Camera.main.WorldToScreenPoint(mp));
            animation.StartAnimation("shoot", false);
        }
        if (Input.mousePosition.x > Screen.width / 2 && !renderer.flipX)
        {
            renderer.flipX = true;
        }
        if (Input.mousePosition.x < Screen.width / 2 && renderer.flipX)
        {
            renderer.flipX = false;
        }
    }

    void Shoot(Vector2 mousePos)
    {
        GameObject bulObject = Instantiate(bullet);
        Bullet bulControl = bulObject.AddComponent<Bullet>();
        bulControl.transform.position = transform.position;
        bulControl.SetVector(Camera.main.WorldToScreenPoint(transform.position), mousePos, bulletOffset);
        bulControl.SetSpeed(bulletSpeed);
        bulControl.transform.eulerAngles = new Vector3(
            bulControl.transform.eulerAngles.x,
            bulControl.transform.eulerAngles.y,
            bulControl.GetAngleArbitrary(Camera.main.WorldToScreenPoint(transform.position), mousePos)
        );
    }

    void ReEnableGravityAfterSpacebar()
    {
        disableGravityWhenHoldingSpacebar = false;
    }

    private void LateUpdate()
    {
        camera.transform.position += (transform.position.x - camera.transform.position.x) * Vector3.right; // dumb unity shitt !!!
        camera.transform.position += (transform.position.y - camera.transform.position.y) * Vector3.up; // dumb unity shitt !!!
    }
}
