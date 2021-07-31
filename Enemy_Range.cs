using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : EnemyThing
{
    private Player thisPlayer = null;
    private bool isFacingLeft = true;
    [SerializeField] private float thisSpeed = 0f;
    private float thisFireTimer = 0f;
    [SerializeField] private float thisFireCD = 0f;
    [SerializeField] private Vector3 thisMovetoTarget = Vector3.zero;
    [SerializeField] private Projectile_Enemy thisBulletPrefab = null;
    [SerializeField] private GameObject thisExplodePrefab = null;
    private enum thisBehaviorMode {moveToPosition, shootAtPlayer};
    private thisBehaviorMode thisBehavior;

    protected void Start()
    {
        StartThing();

        thisPlayer = GameObject.Find("Player").GetComponent<Player>();

        thisFireTimer = thisFireCD;

        thisBehavior = thisBehaviorMode.moveToPosition;

        thisMovetoTarget = transform.position + Vector3.right;
    }
    protected void Update()
    {
        if (!beenKilled)
        {
            UpdateThing();
            UpdateFacing();
            if (!beenHitted)
            {
                UpdateBehavior();
            }

            else
            {
                thisFireTimer = thisFireCD;
            }
        }
    }

    protected void UpdateFacing()
    {
        float aXDifference = thisPlayer.transform.position.x - transform.position.x;

        if (aXDifference < 0f)
        {
            isFacingLeft = true;

            thisSpriteRenderer.flipX = false;

            thisFacingDirection = Vector3.left;
        }

        else
        {
            isFacingLeft = false;

            thisSpriteRenderer.flipX = true;

            thisFacingDirection = Vector3.right;
        }
    }

    protected void UpdateFire()
    {
        if (thisFireTimer >= 0f)
        {
            thisFireTimer -= Time.deltaTime;
        }

        else
        {
            thisFireTimer = thisFireCD;

            StartCoroutine(FireBehavior());
        }
    }

    protected void UpdateBehavior()
    {
        switch (thisBehavior)
        {
            // Move to Shooting Position.
            case thisBehaviorMode.moveToPosition:

                MoveToPos(thisMovetoTarget);

                break;

            case thisBehaviorMode.shootAtPlayer:

                UpdateFire();
            // Shoot at player.
                break;
        }
    }

    protected void SetTargetPosition()
    {
        if (isFacingLeft)
        {
            thisMovetoTarget = thisPlayer.transform.position;

            thisMovetoTarget.x += 5f;
        }

        else
        {
            thisMovetoTarget = thisPlayer.transform.position;

            thisMovetoTarget.x -= 5f;
        }
    }

    protected void MoveToPos(Vector3 aPos)
    {
        Vector3 aDir = aPos - transform.position;

        thisRigidbody.velocity = aDir.normalized * thisSpeed;

        thisAnimator.SetFloat("WalkSpeed", thisRigidbody.velocity.magnitude);

        CheckPos();
    }

    protected void CheckPos()
    {
        float aDistance = (thisMovetoTarget - transform.position).magnitude;

        if (aDistance <= 1f)
        {
            thisBehavior = thisBehaviorMode.shootAtPlayer;
        }
    }

    protected IEnumerator FireBehavior()
    {
        thisAnimator.SetFloat("WalkSpeed", 0);

        thisAnimator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.5f);

        Projectile_Enemy aBulletIns = Instantiate(thisBulletPrefab, transform.position, Quaternion.identity);

        aBulletIns.thisDirection = thisFacingDirection;

        yield return new WaitForSeconds(0.3f);

        thisAnimator.SetBool("isAttacking", false);

        thisBehavior = thisBehaviorMode.moveToPosition;

        SetTargetPosition();
    }

    protected override void CallOnDeath(Vector3 aDir)
    {
        Instantiate(thisExplodePrefab, transform.position, Quaternion.identity);

        base.CallOnDeath(aDir);
    }
}
