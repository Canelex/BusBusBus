﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusController : MonoBehaviour
{
    public LineRenderer line;
    public AudioClip busSound;
    public AudioClip explosionSound;

    void Start()
    {
        AudioManager.Instance.PlayMusic(busSound, 0.5F);
    }

    public void UpdatePosition(float percent)
    {
        float distanceOnLine = GetLineLength() * percent;
        float currentLength = 0;
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            float lengthSegment = GetDistance(i);

            if (currentLength + lengthSegment > distanceOnLine) // Find segment on which bus sits
            {
                // How far the bus sits on this segment (0-1)
                float percentOnSegment = (distanceOnLine - currentLength) / lengthSegment;
                transform.position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i + 1), percentOnSegment);

                if (i > 0)
                {
                    // Calculate rotation for bus.
                    Vector2 a = line.GetPosition(i) - line.GetPosition(i - 1); // distance delta of last segment
                    Vector2 b = line.GetPosition(i + 1) - line.GetPosition(i); // distance delta of this segment
                    float thetaA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90; // slope (theta) of last segment
                    float thetaB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg - 90; // slope (theta) of this segment
                    float thetaC = Mathf.Lerp(thetaA, thetaB, percentOnSegment); // Gradual rotation between segments.
                    transform.rotation = Quaternion.Euler(0, 0, thetaC);
                }
                break;
            }

            currentLength += lengthSegment;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Obstacle")
        {
            AudioManager.Instance.Play(explosionSound, 1F); // Boom!
            AudioManager.Instance.PlayMusic(null, 0); // Turn off bus sound

            // Calculate point of explosion (average of all contact points).
            Vector2 avg = Vector2.zero;
            foreach (ContactPoint2D cp in coll.contacts)
            {
                avg += cp.point;
            }
            avg /= coll.contacts.Length;

            FindObjectOfType<LevelController>().BusCrashedAt(avg);
        }
    }

    float GetLineLength()
    {
        float lengthLine = 0;

        for (int i = 0; i < line.positionCount - 1; i++) // Sum the distance between all vertices
        {
            lengthLine += GetDistance(i);
        }

        return lengthLine;
    }

    float GetDistance(int index)
    {
        return Vector3.Distance(line.GetPosition(index), line.GetPosition(index + 1));
    }
}
