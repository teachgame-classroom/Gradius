using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Animator titleBlinkAnim;
    private bool isStartButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStartButtonPressed == false)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                titleBlinkAnim.SetBool("fast", true);
                GetComponent<AudioSource>().Play();
                Invoke("LoadScene", 1.5f);
            }
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
