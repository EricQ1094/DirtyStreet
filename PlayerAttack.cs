using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Vector3 thisAttackDir = Vector3.zero;
    public float thisDamage = 0f;
    public int thisAttackType = 0;
    //[SerializeField] private List<EnemyThing> thisHitEnemies = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        Crate aCrate = other.gameObject.GetComponent<Crate>();

        if (aCrate != null)
        {
            aCrate.OnDamage(1f);
        }


        EnemyThing aEnemy = other.gameObject.GetComponent<EnemyThing>();

        if (aEnemy != null)
        {
            switch (thisAttackType)
            {
                case 0:

                    aEnemy.CallOnDamage(thisDamage, thisAttackDir);

                    break;

                case 1:

                    aEnemy.CallOnKnockDamage(thisDamage, thisAttackDir);

                    break;
            }
        }

        Enemy_Boss aBoss = other.gameObject.GetComponent<Enemy_Boss>();

        if (aBoss != null)
        {
            aBoss.CallOnDamage(thisDamage);
        }

        DoorOpener aOpener = other.gameObject.GetComponent<DoorOpener>();

        if (aOpener != null)
        {
            aOpener.OpenDoor();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        DoorOpener aOpener = collision.gameObject.GetComponent<DoorOpener>();

        if (aOpener != null)
        {
            aOpener.OpenDoor();
        }
    }

    protected IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
