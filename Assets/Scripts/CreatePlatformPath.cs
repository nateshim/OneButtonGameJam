using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlatformPath : MonoBehaviour
{
    public GameObject pathNode;
    public Transform parent;
    public int numNodes = 4;
    private float pathLength;
    private float pathHeight;
    // Start is called before the first frame update
    void Start()
    {
        pathLength = GetComponent<Platform>().offSetX;
        pathHeight = GetComponent<Platform>().offSetY;
        renderPath();
    }

    void renderPath()
    {
        for (int i = 0; i < numNodes; i++)
        {
            float xPos = transform.position.x + (i * (pathLength / (numNodes - 1)));
            float yPos = transform.position.y + (i * (pathHeight / (numNodes - 1)));
            GameObject node = Instantiate(pathNode, new Vector3(xPos, yPos, 0), Quaternion.identity, parent);
            GetComponent<Platform>().pathNodes.Add(node);
        }
    }
}
