using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;

    public float velocity;
    public float angle;
    private int resolution = 10;

    float g; //force of gravity on the y axis
    float radianAngle;

    void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y * 3);
    }
    // Start is called before the first frame update
    void Update()
    {
        Vector2 jumpForce = GetComponent<CharacterController2D>().GetVelocity();
        angle = Mathf.Atan2(jumpForce.y, jumpForce.x) * Mathf.Rad2Deg;
        velocity = jumpForce.magnitude / g;   
    }

    //populating the LineRenderer with the appropriate settings
    public void RenderArc()
    {
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(CalculateArcArray());
    }

    //create an array of Vector3 positions for arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }
    
    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x =  (t * maxDistance);
        float y =  (x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle))));
        if (!GetComponent<CharacterController2D>().m_FacingRight) {
            return new Vector3(-x + transform.position.x, y + transform.position.y);
        } else {
            return new Vector3(x + transform.position.x, y + transform.position.y);   
        }
    }
}
