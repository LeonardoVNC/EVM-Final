using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class Animatronic3 : BaseAnimatronic
{
    public Transform spawnPoint;
    public Transform appearPoint;
    public Transform blackoutSpawnPoint;

    public float reappearDelay = 20f;
    public float attackWindow = 3f;
    public float flashlightHoldRequired = 3f;
    public float flashlightDetectionRange = 10f;

    public VideoPlayer jumpscareVideo;
    public RawImage jumpscareImage;

    public AudioClip ghost_scape;
    public AudioClip ghost_retreat;

    private enum A3State { Inactive, Visible, Retreating }
    private A3State state = A3State.Inactive;

    private float attackCountdownA3;
    private float flashlightHoldTimer;
    private bool isAttacking = false;
    private bool isBeingFlashed = false;

    private Vector3 realSpawnPos;
    private Vector3 realAppearPos;

    protected override void Awake()
    {
        base.Awake();
        if (spawnPoint != null) realSpawnPos = spawnPoint.position;
        if (appearPoint != null) realAppearPos = appearPoint.position;
        if (agent != null) agent.enabled = false;
    }

    public override void Activate()
    {
        isActive = true;
        StartCoroutine(AppearAfterDelay(reappearDelay));
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsPoweroutActive && currentAIState != AIState.Blackout)
        {
            SetAIState(AIState.Blackout);
            return;
        }

        if (!GameManager.Instance.IsPoweroutActive && currentAIState == AIState.Blackout)
        {
            SetAIState(AIState.Default);
            return;
        }

        if (!isActive) return;

        if (currentAIState == AIState.Default)
            HandleDefaultAI();
        else
            HandleBlackoutAI();
    }

    protected override void HandleDefaultAI()
    {
        if (state != A3State.Visible) return;

        isBeingFlashed = TrackFlashlight();

        if (!isBeingFlashed)
        {
            attackCountdownA3 -= Time.deltaTime;
            if (attackCountdownA3 <= 0f && !isAttacking)
                StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AppearAfterDelay(float delay)
    {
        state = A3State.Retreating;
        yield return new WaitForSeconds(delay);

        if (!isActive || currentAIState == AIState.Blackout) yield break;

        int roll = Random.Range(1, 21);
        if (roll > aiLevel)
        {
            StartCoroutine(AppearAfterDelay(reappearDelay));
            yield break;
        }

        TeleportToPlayer();
    }

void TeleportToPlayer()
{
    Vector3 targetPos = appearPoint != null
        ? realAppearPos
        : playerTransform.position + playerTransform.forward * 1.5f;

    transform.position = targetPos;

    if (playerTransform != null)
    {
        Vector3 dir = playerTransform.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    gameObject.SetActive(true);
    state = A3State.Visible;
    flashlightHoldTimer = 0f;
    attackCountdownA3 = attackWindow;
    isAttacking = false;
    isBeingFlashed = false;

    if (ghost_scape != null) GlobalAudioManager.Instance.PlayGlobalSound(ghost_scape);
    if (animator != null) animator.SetBool("isLooking", false);
}

    bool TrackFlashlight()
    {
        if (playerTransform == null) return false;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist > flashlightDetectionRange)
        {
            flashlightHoldTimer = Mathf.Max(0f, flashlightHoldTimer - Time.deltaTime * 3f);
            if (animator != null) animator.SetBool("isLooking", false);
            return false;
        }

        bool flashlightOn = IsFlashlightPointingAtMe();

        if (flashlightOn)
        {
            if (animator != null) animator.SetBool("isLooking", true);
            flashlightHoldTimer += Time.deltaTime;

            if (flashlightHoldTimer >= flashlightHoldRequired)
            {
                flashlightHoldTimer = 0f;
                Retreat();
            }
            return true;
        }
        else
        {
            if (animator != null) animator.SetBool("isLooking", false);
            flashlightHoldTimer = Mathf.Max(0f, flashlightHoldTimer - Time.deltaTime * 0.5f);
            return false;
        }
    }

    bool IsFlashlightPointingAtMe()
    {
        Flashlight fl = InputManager.Instance?.flashlight;
        if (fl == null || !fl.IsOn) return false;

        Camera playerCam = Camera.main;
        if (playerCam == null) return false;

        Vector3 targetCenter = transform.position + Vector3.up * 2.0f;
        Vector3 dirToAnim = (targetCenter - playerCam.transform.position).normalized;
        float angle = Vector3.Angle(playerCam.transform.forward, dirToAnim);
        return angle < 65f;
    }

    void Retreat()
    {
        state = A3State.Retreating;
        if (animator != null) animator.SetBool("isLooking", false);
        if (agent != null) agent.enabled = false;
        transform.position = realSpawnPos;
        if (ghost_retreat != null) GlobalAudioManager.Instance.PlayGlobalSound(ghost_retreat);
        StartCoroutine(AppearAfterDelay(reappearDelay));
    }

    protected override void HandleBlackoutAI()
    {
        if (playerTransform == null || agent == null || !agent.enabled) return;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = 0f;

        float dist = Vector3.Distance(
            transform.position + Vector3.up * 3.5f,
            playerTransform.position + Vector3.up * 1f
        );

        agent.SetDestination(playerTransform.position);

        if (animator != null) animator.SetBool("isLooking", true);

        if (dist <= 9f && !isAttacking)
            StartCoroutine(AttackSequence());
    }

    public override void SetAIState(AIState newState)
    {
        currentAIState = newState;

        if (newState == AIState.Blackout)
        {
            StopAllCoroutines();
            isActive = true;
            isAttacking = false;
            state = A3State.Visible;

            gameObject.SetActive(true);

            if (agent != null && blackoutSpawnPoint != null)
            {
                agent.enabled = true;
                agent.speed = chaseSpeed;
                agent.stoppingDistance = 0f;

                UnityEngine.AI.NavMeshHit hit;
                bool onMesh = UnityEngine.AI.NavMesh.SamplePosition(
                    blackoutSpawnPoint.position, out hit, 1f, UnityEngine.AI.NavMesh.AllAreas);
                Debug.Log($"[A3] blackoutSpawn en NavMesh: {onMesh} — pos:{blackoutSpawnPoint.position}");

                agent.Warp(blackoutSpawnPoint.position);
                if (ghost_scape != null) GlobalAudioManager.Instance.PlayGlobalSound(ghost_scape);
            }

            if (animator != null) animator.SetBool("isLooking", true);
        }
        else if (newState == AIState.Default)
        {
            if (agent != null) agent.enabled = false;
            Retreat();
        }
    }

    IEnumerator AttackSequence()
    {
        if (isAttacking) yield break;
        GameManager.Instance.StopCall();
        isAttacking = true;
        state = A3State.Retreating;

        if (agent != null) agent.enabled = false;
        GameManager.Instance.SetAlive(false);

        if (jumpscareImage != null) jumpscareImage.gameObject.SetActive(true);
        if (jumpscareVideo != null)
        {
            jumpscareVideo.Play();
            yield return new WaitForSeconds((float)jumpscareVideo.length);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        GameManager.Instance.GoToGameOverScreen();
    }

    protected override void OnMovementTick() { }
    protected override void HandleDoorWaiting() { }
}