using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlyMode : MonoBehaviour
{
    [SerializeField] private GameObject thisBackWall = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void OnTriggerEnter(Collider other)
    {
        Player aPlayer = other.gameObject.GetComponent<Player>();

        if (aPlayer != null)
        {
            aPlayer.inFlightMode = false;

            thisBackWall.SetActive(true);
        }
    }
}
