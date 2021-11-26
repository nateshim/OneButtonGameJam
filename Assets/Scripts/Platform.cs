using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float offSetX = 4.0f;
    public float offSetY = 4.0f;
    
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private bool isRight = false;
    public float speed = 1.0f;
    

    void Awake()
    {
        leftPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rightPosition = new Vector3(transform.position.x + offSetX, transform.position.y + offSetY, 0);
    }

    void Update()
    {
        if (isRight) {
            transform.position = Vector3.MoveTowards(transform.position, leftPosition, speed * Time.deltaTime);
            if (transform.position == leftPosition) {
                isRight = false;
            }
        } else {
            transform.position = Vector3.MoveTowards(transform.position, rightPosition, speed * Time.deltaTime);
            if (transform.position == rightPosition) {
                isRight = true;
            }
        }
    }
}
