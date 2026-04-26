using UnityEngine;
using UnityEngine.AI;

public class Animatronic1 : AnimatronicAI
{
    public Transform[] waypoints;
    public GameTimeManager gameTimeManager;
    public AudioSource footstepAudio;

    private int currentWaypoint = 0;
    private bool activated = false;
    private bool reachedDoor = false;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!activated && gameTimeManager != null)
        {
            if (gameTimeManager.CurrentHour >= 1f)
            {
                activated = true;
                Activate();
                Debug.Log("A1 activado!");
            }
        }

        // Esto debe estar ANTES del return para que siempre se actualice
        if (animator != null)
            animator.SetBool("isWalking", isActive && !reachedDoor);

        if (!isActive || reachedDoor) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();

        if (footstepAudio != null && !footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);

        if (currentWaypoint == waypoints.Length - 1)
        {
            reachedDoor = true;
            OnReachedDoor();
            return;
        }

        currentWaypoint++;
    }

    void OnReachedDoor()
    {
        Debug.Log("A1 llegó a la puerta!");
    }

    public void SendBack()
    {
        reachedDoor = false;
        currentWaypoint = 0;
        agent.SetDestination(waypoints[0].position);
    }
}