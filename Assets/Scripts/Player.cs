using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private Color teal = new Color(0, 128, 128);

    private Dictionary<string, int> colors = new Dictionary<string, int>()
    {
        {"Red", 10},
        {"Teal", 11}
    };
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == colors["Red"] && GetComponent<SpriteRenderer>().color == teal) {
            GetComponent<SceneManagerScript>().LoadNextScene();
        }  
        if (other.gameObject.layer == colors["Red"]) {
            GetComponent<SpriteRenderer>().color = Color.red;
        } else if (other.gameObject.layer == colors["Teal"])
        {
            GetComponent<SpriteRenderer>().color = teal;
        } 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
