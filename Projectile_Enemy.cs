using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Enemy : ProjectileThing
{
    // Start is called before the first frame update
    void Start()
    {
        StartThing();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThing();
    }

    protected void OnTriggerEnter(Collider other)
    {
        Player aPlayer = other.gameObject.GetComponent<Player>();

        if (aPlayer != null)
        {
            aPlayer.CallOnDamage(3);

            Destroy(gameObject);
        }
    }
}
