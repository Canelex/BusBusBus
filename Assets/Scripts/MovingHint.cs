using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHint : MonoBehaviour
{
    public Vector2 distance;
    public float time;
    public int frames;
    private int currentFrame;
    private Vector2 reference;

    void Start()
    {
        reference = transform.position;
        InvokeRepeating("AnimateFrame", 0F, time / frames);
    }

    void AnimateFrame()
    {
        float percent = (float)currentFrame / frames;
        transform.position = reference + distance * percent;
        currentFrame++;
        if (currentFrame > frames) currentFrame = 0;
    }
}
