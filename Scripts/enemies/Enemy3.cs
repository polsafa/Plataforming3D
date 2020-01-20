using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public GameObject bullet;
    public bool atak = false;
    private Animator anim;
    public Vector3 player;
    public float timetoshoot = 0f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timetoshoot += Time.deltaTime;
        if(atak == true && timetoshoot > 2)
        {
            anim.SetBool("atak", true);
            atack();
            timetoshoot = 0;
            atak = false;
        }
        else
        {
            anim.SetBool("atak", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            transform.LookAt(other.transform);
            atak = true;
            player = other.transform.position;
        }
      
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "player")
        {
            transform.LookAt(other.transform);
            atak = true;
            player = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {

            atak = false;
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
    private void atack()
    {

        GameObject bulletPlant = Instantiate(bullet,transform.position, Quaternion.identity);
        bulletPlant.GetComponent<BulletPlant>().goToPoint(player);
        Destroy(bulletPlant, 2);

    }

}
