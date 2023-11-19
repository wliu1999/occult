using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visionSheet : MonoBehaviour
{
    public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            Debug.Log("isPaused");
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("not paused");
            isPaused = false;
        }
    }
}
