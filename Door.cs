using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator thisAnimator = null;
    private bool isDoorOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        thisAnimator.SetBool("isOpened", isDoorOpened);
    }

    public void OpenDoor()
    {
        isDoorOpened = true;
    }
}
