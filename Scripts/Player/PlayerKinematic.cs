using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKinematic : MonoBehaviour
{
    public float speed, jumpspeed, speedRotate;
    public CharacterController control;
    public Vector3 moveDir = Vector3.zero;
    public float gravity;
    public int jumpcount;
    public bool doublejump = true;
    public Animator anim;
    public GameObject modelado;

    public float distanceAtack;
    public float rateAtack, timeAtack;
    private bool activeatack;
    private List<Collider> enemiesOverLap = new List<Collider>();
    private bool rot = false;

    public GameObject missile;

    public float timerotate = 0f;
    public float timetoshoot = 0f;

    private RaycastHit hitpies;

    public Text lifetext;

    public float direction = 0;

    public int maxlife = 100;
    public int life;
    public GameObject spawnbullet;
    private Vector3 currentSavePos;

    public GameObject chechcheck;

    public bool freeze = false;
    public bool flying = false;

    public GameObject canvasend;
    public GameObject canvastutorial;

    public bool end;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("inittutorial", 2);
        Invoke("disinittutorial", 5);
        currentSavePos = transform.position;
        life = maxlife;
        anim = GetComponent<Animator>();
        control = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flying == false)
        {
            anim.SetBool("fly", false);
        }
        if (flying == true)
        {
            anim.SetBool("fly", true);
            moveDir = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDir.y = jumpspeed;

            }
        }

        correctControls();
        timetoshoot += Time.deltaTime;
        atackSystem();
        updatecanvas();
        if (end == false)
        {
            transform.Rotate(Vector3.up * speedRotate * Time.deltaTime * Input.GetAxis("Mouse X"));
        }
        if (control.isGrounded)
        {
            freeze = true;
            moveDir = new Vector3(0, 0, 0);
            anim.SetBool("jump", false);
            anim.SetBool("doublejump", false);
            anim.SetBool("area", false);
            anim.SetBool("caida", true);

            jumpcount = 0;

            if (freeze == true)
            {
                moveDir = new Vector3(0, 0, Input.GetAxis("Vertical"));
                moveDir = transform.TransformDirection(moveDir);
                moveDir *= speed;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveDir.y = jumpspeed;
                    anim.SetBool("jump", true);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    rot = true;
                    anim.SetBool("area", true);
                }

                if (Input.GetKeyDown(KeyCode.E) && timetoshoot > 5)
                {
                    shoot();
                    anim.SetBool("shoot", true);
                }
                else
                {
                    anim.SetBool("shoot", false);
                }



                if (rot == true)
                {
                    timerotate += Time.deltaTime;
                    modelado.transform.Rotate(new Vector3(0f, 1080f, 0f) * Time.deltaTime);
                    if (timerotate >= 1f)
                    {
                        rot = false;
                        modelado.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        timerotate = 0;
                    }
                }
            }
            if (moveDir.x > 0f || moveDir.z > 0f || moveDir.z < 0f || moveDir.x < 0f)
            {
                anim.SetBool("run", true);
            }
            else if (moveDir.x == 0f && moveDir.z == 0f)
            {
                anim.SetBool("run", false);
            }

        }
        else
        {

            moveDir = new Vector3(Input.GetAxis("Horizontal") * speed, moveDir.y, Input.GetAxis("Vertical") * speed);
            moveDir = transform.TransformDirection(moveDir);

            if (Input.GetKeyDown(KeyCode.Space) && jumpcount < 1 && doublejump == true)
            {
                anim.SetBool("jump", false);
                anim.SetBool("doublejump", true);
                moveDir.y = jumpspeed;
                jumpcount++;
            }
        }
        Vector3 newdir = moveDir;
        if (life < 0)
        {
            respawn();
        }
        if (transform.rotation.y > 0)
        {
            direction = 1;
        }
        else if (transform.rotation.y < 0)
        {
            direction = -1;
        }
        if (flying == false)
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
        control.Move(moveDir * Time.deltaTime);
    }

    private void atackSystem()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitpies))
        {
            if (hitpies.collider.tag == "enemy")
            {
                Destroy(hitpies.collider.gameObject);
                moveDir.y = jumpspeed;
            }
        }
        if (activeatack == true)
        {
            Invoke("damageenemy", 0.5f);

        }
        else
        {
            rateAtack += Time.deltaTime;
            if (rateAtack >= 1f && Input.GetKeyDown(KeyCode.Q))
            {

                rateAtack = 0;
                activeatack = true;
                Invoke("stopatack", 0.7f);
            }
        }
    }
    private void stopatack()
    {
        activeatack = false;
        enemiesOverLap.Clear();
    }
    private void damageenemy()
    {
        enemiesOverLap = new List<Collider>(Physics.OverlapSphere(transform.position, distanceAtack));
        for (int i = 0; i < enemiesOverLap.Count; i++)
        {
            if (enemiesOverLap[i].tag == "enemy")
            {
                Destroy(enemiesOverLap[i].gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            life = life - 10;
        }
        if (other.tag == "bulletboss")
        {

            life = life - 30;
            playerretroceso(10);
            Destroy(other.gameObject);
        }

        if (other.tag == "bullet")
        {
            life = life - 15;
            playerretroceso(10);
            Destroy(other.gameObject);
        }
        if (other.tag == "tunel")
        {
            flying = true;
        }
        if (other.gameObject.tag == "check")
        {
            checkpoint(other.gameObject);
        }
        if (other.gameObject.tag == "platform")
        {
            transform.parent = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "platform")
        {
            transform.parent = other.transform;
        }
        if (other.tag == "tunel")
        {
            flying = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
        if (other.tag == "tunel")
        {
            flying = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plant")
        {
            life = life - 10;
        }

    }

    public void playerretroceso(float push)
    {
        anim.SetBool("jump", true);
        moveDir.y = push;
        moveDir.x = push;

        control.Move(moveDir * Time.deltaTime);
    }
    public void checkpoint(GameObject saver)
    {
        chechcheck.SetActive(true);
        currentSavePos = saver.transform.position;
        Invoke("chaumensaje", 2);
    }
    public void chaumensaje()
    {
        chechcheck.SetActive(false);
    }
    public void respawn()
    {
        
        life = 100;
        transform.position = currentSavePos;
    }
    public void shoot()
    {
        GameObject bullet = Instantiate(missile, new Vector3(spawnbullet.transform.position.x, spawnbullet.transform.position.y, spawnbullet.transform.position.z), Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 20));
        bullet.transform.LookAt(transform.position);
        bullet.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        Destroy(bullet, 4);
    }
    public void updatecanvas()
    {
        lifetext.text = life.ToString() + " %";
    }

    public void correctControls()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            modelado.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            modelado.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void openEndCanvas()
    {
        canvasend.SetActive(true);
    }
    public void inittutorial()
    {
        canvastutorial.SetActive(true);
    }
    public void disinittutorial()
    {
        canvastutorial.SetActive(false);
    }

}
