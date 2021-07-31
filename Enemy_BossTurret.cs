using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossTurret : MonoBehaviour
{
    [SerializeField] private GameObject thisTurret = null;
    private AudioSource thisAudioSource = null;
    private GameObject thisPlayer = null;
    private float thisFireTimer = 0f;
    [SerializeField] private Projectile_Enemy thisBulletPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        thisPlayer = GameObject.Find("Player");
        thisFireTimer = 3f;
        thisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        thisTurret.transform.LookAt(thisPlayer.transform);

        if (thisFireTimer >= 0f)
        {
            thisFireTimer -= Time.deltaTime;
        }

        else
        {
            Vector3 aBulletDir = thisPlayer.transform.position - transform.position;

            Projectile_Enemy aBullet = Instantiate(thisBulletPrefab, thisTurret.transform.position, Quaternion.identity);

            aBullet.thisDirection = aBulletDir;

            thisFireTimer = 3f;

            thisAudioSource.Play();
        }
    }
}
