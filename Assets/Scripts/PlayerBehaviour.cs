using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    public static PlayerBehaviour Instance;
    public Transform playerTransform; // Reference to the player's transform
    private bool _isDead;
    private int _fruitsCollected;
    private int _totalFruitsEncountered;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this); // Important for scene transfer
        _fruitsCollected = 0;
        _totalFruitsEncountered = 0;
    }

    void Start() {
        _fruitsCollected = 0;
        _totalFruitsEncountered = 0;
        playerTransform = transform; 
    }
    public void LevelStart(int fruitsInLevel) {
        _totalFruitsEncountered += fruitsInLevel;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Fruit")) {
            _fruitsCollected += 1;
            GameManager.Instance.IncreaseFruitCount();
        }
    }

    // Coroutine to handle player respawning
    public IEnumerator Spawn(Vector3 spawnLocation) {
        playerTransform.position = spawnLocation;

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameRunning = true;

    }
}
