using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private Animator thisAnimator = null;
    private AudioSource thisAudioSource = null;
    [SerializeField] private float thisHealth = 0f;
    [SerializeField] private AudioClip thisDestroySound = null;
    private bool onDeath = false;
    [SerializeField] private GameObject thisDrops = null;
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        thisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDamage(float aDamage)
    {
        if (!onDeath)
        {
            thisHealth -= aDamage;

            StartCoroutine(ShakeBehavior());

            if (thisHealth <= 0f)
            {
                StartCoroutine(DeathBehavior());
            }

            else
            {
                thisAudioSource.Play();
            }
        }
    }

    protected IEnumerator ShakeBehavior()
    {
        thisAnimator.SetBool("isShaking", true);

        yield return new WaitForSeconds(0.133f);

        thisAnimator.SetBool("isShaking", false);
    }

    protected IEnumerator DeathBehavior()
    {
        onDeath = true;

        thisAnimator.SetBool("isBroken", true);

        thisAudioSource.PlayOneShot(thisDestroySound);

        yield return new WaitForSeconds(1.0f);

        if (thisDrops != null)
        {
            Instantiate(thisDrops, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
