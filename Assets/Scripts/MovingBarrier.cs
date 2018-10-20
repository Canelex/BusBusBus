using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    public float left;
    public float right;
    public float period;
    private Vector3 point1;
    private Vector3 point2;
    private float percent;
    private bool whichWay;
    private LevelController levelController;

    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        point1 = transform.position + transform.right * left;
        point2 = transform.position + transform.right * right; 
    }

    void Update()
    {
        if (!levelController.gameOver)
        {
            percent += Time.deltaTime / period;

            if (whichWay)
            {
                transform.position = Vector2.Lerp(point1, point2, Mathf.Min(percent, 1));
            }
            else
            {
                transform.position = Vector2.Lerp(point2, point1, Mathf.Min(percent, 1));
            }

            if (percent > 1)
            {
                percent = 0;
                whichWay = !whichWay; // Swap the direction.
            }
        }
    }
}
