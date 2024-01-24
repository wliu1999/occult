using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadMainGameplay()
    {
        SceneManager.LoadScene(1);
    }

    public void loadScenarioTesting()
    {
        SceneManager.LoadScene(2);
    }

    public void loadNewUI()
    {
        SceneManager.LoadScene(3);
    }
}
