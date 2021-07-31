using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : ProjectileThing
{
    [SerializeField] private PlayerAttack thisAttackCollider = null;
    private float thisSpawnAttackColliderTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartThing();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThing();

        thisSpeed -= Time.deltaTime * 5.0f;

        if (thisSpeed <= 0f)
        {
            Destroy(gameObject);
        }

        if (thisSpawnAttackColliderTimer >= 0)
        {
            thisSpawnAttackColliderTimer -= Time.deltaTime;
        }

        else
        {
            PlayerAttack aAttack = Instantiate(thisAttackCollider, transform.position, Quaternion.identity);

            aAttack.thisAttackDir = thisDirection;
            aAttack.thisDamage = 1f;
            aAttack.thisAttackType = 1;

            thisSpawnAttackColliderTimer = 0.4f;
        }
    }
}
