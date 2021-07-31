using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Basic Components for player.
    private GameObject thisPlayerModel = null;
    private Rigidbody thisRigidbody = null;
    private Animator thisAnimator = null;
    private SpriteRenderer thisSpriteRenderer = null;

    [Header("Player Attributes")]
    [SerializeField] private float thisHP = 0f;
    [SerializeField] private float thisComboDamage = 0f;
    [SerializeField] private float thisFinisherDamage = 0f;
    [SerializeField] private float thisMovementSpeed = 0f;
    [SerializeField] private float thisJumpHeight = 0f;

    // Vars for player movement.
    private Vector3 thisXDirection = Vector3.zero;
    private Vector3 thisZDirection = Vector3.zero;
    private Vector3 thisFacingDirection = Vector3.zero;

    // Vars for player jumping.
    [HideInInspector] public bool isPlayerOnGround = true;
    private const float thisOnGroundMaxDistance = 0.2f;

    // Vars for player's states. For communicating with other scripts.
    [HideInInspector] public bool isPlayerFacingRight = true; // True = Player facing RIGHT, False = Player facing LEFT.
    [HideInInspector] public bool canPlayerMove = true;
    [HideInInspector] public bool beenAttack = false;
    [HideInInspector] public bool inFlightMode = false;

    // Vars for player's attack.
    [HideInInspector] public bool isPlayerAttack = false;
    [HideInInspector] public bool canPlayerCombo = false;
    private int thisPlayerAttackTimer = 0; // 50 = 1 second.
    [HideInInspector] public int thisPlayerComboIndex = 0;
    [SerializeField] private PlayerProjectile thisGunFirePrefab = null;
    [SerializeField] private PlayerFlightProjectile thisFlightBulletPrefab = null;
    private float thisAttackAnimationTimer = 0f;
    private const int thisComboTiming = 25;
    [SerializeField] private PlayerAttack thisPlayerAttackCollider = null;

    // For Player been hit.
    private float thisBeenAttackTimer = 0f;

    // For Player Sound Effect.
    private AudioSource thisAudioSource = null;
    [Header("Player Sound Effects")]
    [SerializeField] private AudioClip thisSwordSwing = null;
    [SerializeField] private AudioClip thisGunFire = null;

    // Start is called before the first frame update
    protected void Start()
    {
        thisPlayerModel = transform.GetChild(0).gameObject;
        thisRigidbody = GetComponent<Rigidbody>();
        thisAnimator = thisPlayerModel.GetComponent<Animator>();
        thisSpriteRenderer = thisPlayerModel.GetComponent<SpriteRenderer>();
        thisAudioSource = GetComponent<AudioSource>();

        thisFacingDirection = Vector3.right;
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateUI();

        if (!inFlightMode)
        {
            UpdateOnDamageTimer();

            if (!beenAttack)
            {
                if (canPlayerMove)
                {
                    UpdateMovement();
                }

                UpdateVelocity();
                UpdateAttack();
            }

            UpdateAnimation();
        }

        else
        {
            UpdateFlightMode();
            UpdateFlightMovement();
            UpdateFlightVelocity();
            UpdateFlightAttack();
        }
    }

    protected void FixedUpdate()
    {
        if (!inFlightMode)
        {
            FixUpdateComboTimer();
            FixUpdateGroundCheck();
        }
    }
    protected void UpdateMovement()
    {
        thisXDirection = Vector3.zero;
        thisZDirection = Vector3.zero;

        // Keyboard Input.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            thisZDirection += Vector3.forward;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            thisZDirection += Vector3.back;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            thisXDirection += Vector3.left;

            isPlayerFacingRight = false; // Update Player Facing Direction.

            thisFacingDirection = Vector3.left;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            thisXDirection += Vector3.right;

            isPlayerFacingRight = true; // Update Player Facing Direction.

            thisFacingDirection = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isPlayerOnGround)
            {
                thisRigidbody.AddForce(Vector3.up * thisJumpHeight);
            }
        }

        // Controller Input.

        float aXInput = Input.GetAxis("ControllerHorizontal");
        float aYInput = Input.GetAxis("ControllerVertical");

        if (aXInput > 0f)
        {
            thisXDirection += Vector3.right * aXInput;

            isPlayerFacingRight = true; // Update Player Facing Direction.

            thisFacingDirection = Vector3.right;
        }

        else if (aXInput < 0f)
        {
            thisXDirection += Vector3.right * aXInput;

            isPlayerFacingRight = false; // Update Player Facing Direction.

            thisFacingDirection = Vector3.left;
        }

        if (aYInput > 0f)
        {
            thisZDirection += Vector3.forward * aYInput;
        }

        else if (aYInput < 0f)
        {
            thisZDirection += Vector3.forward * aYInput;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (isPlayerOnGround)
            {
                thisRigidbody.AddForce(Vector3.up * thisJumpHeight);
            }
        }
    }

    protected void UpdateVelocity()
    {
        thisRigidbody.useGravity = true;
        // Using combination of X Velocity and Z Velocity to avoid velocity multiply issue.
        Vector3 aXVelocity = Vector3.zero;
        Vector3 aZVelocity = Vector3.zero;
        Vector3 aVelocity = Vector3.zero;

        aXVelocity = thisXDirection * thisMovementSpeed;
        aZVelocity = thisZDirection * thisMovementSpeed * 2f; // Make Z Velocty 3 times faster becasue of the pespective of the level.

        // Calculate the final Velocty of Player.
        aVelocity = aXVelocity + aZVelocity;
        aVelocity.y = thisRigidbody.velocity.y; // Set Y to avoid gravity broken.
        thisRigidbody.velocity = aVelocity;
    }

    // Check if player is on ground by using RayCast.
    protected void FixUpdateGroundCheck()
    {
        Vector3 aCenterRayPos = transform.position;
        aCenterRayPos.y -= GetComponent<Collider>().bounds.extents.y - 0.1f;
        Vector3 aLeftRayPos = aCenterRayPos;
        aLeftRayPos.x -= GetComponent<Collider>().bounds.extents.x;
        Vector3 aRightRayPos = aCenterRayPos;
        aRightRayPos.x += GetComponent<Collider>().bounds.extents.x;

        RaycastHit aCenterHit;
        RaycastHit aLeftHit;
        RaycastHit aRightHit;

        float aCenterDistance = 0f; // Distance between player and the ground.
        float aLeftDistance = 0f;
        float aRightDistance = 0f;

        if (Physics.Raycast(aCenterRayPos, Vector3.down, out aCenterHit, Mathf.Infinity))
        {
            aCenterDistance = aCenterHit.distance;
        }

        else
        {
            aCenterDistance = 50f;
        }

        if (Physics.Raycast(aLeftRayPos, Vector3.down, out aLeftHit, Mathf.Infinity))
        {
            aLeftDistance = aLeftHit.distance;
        }

        else
        {
            aLeftDistance = 50f;
        }

        if (Physics.Raycast(aRightRayPos, Vector3.down, out aRightHit, Mathf.Infinity))
        {
            aRightDistance = aRightHit.distance;
        }

        else
        {
            aRightDistance = 50f;
        }

        // Set isPlayerOnGround by the distance to the ground.


        if (aCenterDistance <= thisOnGroundMaxDistance || aLeftDistance <= thisOnGroundMaxDistance || aRightDistance <= thisOnGroundMaxDistance)
        {
            isPlayerOnGround = true;
        }

        else if (aCenterDistance > thisOnGroundMaxDistance && aLeftDistance > thisOnGroundMaxDistance && aRightDistance > thisOnGroundMaxDistance)
        {
            isPlayerOnGround = false;
        }
    }

    protected void FixUpdateComboTimer()
    {
        // This is the input timing system for the player attack.
        thisPlayerAttackTimer -= 1;

        if (thisPlayerAttackTimer > 0 && thisPlayerAttackTimer < thisComboTiming) // Player in right timing.
        {
            canPlayerCombo = true;

            isPlayerAttack = false;
        }

        else if (thisPlayerAttackTimer > thisComboTiming)
        {
            canPlayerCombo = false;
        }

        else if (thisPlayerAttackTimer <= 0)// If player missed the combo input timing.
        {
            canPlayerCombo = false;

            thisPlayerComboIndex = 0;

            isPlayerAttack = false;
        }
    }

    protected void UpdateAttack()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire1"))
        {
            if (!isPlayerAttack) // isPlayerAttack if to prevent attack been triggered multiple times.
            {
                if (isPlayerOnGround) // Player on ground!
                {
                    SetPlayerAttack();
                }

                else // Player in air!
                {
                    // Player Jump Attack!
                }
            }
        }
    }

    protected void SetPlayerAttack()
    {
        isPlayerAttack = true;

        if (canPlayerCombo)
        {
            thisPlayerComboIndex++;
        }

        switch (thisPlayerComboIndex)
        {
            case 1:

                ComboAttack();

                thisAnimator.SetInteger("thisAttackComboIndex", 1);

                thisAttackAnimationTimer = 0.217f;

                int aAnimationTime = (int)Mathf.Round(0.217f * 50f);

                thisPlayerAttackTimer = aAnimationTime + thisComboTiming;

                break;

            case 2:

                ComboAttack();

                thisAnimator.SetInteger("thisAttackComboIndex", 2);

                thisAttackAnimationTimer = 0.35f;

                aAnimationTime = (int)Mathf.Round(0.35f * 50f);

                thisPlayerAttackTimer = aAnimationTime + thisComboTiming;

                break;

            case 3:

                FinisherAttack();

                thisAnimator.SetInteger("thisAttackComboIndex", 3);

                PlayerProjectile aFireball;
                aFireball = Instantiate(thisGunFirePrefab, transform.position, Quaternion.identity);

                aFireball.thisDirection = thisFacingDirection;

                thisAttackAnimationTimer = 1f;

                thisPlayerAttackTimer = 50;

                thisPlayerComboIndex = 0;

                break;

            default:

                ComboAttack();

                thisAnimator.SetInteger("thisAttackComboIndex", 0);

                thisAttackAnimationTimer = 0.2f;

                aAnimationTime = (int)Mathf.Round(0.2f / 50f);

                thisPlayerAttackTimer = aAnimationTime + thisComboTiming;

                break;
        }
    }

    protected void ComboAttack()
    {
        PlayerAttack aAttackCollider = Instantiate(thisPlayerAttackCollider, transform.position + thisFacingDirection * 1f, Quaternion.identity);

        aAttackCollider.thisAttackDir = thisFacingDirection;

        aAttackCollider.thisDamage = thisComboDamage;

        aAttackCollider.thisAttackType = 0;

        thisAudioSource.PlayOneShot(thisSwordSwing);
    }

    protected void FinisherAttack()
    {
        PlayerAttack aAttackCollider = Instantiate(thisPlayerAttackCollider, transform.position + thisFacingDirection * 1f, Quaternion.identity);

        aAttackCollider.thisAttackDir = thisFacingDirection;

        aAttackCollider.thisDamage = thisFinisherDamage;

        aAttackCollider.thisAttackType = 1;

        thisAudioSource.PlayOneShot(thisGunFire);
    }

    protected void UpdateAnimation()
    {
        thisAnimator.SetBool("isFlying", false);

        if (beenAttack)
        {
            thisAnimator.SetBool("beenAttack", true);
        }

        else
        {
            thisAnimator.SetBool("beenAttack", false);
        }

        if (isPlayerFacingRight)
        {
            thisSpriteRenderer.flipX = false;
        }

        else
        {
            thisSpriteRenderer.flipX = true;
        }

        thisAnimator.SetFloat("thisSpeed", thisRigidbody.velocity.magnitude);

        thisAnimator.SetFloat("thisJumpSpeed", thisRigidbody.velocity.y);

        thisAnimator.SetBool("isOnGround", isPlayerOnGround);

        if (thisAttackAnimationTimer > 0f)
        {
            thisAnimator.SetBool("isAttack", true);

            thisAttackAnimationTimer -= Time.deltaTime;

            canPlayerMove = false;

            StopPlayer();
        }

        else
        {
            thisAnimator.SetBool("isAttack", false);

            canPlayerMove = true;
        }
    }

    protected void StopPlayer()
    {
        thisXDirection = Vector3.zero;
        thisZDirection = Vector3.zero;
    }

    public void CallOnDamage(float aDamage)
    {
        OnDamage(aDamage);
    }
    protected void OnDamage(float aDamage)
    {
        thisRigidbody.velocity = Vector3.zero;

        thisBeenAttackTimer = 0.5f;

        thisHP -= aDamage;

        if (thisHP <= 0)
        {
            GlobalController.ThisGlobalController.LoadLoseScene();
        }
    }

    protected void UpdateOnDamageTimer()
    {
        if (thisBeenAttackTimer > 0)
        {
            thisBeenAttackTimer -= Time.deltaTime;

            beenAttack = true;
        }

        else
        {
            beenAttack = false;
        }
    }

    protected void UpdateFlightMode()
    {
        thisAnimator.SetBool("isFlying", true);

        thisRigidbody.useGravity = false;
    }

    protected void UpdateFlightMovement()
    {
        thisXDirection = Vector3.zero;

        // Controller Input.

        float aXInput = Input.GetAxis("ControllerHorizontal");

        if (aXInput > 0f)
        {
            thisXDirection += Vector3.right * aXInput;

            thisSpriteRenderer.flipX = false;

            thisFacingDirection = Vector3.right;
        }

        else if (aXInput < 0f)
        {
            thisXDirection += Vector3.right * aXInput;

            thisSpriteRenderer.flipX = true;

            thisFacingDirection = Vector3.left;
        }
    }

    protected void UpdateFlightVelocity()
    {
        Vector3 aVelocity = Vector3.zero;

        aVelocity = thisXDirection * thisMovementSpeed * 2f;

        aVelocity.y = 5f;

        thisRigidbody.velocity = aVelocity;
    }

    protected void UpdateFlightAttack()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            Instantiate(thisFlightBulletPrefab, transform.position + Vector3.up, new Quaternion(0f, 0f, 270f, 0f));
        }
    }

    protected void UpdateUI()
    {
        UIController.ThisUIController.SetHPBarScale(thisHP);
    }

    public void CallOnHeal(float aHeal)
    {
        thisHP += aHeal;

        thisHP = Mathf.Clamp(thisHP, 0f, 20f);
    }
}
