using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesDetector : MonoBehaviour
{
    const int numViewDirection = 20;
    public static readonly Vector2[] directions;

    static SpritesDetector()
    {
        directions = new Vector2[SpritesDetector.numViewDirection];

        float goldenRation = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRation;

        for (int i = 0; i < numViewDirection; i++)
        {
            float t = (float)i / numViewDirection;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            directions[i] = new Vector2(x, y);
        }
    }

}
