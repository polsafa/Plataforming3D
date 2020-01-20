using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booss : MonoBehaviour
{
    public int currentPoint;
    public bool startBattle;
    public List<GameObject> spawnpoints;
    public List<GameObject> points;
    public float speed;
    public int life = 100;
    public GameObject bullet;
    public bool state1, state2, state3;
    public Vector3 player;
    public float timetoshoot;
    public GameObject plants;
    public bool onetime = false;
    public Text lifetext;
    public GameObject canvasboos;
    public GameObject playercito;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateBoss();
        if (startBattle == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].transform.position, speed * Time.deltaTime);
            if (transform.position == points[currentPoint].transform.position)
            {
                currentPoint++;
                
                if (currentPoint >= points.Count)
                {
                    currentPoint = 0;
                }
            }

            if (life >= 60)
            {
                state1 = true;
            }
            if(life <= 40 && state1 == true)
            {
                state1 = false;
                state2 = true;
            }
            if(life <= 24 && onetime == false)
            {
                state2 = false;
                state3 = true;   
            }
            if(state1 == true)
            {
                GenerateProjectils(5f);
            }
            if(state2 == true)
            {
                GenerateProjectils(2f);
            }
            if(state3 == true )
            {
                GenerateProjectils(1f);
                if(onetime == false)
                {
                    spawnMonsters();
                }
                onetime = true;
            }
           
            
        }
        if (life <= 0)
        {
            playercito.GetComponent<PlayerKinematic>().openEndCanvas();
            playercito.GetComponent<PlayerKinematic>().end = true;
            startBattle = false;
            canvasboos.SetActive(false);
            Destroy(gameObject);     
        }

    }
    public void GenerateProjectils(float cd)
    {
        timetoshoot += Time.deltaTime;
        if(timetoshoot >= cd)
        {
            GameObject BulletBoss = Instantiate(bullet, transform.position, Quaternion.identity);
            BulletBoss.GetComponent<BulletBoss>().goToPoint(player);
            Destroy(BulletBoss, 3);
            timetoshoot = 0;
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "missile")
        {

            Destroy(collision.gameObject);
            life = life - 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            playercito = other.gameObject;
            canvasboos.SetActive(true);
            transform.LookAt(other.transform);
            startBattle = true;
            player = other.transform.position;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "player")
        {
            canvasboos.SetActive(true);
            transform.LookAt(other.transform);
            startBattle = true;
            player = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            canvasboos.SetActive(false);
            life = 100;
            state1 = false;
            state2 = false;
            state3 = false;
            onetime = false;
            startBattle = false;
        }
    }

    private void spawnMonsters()
    {
        for(int i = 0; i < spawnpoints.Count; i++)
        {
            GameObject invocacion = Instantiate(plants, spawnpoints[i].transform.position, Quaternion.identity);
            invocacion.transform.parent = GameObject.Find("MAPA").transform;


        }
    }
   private void updateBoss()
    {
        lifetext.text = life.ToString() + "%";

    }

}
