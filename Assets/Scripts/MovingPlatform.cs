using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private float speed;

    public Rigidbody2D rb { get; private set;}

    private int count;
    private bool goBack;

    private float current;
    private float dir;
    private float step;

    void Start()
    {
        count = 0;
        goBack = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(count == (points.Count - 1))
        {
            goBack = true;
        }

        else if(count == 0)
        {
            goBack = false;
        }

        if(this.transform.position == points[count].position)
        {
            if(!goBack)
            {
                count++;
            }
            else{
                count--;
            }
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, points[count].position, speed * Time.deltaTime);
        //rb.MovePosition(Vector2.MoveTowards(rb.position, points[count].position, speed * Time.deltaTime));
    }
}