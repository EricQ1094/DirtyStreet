using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] thisEnemysArray = null;
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
            foreach(GameObject aEnemy in thisEnemysArray)
            {
                aEnemy.GetComponent<EnemyThing>().enabled = true;
            }

            Destroy(gameObject);
        }
    }
}
