using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int currentPoint;
    public List<GameObject> points;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].transform.position, speed * Time.deltaTime);
        if(transform.position == points[currentPoint].transform.position){
            currentPoint++;
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            if (currentPoint >= points.Count)
            {
                currentPoint = 0;
                transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);

            }
        }
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
