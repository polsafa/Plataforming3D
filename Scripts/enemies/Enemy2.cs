using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public CharacterController control;
    private  Vector3 movedir= Vector3.zero;
    public float gravity, speed, jumpspeed;

    public List<GameObject> points;
    public int currentpoint = 0;

    private float timejump;
    public float distancePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(points[currentpoint].transform.position);

        if (control.isGrounded)
        {
            timejump += Time.deltaTime;
            if(timejump >= 0.5f)
            {
                timejump = 0;
                movedir.y = jumpspeed;
            }

            distancePoint = Vector3.Distance(transform.position, points[currentpoint].transform.position);

            if (distancePoint < 2)
            {
                
                currentpoint++;
                if (currentpoint >= points.Count)
                {
                    currentpoint = 0;
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentpoint].transform.position, speed * Time.deltaTime);

        }
        movedir.y -= gravity * Time.deltaTime;
        control.Move(movedir * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "missile")
        {

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
