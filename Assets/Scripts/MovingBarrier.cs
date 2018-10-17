using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    private Vector2 original;
    public Vector2 point1;
    public Vector2 point2;
    public float period;
    private float percent;
    private bool whichWay;
    private LevelController levelController;

    void Start()
    {
        original = transform.position;
        levelController = FindObjectOfType<LevelController>();
    }

    void Update()
    {
        if (!levelController.gameOver)
        {
            percent += Time.deltaTime / period;

            if (whichWay)
            {
                transform.position = original + Vector2.Lerp(point1, point2, Mathf.Min(percent, 1));
            }
            else
            {
                transform.position = original + Vector2.Lerp(point2, point1, Mathf.Min(percent, 1));
            }

            if (percent > 1)
            {
                percent = 0;
                whichWay = !whichWay; // Swap the direction.
            }
        }
    }
}
