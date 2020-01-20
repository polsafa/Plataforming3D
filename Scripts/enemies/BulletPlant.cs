using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlant : MonoBehaviour
{
    public bool active;
    public Vector3 pointo;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointo, speed * Time.deltaTime);
        }
        if(transform.position == pointo)
        {
            Destroy(gameObject);
        }
    }

    public void goToPoint(Vector3 vec)
    {
        active = true;
        pointo = vec;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "platform")
        {
            Destroy(gameObject);
        }

    }
}
