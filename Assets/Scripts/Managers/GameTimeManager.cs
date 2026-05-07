using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    private float realSecondsPerGameHour = 60f;
    private float currentHour = 0f;
    private int lastHourTracked = 0;
    private bool timeStopped = false;
    public BaseAnimatronic[] animatronics;
    private int difficultyIncrease = 2;
    public AudioClip clockClip;
    public AudioClip clockWinClip;
    public float CurrentHour => currentHour;

    void Start()
    {
        int difficulty = MainMenu.difficulty;
        realSecondsPerGameHour = 40f + (10f * difficulty);

        foreach (BaseAnimatronic anim in animatronics)
        {
            if (anim != null)
            {
                anim.SetAILevel(2 + difficulty * 2);
                anim.SetMovementInterval(15f - (difficulty * 2));
                anim.SetAttackTimer(6f - difficulty);
            }
        }

        
        if (animatronics.Length > 2 && animatronics[2] != null)
        {
            float reappearDelay = difficulty == 1 ? 20f : difficulty == 2 ? 12f : 5f;
            float flashHold = difficulty == 1 ? 2f : difficulty == 2 ? 3f : 5f;
            animatronics[2].SetReappearDelay(reappearDelay);
            animatronics[2].SetFlashlightHoldRequired(flashHold);
        }
    }

    void Update()
    {
        if (timeStopped || !GameManager.Instance.HasPower) return;
        currentHour += Time.deltaTime / realSecondsPerGameHour;
        int hourAsInt = Mathf.FloorToInt(currentHour);
        if (hourAsInt > lastHourTracked)
            OnHourPassed(hourAsInt);
        UIManager.Instance.UpdateTimeText(GetFormattedTime());
    }

    void OnHourPassed(int newHour)
    {
        lastHourTracked = newHour;
        CheckAnimatronics(newHour);
        if (newHour >= 6)
        {
            timeStopped = true;
            if (clockWinClip != null) GlobalAudioManager.Instance.PlayGlobalSound(clockWinClip);
            Invoke("NotifyWin", 4f);
        }
        else
        {
            if (clockClip != null) GlobalAudioManager.Instance.PlayGlobalSound(clockClip);
        }
    }

    void NotifyWin() => GameManager.Instance.Win();

    private void CheckAnimatronics(int newHour)
    {
        if (newHour == 1 && animatronics.Length > 0 && animatronics[0] != null)
            animatronics[0].Activate();
        if (newHour == 2 && animatronics.Length > 1 && animatronics[1] != null)
            animatronics[1].Activate();
        if (newHour == 3 && animatronics.Length > 2 && animatronics[2] != null)
            animatronics[2].Activate();

        foreach (BaseAnimatronic anim in animatronics)
        {
            if (anim != null) anim.IncreaseDifficulty(difficultyIncrease);
        }
    }

    public string GetFormattedTime()
    {
        int hour = Mathf.FloorToInt(currentHour);
        int displayHour = (hour == 0) ? 12 : (hour > 6 ? 6 : hour);
        return string.Format("{0} AM", displayHour);
    }
}