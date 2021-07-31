using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThing : MonoBehaviour
{
    // Basic components of enemy objects.
    protected Animator thisAnimator = null;
    protected SpriteRenderer thisSpriteRenderer = null;
    public Vector3 thisFacingDirection = Vector3.zero;
    protected Rigidbody thisRigidbody = null;
    protected AudioSource thisAudioSource = null;

    [SerializeField] private AudioClip thisBeenHitSound = null;
    [SerializeField] private AudioClip thisDeathSound = null;
    [SerializeField] private AudioClip thisHitGroundSound = null;
    // For Enemy Health.
    [SerializeField] private float thisHealth = 0f;
    [SerializeField] private float thisHitRecoverTime = 0f;
    [SerializeField] private float thisHitRecoverTimer = 0f;
    [SerializeField] private float thisKnockRecoverTime = 0f;
    [SerializeField] private float thisKnockRecoverTimer = 0f;
    // For Enemy states.
    [SerializeField] protected bool beenHitted = false;
    [SerializeField] protected bool beenKnockedDown = false;
    [SerializeField] protected bool beenKilled = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool canMove = true;
    protected void StartThing()
    {
        // Get basic components while start.
        thisAnimator = GetComponent<Animator>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisAudioSource = GetComponent<AudioSource>();
        thisFacingDirection = Vector3.left;
    }
    protected void UpdateThing()
    {
        UpdateTimers();
        UpdateFacingDir();

        if (!beenHitted && !beenKnockedDown && !isAttacking && !beenKilled)
        {
            canMove = true;
        }

        else if (beenHitted || beenKnockedDown || isAttacking || beenKilled)
        {
            canMove = false;
        }
    }
    // Update enemy's timers.
    protected void UpdateTimers()
    {
        // Here is the recover timer on normal hit.
        if (thisHitRecoverTimer > 0f)
        {
            beenHitted = true;

            thisAnimator.SetBool("BeenHit", true);

            thisHitRecoverTimer -= Time.deltaTime;
        }

        else
        {
            beenHitted = false;

            thisAnimator.SetBool("BeenHit", false);
        }

        // Here is the recover timer on kncoked down.
        if (thisKnockRecoverTimer > 0f)
        {
            beenKnockedDown = true;

            thisAnimator.SetBool("KnockedDown", true);

            thisKnockRecoverTimer -= Time.deltaTime;
        }

        else
        {
            if (thisHealth > 0)
            {
                beenKnockedDown = false;

                thisAnimator.SetBool("KnockedDown", false);
            }
        }
    }
    // Update enemy's sprite direction based on facing direction.
    protected void UpdateFacingDir()
    {
        if (thisFacingDirection == Vector3.left)
        {
            thisSpriteRenderer.flipX = false;
        }

        else if(thisFacingDirection == Vector3.right)
        {
            thisSpriteRenderer.flipX = true;
        }
    }
    // This section is all for enemy reaction on damage.
    public virtual void CallOnDamage(float aDamage, Vector3 aDir)
    {
        if (!beenKnockedDown)
        {
            thisHealth -= aDamage;

            if (thisHealth <= 0)
            {
                // Do death if health is below or equal 0;
                CallOnDeath(aDir);
            }

            else
            {
                // Do normal onDamage if health is not empty.
                OnDamage(aDir);
            }
        }
    }

    protected virtual void OnDamage(Vector3 aDamageDirection)
    {
        thisRigidbody.velocity = Vector3.zero;

        thisAudioSource.PlayOneShot(thisBeenHitSound);

        thisHitRecoverTimer = thisHitRecoverTime;

        thisFacingDirection = -aDamageDirection;
    }

    public virtual void CallOnKnockDamage(float aDamage, Vector3 aDir)
    {
        if (!beenKilled)
        {
            thisHealth -= aDamage;

            thisRigidbody.velocity = Vector3.zero;

            if (thisHealth <= 0)
            {
                // Do death if health is below or equal 0;
                CallOnDeath(aDir);
            }

            else
            {
                // Do Knocked Down Damage if health is not empty.
                OnDamage(aDir);
                thisRigidbody.AddForce(aDir * 3500f);
            }
        }
    }

    protected void OnKnockDamage(Vector3 aKnockDirection)
    {
        thisAudioSource.PlayOneShot(thisBeenHitSound);

        thisKnockRecoverTimer = thisKnockRecoverTime;

        thisRigidbody.AddForce(Vector3.up * 3000f);

        thisRigidbody.AddForce(aKnockDirection * 3000f);

        thisFacingDirection = -aKnockDirection;
    }

    protected virtual void CallOnDeath(Vector3 aDir)
    {
        StartCoroutine(OnDeathBehavoir(aDir));
    }
    protected IEnumerator OnDeathBehavoir(Vector3 aKnockDirection)
    {
        beenKilled = true;

        thisAnimator.SetBool("OnDeath", true);

        thisAudioSource.PlayOneShot(thisDeathSound);

        beenKnockedDown = true;

        thisRigidbody.AddForce(Vector3.up * 3000f);

        thisRigidbody.AddForce(aKnockDirection * 3000f);

        thisFacingDirection = -aKnockDirection;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (beenKnockedDown)
        {
            if (collision.gameObject != null)
            {
                thisAudioSource.PlayOneShot(thisHitGroundSound);
            }
        }
    }
}
