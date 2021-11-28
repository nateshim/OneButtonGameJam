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
    private Color teal = new Color(0, 0.5019608f, 0.5019608f);
    public float speed = 1.0f;

    public List<GameObject> pathNodes = new List<GameObject>();
    

    void Awake()
    {
        leftPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rightPosition = new Vector3(transform.position.x + offSetX, transform.position.y + offSetY, 0);
    }

    void Update()
    {
        CheckColor();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 14)
        {
            other.transform.parent.GetComponent<Rigidbody2D>().isKinematic=true;
            other.transform.parent.parent = transform;
        }
    }

    void CheckColor()
    {
        Color currColor = GetComponent<SpriteRenderer>().color;
        Color rgbColor = new Color(currColor.r, currColor.g, currColor.b);
        if (GameObject.Find("Player").GetComponent<SpriteRenderer>().color != teal &&  rgbColor == teal)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(currColor.r, currColor.g, currColor.b, .2f);
            for (int i = 0; i < pathNodes.Count; i++)
            {
                Color pathNodeColor = pathNodes[i].GetComponent<SpriteRenderer>().color;
                pathNodes[i].GetComponent<SpriteRenderer>().color = new Color(pathNodeColor.r, pathNodeColor.g, pathNodeColor.b, .2f);
            }

        } else if (GameObject.Find("Player").GetComponent<SpriteRenderer>().color == teal && teal == rgbColor)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = new Color(currColor.r, currColor.g, currColor.b, 1f);
            for (int i = 0; i < pathNodes.Count; i++)
            {
                Color pathNodeColor = pathNodes[i].GetComponent<SpriteRenderer>().color;
                pathNodes[i].GetComponent<SpriteRenderer>().color = new Color(pathNodeColor.r, pathNodeColor.g, pathNodeColor.b, 1f);
            }
        }
    }
}
