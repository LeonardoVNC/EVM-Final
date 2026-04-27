using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Animatronic1 : AnimatronicAI
{
    public Transform[] waypoints;
    public GameTimeManager gameTimeManager;
    public AudioSource footstepAudio;

    public DoorController doorControllerLeft;
    public DoorController doorControllerRight;

    public string gameOverSceneName = "GameOverScreen";
    public float waitAtDoorTime = 6f;
    public float attackTimer = 3f;

    private int currentWaypoint = 0;
    private bool activated = false;
    private bool reachedDoor = false;
    private bool waitingAtDoor = false;
    private bool attacking = false;
    private float doorTimer = 0f;
    private float attackCountdown = 0f;
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

        if (animator != null)
            animator.SetBool("isWalking", isActive && !reachedDoor);

        if (!isActive) return;

        if (waitingAtDoor)
        {
            doorTimer += Time.deltaTime;

           
            if (IsBlockedByDoor())
            {
                Debug.Log("A1 bloqueado por puerta, retrocediendo");
                waitingAtDoor = false;
                reachedDoor = false;
                SendBack();
                return;
            }
            attackCountdown -= Time.deltaTime;
            Debug.Log("A1 esperando, countdown: " + attackCountdown);

            if (attackCountdown <= 0f && !attacking)
            {
                attacking = true;
                StartCoroutine(AttackSequence());
            }
            return;
        }

        if (reachedDoor) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();

        if (footstepAudio != null && !footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    bool IsBlockedByDoor()
    {
        if (doorControllerLeft != null && doorControllerLeft.IsClosed()) return true;
        if (doorControllerRight != null && doorControllerRight.IsClosed()) return true;
        return false;
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);

        if (currentWaypoint == waypoints.Length - 1)
        {
           
            if (agent.remainingDistance < 0.3f)
            {
                reachedDoor = true;
                OnReachedDoor();
            }
            return;
        }

        currentWaypoint++;
    }

    void OnReachedDoor()
    {
        Debug.Log("A1 llegó a la puerta!");
        waitingAtDoor = true;
        doorTimer = 0f;
        attackCountdown = attackTimer;

        
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.updateRotation = false;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    IEnumerator AttackSequence()
    {
        Debug.Log("JUMPSCARE!");
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToGameOverScreen();
    }

    public void SendBack()
    {
        StopAllCoroutines(); 
        agent.updateRotation = true;
        reachedDoor = false;
        waitingAtDoor = false;
        attacking = false;
        currentWaypoint = 0;
        doorTimer = 0f;
        attackCountdown = 0f;
        agent.SetDestination(waypoints[0].position);
    }
}