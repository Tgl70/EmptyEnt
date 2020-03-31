using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int sceneCount = SceneManager.sceneCount;
        Debug.Log(sceneCount);
        for (int i = 0; i<sceneCount; i++)
        {
            Debug.Log(SceneManager.GetSceneAt(i).name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "player")
        {
            Scene sceneToLoad = SceneManager.GetSceneByName("LavaScene");
            SceneManager.LoadScene(sceneToLoad.name, LoadSceneMode.Additive);
            SceneManager.MoveGameObjectToScene(GameObject.Find("UI"), sceneToLoad);
        }
        

    }
}
