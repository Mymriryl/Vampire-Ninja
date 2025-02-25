using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;

    private void Awake()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.enabled = Application.isEditor;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player"))
            return;
            
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.RegisterNewCheckpoint(checkpointIndex, transform.position);
    }
}
