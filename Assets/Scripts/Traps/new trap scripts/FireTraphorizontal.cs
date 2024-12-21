using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireTraphorizontal : MonoBehaviour
{
    BoxCollider2D _col;

    bool _isInCooldown = false;

    [SerializeField]bool _activetrap;
    [SerializeField]float _timer;

    void Start() 
    {

        _col = GetComponent<BoxCollider2D>();

    }

    void Update()
    {

        if (!_activetrap)
            return;
        if (_isInCooldown)
            return;

        Debug.Log("ativando trap");
        StartCoroutine(CdDelay());
        

    }

    IEnumerator CdDelay()
    {

        _col.enabled = true;
        _isInCooldown = true;
        yield return new WaitForSeconds (_timer);
        _col.enabled = false;
        yield return new WaitForSeconds (_timer);
        _isInCooldown = false;

    }
}
