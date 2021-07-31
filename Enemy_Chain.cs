using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chain : EnemyThing
{
    public Player thisPlayer = null;
    private Vector3 thisTargetPos = Vector3.zero;
    [SerializeField] private float thisMovementSpeed = 0f;
    private Vector3 thisXDirection = Vector3.zero;
    private Vector3 thisZDirection = Vector3.zero;
    [SerializeField] private float thisBehaveUpdateTime = 0f;
    private float thisBehaveUpdateTimer = 0f;
    [SerializeField] private float thisAttackCD = 0f;
    private float thisAttackCDTimer = 0f;
    private bool canAttack = true;

    // Start is called before the first frame update
    protected void Start()
    {
        StartThing();
        thisPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateThing();
        UpdateBehave();
        UpdateWalkAnimation();

        if (!beenKnockedDown)
        {
            UpdateAttackPlayer();
            UpdateVelocity();
        }

        if (canMove)
        {
            MovetoLocation(thisTargetPos);
        }
    }

    protected void UpdateVelocity()
    {
        // Using combination of X Velocity and Z Velocity to avoid velocity multiply issue.
        Vector3 aXVelocity = Vector3.zero;
        Vector3 aZVelocity = Vector3.zero;
        Vector3 aVelocity = Vector3.zero;

        aXVelocity = thisXDirection * thisMovementSpeed;
        aZVelocity = thisZDirection * thisMovementSpeed * 3f; // Make Z Velocty 3 times faster becasue of the pespective of the level.

        // Calculate the final Velocty of Player.
        aVelocity = aXVelocity + aZVelocity;
        aVelocity.y = thisRigidbody.velocity.y; // Set Y to avoid gravity broken.
        thisRigidbody.velocity = aVelocity;
    }

    protected void UpdateWalkAnimation()
    {
        thisAnimator.SetFloat("WalkSpeed", thisRigidbody.velocity.magnitude);
    }
    protected void UpdateBehave()
    {
        if (thisBehaveUpdateTimer >= 0f)
        {
            thisBehaveUpdateTimer -= Time.deltaTime;
        }

        else
        {
            int aBehaveInt = 0;

            aBehaveInt = Random.Range(1, 2);

            thisBehaveUpdateTimer = thisBehaveUpdateTime;

            switch (aBehaveInt)
            {
                case 0:

                    Vector3 aPlayerPos = thisPlayer.transform.position;

                    float thisRandomX = aPlayerPos.x + Random.Range(-5f, 5f);
                    float thisRandomZ = aPlayerPos.z + Random.Range(-2.5f, 2.5f);

                    Vector3 aRandomPos = Vector3.zero;
                    aRandomPos.x = thisRandomX;
                    aRandomPos.z = thisRandomZ;

                    thisTargetPos = aRandomPos;
                    print("Walk to random pos");
                    break;

                case 1:

                    float aXDiff = transform.position.x - thisPlayer.transform.position.x;

                    aPlayerPos = thisPlayer.transform.position;

                    if (aXDiff > 0)
                    {
                        aPlayerPos.x += 2f;
                       
                    }

                    else
                    {
                        aPlayerPos.x -= 2f;
                    }

                    thisTargetPos = aPlayerPos;
                    thisTargetPos = thisPlayer.transform.position;
                    print("Walk to player");
                    break;
            }
        }
    }

    protected void UpdateAttackPlayer()
    {
        Vector3 aPosDifference = transform.position - thisPlayer.transform.position;
        float aDistance = aPosDifference.magnitude;

        if (aDistance <= 1.5f)
        {
            if (thisAttackCDTimer >= 0f)
            {
                thisAttackCDTimer -= Time.deltaTime;
            }

            else
            {
                canAttack = true;
            }

            if (canAttack)
            {
                StartCoroutine(AttackPlayerBehavior());

                canAttack = false;

                thisAttackCDTimer = thisAttackCD;

                thisBehaveUpdateTimer = 2f;
            }
        }
    }

    protected IEnumerator AttackPlayerBehavior()
    {
        isAttacking = true;

        thisAnimator.SetBool("isAttack", true);

        Vector3 aRayPos = transform.position; // A raycast start position.

        RaycastHit aHit; // The hit result of the ray.

        float aDistance = 0f; // The distance between player and object. I use it to determine if player can hit an object.

        if (Physics.Raycast(aRayPos, thisFacingDirection, out aHit, Mathf.Infinity))
        {
            aDistance = aHit.distance; // Right Distance = 1.5f

            if (aDistance <= 1.5f) // a hit object is in player's melee attack range.
            {
                Player aPlayer = aHit.collider.gameObject.GetComponent<Player>();

                if (aPlayer != null)
                {
                    aPlayer.CallOnDamage(1f);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        thisAnimator.SetBool("isAttack", false);

        isAttacking = false;
    }
    protected void MovetoLocation(Vector3 aLocation)
    {
        thisXDirection = Vector3.zero;
        thisZDirection = Vector3.zero;

        Vector3 aPos = aLocation;
        Vector3 aSelfPos = transform.position;

        if (aLocation.x - aSelfPos.x < -1f)
        {
            HorizontalMovement(-1f);
        }

        else if (aLocation.x - aSelfPos.x > 1f)
        {
            HorizontalMovement(1f);
        }

        if (aLocation.z - aSelfPos.z < -1f)
        {
            VerticalMovement(-1f);
        }

        else if (aLocation.z - aSelfPos.z > 1f)
        {
            VerticalMovement(1f);
        }

    }

    protected void HorizontalMovement(float aDir)
    {
        thisXDirection += aDir * Vector3.right;
    }

    protected void VerticalMovement(float aDir)
    {
        thisZDirection += aDir * Vector3.forward;
    }

    public override void CallOnDamage(float aDamage, Vector3 aDir)
    {
        thisXDirection = Vector3.zero;
        thisZDirection = Vector3.zero;

        base.CallOnDamage(aDamage, aDir);

        thisAttackCDTimer = thisAttackCD;
    }

    public override void CallOnKnockDamage(float aDamage, Vector3 aDir)
    {
        thisXDirection = Vector3.zero;
        thisZDirection = Vector3.zero;

        base.CallOnKnockDamage(aDamage, aDir);

        thisAttackCDTimer = thisAttackCD;
    }

    // Enemy Behavior:
    // 1. Try to approch player and attack player
    // 2. Randomly move to some place of the scene.
    // 3. Try to pick up some items?
    // 4. Only one enemy should try to attack player at the same time?
}
