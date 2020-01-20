using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            other.gameObject.GetComponent<PlayerKinematic>().respawn();
        }
    }
}
