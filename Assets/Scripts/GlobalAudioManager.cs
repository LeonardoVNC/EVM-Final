using UnityEngine;

public class GlobalAudioManager : MonoBehaviour {
    public static GlobalAudioManager Instance;
    private AudioSource audioSource;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
