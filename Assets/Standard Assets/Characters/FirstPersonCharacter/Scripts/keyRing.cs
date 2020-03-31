using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class keyRing : MonoBehaviour
{
    public static int keys;
    public TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {
        keys = 0;
        updateKeyCounter();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "key")
        {
            keys++;
            Destroy(collision.gameObject);
            updateKeyCounter();
        }
    }

    private void updateKeyCounter()
    {
        counterText.SetText("X " + keys);
    }
}
