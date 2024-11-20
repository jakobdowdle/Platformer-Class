using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehaviour : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            animator.SetBool("Despawn", true);
            StartCoroutine(WaitForDespawnAnimation());
        }
    }

    private IEnumerator WaitForDespawnAnimation() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        Destroy(gameObject);
    }
}
