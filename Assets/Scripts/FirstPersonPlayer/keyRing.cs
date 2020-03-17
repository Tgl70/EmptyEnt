using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyRing : MonoBehaviour
{
    static int keys;

    // Start is called before the first frame update
    void Start()
    {
        keys = 0;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "key")
        {
            keys++;
            Destroy(collision.gameObject);
        }
    }
}
