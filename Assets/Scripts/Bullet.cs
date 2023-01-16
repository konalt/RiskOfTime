using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 0f;
    private Vector2 d;

    public float GetAngleArbitrary(Vector2 p, Vector2 v)
    {
        float mx = v.x;
        float my = v.y;
        float gx = p.x;
        float gy = p.y;

        float theta = 0;

        if (mx > gx)
        {
            theta =
                (Mathf.Atan((gy - my) / (gx - mx)) * 180) / Mathf.PI;
        }
        else if (mx < gx)
        {
            theta =
                180 +
                (Mathf.Atan((gy - my) / (gx - mx)) * 180) / Mathf.PI;
        }
        else if (mx == gx)
        {
            if (my > gy)
            {
                theta = 90;
            }
            else
            {
                theta = 270;
            }
        }

        return Mathf.Round(theta);
    }

    public void SetVector(Vector2 p, Vector2 v, float offset = 0f)
    {
        // code stolen from funkymulti
        float mx = v.x;
        float my = v.y;
        float px = p.x;
        float py = p.y;

        float x = mx - px;
        float y = my - py;

        float l = Mathf.Sqrt(x * x + y * y);

        x /= l;
        y /= l;

        d = new Vector2(x, y);
        transform.position += (Vector3)d * offset;
    }

    public void SetSpeed(float sp)
    {
        speed = sp;
    }

    private void Update()
    {
        transform.position += (Vector3)d * speed;
    }
}
