using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    private static GlobalController thisGlobalController;

    public static GlobalController ThisGlobalController
    {
        get
        {
            return thisGlobalController;
        }
    }

    private float thisTimeStopDuration = 0f;
    // Start is called before the first frame update
    void Start()
    {
        thisGlobalController = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisTimeStopDuration > 0)
        {
            Time.timeScale = 0;

            thisTimeStopDuration -= Time.unscaledDeltaTime;
        }

        else
        {
            Time.timeScale = 1;
        }

        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void StopTimeinDuration(float aDuration)
    {
        thisTimeStopDuration = aDuration;
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene(2);
    }
}
