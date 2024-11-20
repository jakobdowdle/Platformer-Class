using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    [HideInInspector] public bool GameRunning;
    [SerializeField] private GameObject _fruits;
    [SerializeField] private TextMeshProUGUI _collectionVisual;
    private int _levelFruitCount;
    private int _collectedFruitCount;
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

        _collectedFruitCount = 0;
        _levelFruitCount = CountLevelFruits();
        PlayerBehaviour.Instance.LevelStart(_levelFruitCount);
        Debug.Log("Number of fruits: " + _levelFruitCount);
    }

    private void Update() {
        SetFruitUI();
    }

    public void EndGame() {
        GameRunning = false;
        Debug.Log("Game Over!");
    }

    // Method to count the number of child objects (fruits)
    private int CountLevelFruits() {
        if (_fruits != null) {
            return _fruits.transform.childCount;
        } else {
            Debug.LogWarning("_fruits GameObject is not assigned!");
            return 0;
        }
    }

    public void SetFruitUI() {
        _collectionVisual.text = _collectedFruitCount + "/" + _levelFruitCount;
    }

    public void IncreaseFruitCount() {
        _collectedFruitCount++;
    }
}
