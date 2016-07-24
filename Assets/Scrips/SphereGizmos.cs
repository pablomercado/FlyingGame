using UnityEngine;
using System.Collections;
using System;

public class SphereGizmos : MonoBehaviour {

    public int resolution = 10;

    private void OnDrawGizmosSelected()
    {
        float step = 2f / resolution;
        for (int i = 0; i <= resolution; i++)
        {
            ShowPoint(i * step - 1f, -1f);
            ShowPoint(i * step - 1f, 1f);
        }
        for (int i = 1; i < resolution; i++)
        {
            ShowPoint(-1, i * step - 1f);
            ShowPoint(1, i * step - 1f);
        }
    }

    private void ShowPoint(float x, float y)
    {
        Vector2 square = new Vector2(x, y);
        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1f - square.y * square.y * 0.5f);
        circle.y = square.y * Mathf.Sqrt(1f - square.x * square.x * 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(square, 0.025f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(circle, 0.025f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(square, circle);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(circle, Vector2.zero);



    }
}
