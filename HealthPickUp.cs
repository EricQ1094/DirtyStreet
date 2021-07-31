using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
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
            aPlayer.CallOnHeal(5f);

            Destroy(gameObject);
        }
    }
}
