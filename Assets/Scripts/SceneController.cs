using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public static SceneController Instance;
    public Transform SpawnPoint;
    [SerializeField] private string _nextLevel;
    [SerializeField] private GameObject _nextLevelUI;
    [SerializeField] private GameObject _nextLevelButton;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // Ensure the UI is hidden at the start
        if (_nextLevelUI != null) {
            _nextLevelUI.SetActive(false);
        }
        SpawnPoint.gameObject.SetActive(false); // Hides spawn point in the scene
        StartCoroutine(PlayerBehaviour.Instance.Spawn(SpawnPoint.position)); // Calls the spawn coroutine from PlayerBehaviour
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameManager.Instance.GameRunning = false;

            // Activate the UI when player enters the checkpoint
            ActivateNextLevelUI();
        }
    }

    private void ActivateNextLevelUI() {
        if (_nextLevelUI != null) {
            _nextLevelUI.SetActive(true);
        }

        if (_nextLevelButton != null) {
            _nextLevelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(LoadNextLevel);
        }
    }

    private void LoadNextLevel() {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
