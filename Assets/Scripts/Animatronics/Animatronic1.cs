using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class Animatronic1 : BaseAnimatronic
{
    public Transform[] route1Waypoints;
    public int doorCheckWaypointIndex = 2;
    public DoorController doorControllerLeft;

    public Transform[] route2Waypoints;

    public Transform waypointAuditorio;

    public float flashlightDetectionRange = 15f;
    public float flashlightHoldRequired = 0.8f;

    public VideoPlayer jumpscareVideo;
    public RawImage jumpscareImage;

    private enum A1State
    {
        Idle, Route1, Route1Attack, Route2,
        NearPlayer, ReturningToAuditorio, Attacking
    }
    private A1State state = A1State.Idle;

    private Transform[] currentRoute;
    private int currentWaypointIndex = 0;
    private bool nextRouteIsRoute2 = false;
    private bool isVulnerable = false;
    private float flashlightHoldTimer = 0f;

    public override void Activate()
    {
        base.Activate();
        StartRoute1();
    }
    protected override void Update()
    {
        if (!isActive) return;

        // Si está en blackout, manejar directamente sin pasar por base.Update
        if (currentAIState == AIState.Blackout)
        {
            HandleBlackoutAI();
            return;
        }

        base.Update();

        switch (state)
        {
            case A1State.Route1:
            case A1State.Route1Attack:
            case A1State.Route2:
            case A1State.ReturningToAuditorio:
                AdvanceAlongRoute();
                break;
            case A1State.NearPlayer:
                HandleNearPlayer();
                TrackFlashlight();
                break;
        }
    }

    protected override void OnMovementTick() { }

    void StartRoute1()
    {
        currentRoute = route1Waypoints;
        currentWaypointIndex = 0;
        state = A1State.Route1;
        SetDestination();
    }

    void StartRoute1Attack()
    {
        currentRoute = route1Waypoints;
        currentWaypointIndex = route1Waypoints.Length - 1;
        state = A1State.Route1Attack;
        SetDestination();
    }

    void StartRoute2()
    {
        currentRoute = route2Waypoints;
        currentWaypointIndex = 0;
        state = A1State.Route2;
        SetDestination();
    }

    void StartReturnToAuditorio(bool goToRoute2Next)
    {
        isVulnerable = false;
        flashlightHoldTimer = 0f;
        nextRouteIsRoute2 = goToRoute2Next;
        state = A1State.ReturningToAuditorio;

        agent.ResetPath();
        agent.Warp(transform.position + (-transform.forward * 4f));
        StartCoroutine(SetDestinationDelayed(waypointAuditorio, 0.5f));
    }

    IEnumerator SetDestinationDelayed(Transform target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
            agent.SetDestination(target.position);
    }

    void SetDestination()
    {
        if (currentRoute == null || currentRoute.Length == 0) return;
        agent.SetDestination(currentRoute[currentWaypointIndex].position);
    }

    void AdvanceAlongRoute()
    {
        if (state == A1State.Route1)
        {
            if (doorControllerLeft != null && doorControllerLeft.IsClosed())
            {
                agent.ResetPath();
                StartReturnToAuditorio(goToRoute2Next: true);
                return;
            }
        }

        if (state == A1State.ReturningToAuditorio)
        {
            float distToPlayer = playerTransform != null ?
                Vector3.Distance(transform.position, playerTransform.position) : float.MaxValue;
            if (doorControllerLeft != null && doorControllerLeft.IsClosed() && distToPlayer < 6f)
            {
                EnterNearPlayerState();
                return;
            }
        }

        if (agent.pathPending) return;
        if (agent.remainingDistance > agent.stoppingDistance + 0.1f) return;

        if (state == A1State.ReturningToAuditorio)
        {
            if (nextRouteIsRoute2) StartRoute2();
            else StartRoute1();
            return;
        }

        if (state == A1State.Route1 && currentWaypointIndex == doorCheckWaypointIndex)
        {
            StartRoute1Attack();
            return;
        }

        if (currentWaypointIndex >= currentRoute.Length - 1)
        {
            EnterNearPlayerState();
            return;
        }

        currentWaypointIndex++;
        SetDestination();
    }

    void EnterNearPlayerState()
    {
        state = A1State.NearPlayer;
        isVulnerable = true;
        flashlightHoldTimer = 0f;
        attackCountdown = attackTimer;
        agent.ResetPath(); // se queda parado en el waypoint
    }

    void HandleNearPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 dir = playerTransform.position - transform.position;
            dir.y = 0f;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 8f
                );
        }

        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0f)
            StartCoroutine(AttackSequence());
    }

    void TrackFlashlight()
    {
        if (!isVulnerable || playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist > flashlightDetectionRange) return;

        bool flashlightOn = IsFlashlightPointingAtMe();

        if (flashlightOn)
        {
            flashlightHoldTimer += Time.deltaTime;
            if (flashlightHoldTimer >= flashlightHoldRequired)
            {
                flashlightHoldTimer = 0f;
                bool goRoute2Next = (currentRoute == route1Waypoints);
                StartReturnToAuditorio(goToRoute2Next: goRoute2Next);
            }
        }
        else
        {
            flashlightHoldTimer = 0f;
        }
    }

    bool IsFlashlightPointingAtMe()
    {
        Flashlight fl = InputManager.Instance?.flashlight;
        if (fl == null || !fl.IsOn) return false;

        Camera playerCam = Camera.main;
        if (playerCam == null) return false;

        float dist = Vector3.Distance(transform.position, playerCam.transform.position);
        Vector3 dirToAnim = (transform.position - playerCam.transform.position).normalized;
        float angle = Vector3.Angle(playerCam.transform.forward, dirToAnim);

        return angle < (dist < 3f ? 60f : 30f);
    }

    IEnumerator AttackSequence()
    {
        if (state == A1State.Attacking) yield break;
        state = A1State.Attacking;
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

    protected override void HandleDoorWaiting() { }

    protected override void HandleBlackoutAI()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        agent.speed = dist <= detectionRange ? chaseSpeed : moveSpeed * 0.5f;
        agent.SetDestination(playerTransform.position);

        if (dist < 3.5f)
            StartCoroutine(AttackSequence());
    }

    public override void SetAIState(AIState newState)
    {
        if (newState == AIState.Blackout)
        {
            isVulnerable = false;
            flashlightHoldTimer = 0f;
            agent.ResetPath();
            agent.speed = chaseSpeed;
        }
        base.SetAIState(newState);
    }
}