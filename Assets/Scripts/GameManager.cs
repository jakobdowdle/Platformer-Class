using UnityEngine;

public class GameManager : MonoBehaviour {
    [HideInInspector] public bool GameRunning;
    public static GameManager Instance;

    // Start is called before the first frame update
    void Start() {
        GameRunning = false;
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        // Initialize the game state
        GameRunning = true;
    }

    // Example of a method to restart the level or manage game state
    public void RestartLevel() {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }

    public void EndGame() {
        GameRunning = false;
        Debug.Log("Game Over!");
    }
}
