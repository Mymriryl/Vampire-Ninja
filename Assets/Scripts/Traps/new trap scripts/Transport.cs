using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{

     Rigidbody2D _rb;
     public float _speed = 5f;

     public Transform _direction;

     void Start()
     {

     }
     void Update() 
     {
          transform.position = Vector2.MoveTowards(transform.position, _direction.position, _speed * Time.deltaTime);
    
     }
     private void OnCollisionEnter2D(Collision2D _coll) 
     {
          _coll.transform.SetParent(transform);
     }
     private void OnCollisionExit2D(Collision2D _coll) 
     {
          _coll.transform.SetParent(null);
     }
}
