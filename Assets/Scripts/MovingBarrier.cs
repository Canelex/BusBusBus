using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    public float radius;
    public float period;
    public float delay;
    public bool toRight;
    private Vector3 a;
    private Vector3 b;
    private float time;
    private float percent;
    private LevelController levelController;

    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        a = transform.position - transform.right * radius;
        b = transform.position + transform.right * radius;
        transform.position = (toRight ? a : b); // Starting point.
    }

    void Update()
    {
        if (!levelController.IsLevelPlaying())
        {
            return;
        }

        if (time >= delay)
        {
            percent += Time.deltaTime / period;

            if (toRight)
            {
                transform.position = Vector2.Lerp(a, b, Mathf.Min(percent, 1));
            }
            else
            {
                transform.position = Vector2.Lerp(b, a, Mathf.Min(percent, 1));
            }

            if (percent >= 1)
            {
                percent = 0;
                toRight = !toRight; // Swap the direction.
            }
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
