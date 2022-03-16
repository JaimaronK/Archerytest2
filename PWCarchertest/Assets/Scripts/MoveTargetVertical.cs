using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetVertical : MonoBehaviour
{
    private float speed = 5f; //Speed the targets move at
    private float xmin = -2.5f; //X range min
    private float xmax = 9.5f; // X range max
    private float ymin = -2.5f; // Y Range min
    private float ymax = 3.8f; // y Range max
    private int phase = 1; // Quadrant check

    // Start is called before the first frame update
    void Start()
    {
       // speed = Random.Range(1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        direction();
    }

    void direction() 
    {
        if (transform.position.x <= xmin && transform.position.y >= ymax)
        {
            phase = 1; //Quadrant 1 Top Left;
        }
        if (transform.position.x <= xmin && transform.position.y <= ymin)
        {
            phase = 2; //Quadrant 2 Bottom Left
        }
        if (transform.position.x >= xmax && transform.position.y <= ymin)
        {
            phase = 3; //Quadrant 3 Bottom Right
        }
        if (transform.position.x >= xmax && transform.position.y >= ymax)
        {
            phase = 4; // Quadrant 4 Top right;
        }

        if (phase == 1) 
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime); // target move in the direction towards the next quadrant
        }
        else if (phase == 2) 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (phase == 3) 
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (phase == 4) 
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
