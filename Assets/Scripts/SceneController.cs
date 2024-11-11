using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    public Transform SpawnPoint;
    [SerializeField] private string _nextLevel;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint.gameObject.SetActive(false);
        //StartCoroutine(PlayerBehaviour.Instance.Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
