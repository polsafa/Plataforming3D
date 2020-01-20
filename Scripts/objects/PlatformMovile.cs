using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovile : MonoBehaviour
{
    public Transform target;
    // velocidad de movimiento
    public float speed;

    private Vector3 start, end;

    // Use this for initialization
    void Start()
    {
        if (target != null)
        {
            target.parent = null;
            start = transform.position;
            end = target.position;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // movimiento
        if (target != null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);
        }
        // correccion de movimiento
        if (transform.position == target.position)
        {
            // Operador ternario
            // cambia de valor la variable target dependiendo de donde este
            target.position = (target.position == start) ? end : start;
        }
    }
}
