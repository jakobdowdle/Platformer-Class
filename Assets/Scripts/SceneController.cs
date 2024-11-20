using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    public static SceneController Instance;
    public Transform SpawnPoint;

    [SerializeField] private string _nextLevel;
    [SerializeField] private GameObject _nextLevelUI;
    [SerializeField] private GameObject _nextLevelButton;

    [SerializeField] GameObject audioVictoryPrefab;
    private AudioSource instantiateVictorySound;

    private void Awake() {
        // Singleton pattern setup
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        // Ensure UI and spawn point settings are initialized
        if (_nextLevelUI != null) {
            _nextLevelUI.SetActive(false);
        }

        if (SpawnPoint != null) {
            SpawnPoint.gameObject.SetActive(false); // Hides the spawn point in the scene
        }

        if (PlayerBehaviour.Instance != null) {
            StartCoroutine(PlayerBehaviour.Instance.Spawn(SpawnPoint.position));
        } else {
            Debug.LogWarning("PlayerBehaviour instance not found during Start.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Stop the game and show the "Next Level" UI
            GameManager.Instance.GameRunning = false;
            if (audioVictoryPrefab != null) {
                instantiateVictorySound = Instantiate(audioVictoryPrefab).GetComponent<AudioSource>();
                instantiateVictorySound.Play();
            }
            ActivateNextLevelUI();
        }
    }

    private void ActivateNextLevelUI() {
        if (_nextLevelUI != null) {
            _nextLevelUI.SetActive(true);
        }

        if (_nextLevelButton != null) {
            Button button = _nextLevelButton.GetComponent<Button>();
            if (button != null) {
                button.onClick.RemoveAllListeners(); // Prevent duplicate listeners
                button.onClick.AddListener(LoadNextLevel);
            } else {
                Debug.LogWarning("Next level button is missing a Button component!");
            }
        } else {
            Debug.LogWarning("Next level button not assigned!");
        }
    }

    private void LoadNextLevel() {
        //if (string.IsNullOrEmpty(_nextLevel)) {
        //    Debug.LogWarning("Next level scene name is not assigned!");
        //    return;
        //}

        //SceneManager.LoadScene(_nextLevel);
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void Restart() {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }
}
