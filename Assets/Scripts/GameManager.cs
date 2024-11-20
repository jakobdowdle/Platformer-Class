using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    [HideInInspector] public bool GameRunning;
    [SerializeField] private GameObject _fruits;
    [SerializeField] private TextMeshProUGUI _collectionVisual;
    [SerializeField] private TextMeshProUGUI _totalVisual;
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

        if (_fruits != null) {
            _collectedFruitCount = 0;
            _levelFruitCount = CountLevelFruits();
            PlayerBehaviour.Instance.LevelStart(_levelFruitCount);
        }

        if (_totalVisual != null) {
            _totalVisual.text = PlayerBehaviour.Instance.SendCollectedTotal() + "/" + PlayerBehaviour.Instance.SendEncounteredTotal();
        }
    }

    private void Update() {
        if (_collectionVisual != null) {
            SetFruitUI();
        }
    }

    public void EndGame() {
        GameRunning = false;
        Debug.Log("Game Over!");
    }

    // Method to count the number of child objects (fruits)
    public int CountLevelFruits() {
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

    public int GetCollectedFruit() {
        return _collectedFruitCount;
    }

    public int GetTotalLevelFruits() {
        return _levelFruitCount;
    }

    public void IncreaseFruitCount() {
        _collectedFruitCount++;
    }
}
