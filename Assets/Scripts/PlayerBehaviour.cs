using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    public static PlayerBehaviour Instance;
    public Transform playerTransform; // Reference to the player's transform
    private bool _isDead;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this); // Important for scene transfer
    }

    void Start() {
        // Initialize the player or other necessary setups
        playerTransform = transform; // Assuming this script is attached to the player
    }

    // Coroutine to handle player respawning
    public IEnumerator Spawn(Vector3 spawnLocation) {
        playerTransform.position = spawnLocation;

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameRunning = true;

    }

    // Add any other logic for the player as needed
}
