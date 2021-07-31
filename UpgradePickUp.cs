using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnCollisionEnter(Collision collision)
    {
        Player aPlayer = collision.gameObject.GetComponent<Player>();

        if (aPlayer != null)
        {
            aPlayer.inFlightMode = true;

            UIController.ThisUIController.SetHintText("Suit Upgrade!");

            Destroy(gameObject);
        }
    }
}
