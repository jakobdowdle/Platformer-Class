using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Instance;
    public Animator SpriteAnimator, EffectsAnimator;
    private bool _isDead;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this); //Important for scene transfer.
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
