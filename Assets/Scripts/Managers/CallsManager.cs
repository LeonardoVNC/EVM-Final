using UnityEngine;

public class CallsManager : MonoBehaviour {
    private bool[] callTriggered = new bool[4];
    
    public AudioSource callAudioSource;
    public AudioClip[] calls;

    public void PlayCall(int index) {
        if (callTriggered[index] || calls == null || index >= calls.Length || calls[index] == null) return;
        
        callTriggered[index] = true;
        callAudioSource.clip = calls[index];
        callAudioSource.Play();
    }

    public void StopCall() {
        if (callAudioSource != null && callAudioSource.isPlaying) {
            callAudioSource.Stop();
        }
    }

    public void PauseCall(){
        if (callAudioSource.isPlaying) callAudioSource.Pause();
    }

    public void UnPauseCall() {
        if (callAudioSource.clip != null) callAudioSource.UnPause();
    }
}
