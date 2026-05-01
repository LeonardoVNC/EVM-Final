using UnityEngine;
using System.Collections;

public class Animatronic1 : BaseAnimatronic {
    public DoorController doorControllerRight;
    public UnityEngine.Video.VideoPlayer jumpscareVideo;
    public UnityEngine.UI.RawImage jumpscareImage;

    protected override void OnMovementTick() {
        if (reachedDoor || waypoints.Length == 0 || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance) {
            agent.SetDestination(waypoints[currentWaypoint].position);

            if (currentWaypoint == waypoints.Length - 1) {
                StartCoroutine(CheckArrival());
            } else {
                currentWaypoint++;
            }
        }
    }

    void OnReachedDoor() {
        waitingAtDoor = true;
        attackCountdown = attackTimer;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.updateRotation = false;
    }

    protected override void HandleDoorWaiting() {
        if (doorControllerRight != null && doorControllerRight.IsClosed()) {
            SendBack();
            return;
        }
        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0f) {
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence() {
        waitingAtDoor = false; 
        if (jumpscareImage != null) jumpscareImage.gameObject.SetActive(true);
        if (jumpscareVideo != null) {
            jumpscareVideo.Play();
            yield return new WaitForSeconds((float)jumpscareVideo.length);
        } else {
            yield return new WaitForSeconds(2f);
        }
        GameManager.Instance.GoToGameOverScreen();
    }

    IEnumerator CheckArrival() {
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);
        reachedDoor = true;
        OnReachedDoor();
    }

    public void SendBack() {
        reachedDoor = false;
        waitingAtDoor = false;
        currentWaypoint = 0;
        agent.updateRotation = true;
        
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(waypoints[0].position, out hit, 20f, UnityEngine.AI.NavMesh.AllAreas)) {
            agent.Warp(hit.position);
            agent.SetDestination(waypoints[0].position);
        }
    }

    protected override void HandleBlackoutAI() {
        Debug.Log("Modo blackout chico");
    }
}
