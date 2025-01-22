 using UnityEngine;

public class DaffyV2 : MonoBehaviour
{
   [SerializeField] public float _speed;
   Vector2 _move;
   Rigidbody2D _rb;
   public Transform _player;
   [SerializeField] public float _detectionRadius;
   private Vector2 _randomDirec;
   private float _randomMoveTimer;
   public float _randomMoveInterval = 2f;
   public LayerMask obstacleMask;
   public float obstacleCheckDistance = 0.5f;
    void Start() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _randomMoveTimer = _randomMoveInterval;
        chooseRandomDirection();
    }
   void Update() 
   {
        float _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (_distanceToPlayer <= _detectionRadius)
            moveTowardsPlayer();
        else
            randomMovement();
   }

   void moveTowardsPlayer()
   {
        Vector2 direction = (_player.position - transform.position).normalized;
        if (!IsObstacleAhead(direction))
            _rb.velocity = direction * _speed;

        else
            _rb.velocity = Vector2.zero;
   }

   void randomMovement()
   {
        _randomMoveTimer -= Time.deltaTime;
        if (_randomMoveTimer <=0 )
        {
            chooseRandomDirection();
            _randomMoveTimer = _randomMoveInterval;
        }
        
        transform.position += (Vector3)(_randomDirec * _speed * Time.deltaTime);
   }

   void chooseRandomDirection()
   {
        float _randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _randomDirec = new Vector2(Mathf.Cos(_randomAngle), Mathf.Sin(_randomAngle));
   }

   bool IsObstacleAhead(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleCheckDistance, obstacleMask);
        return hit.collider != null;
    }

   void OnDrawGizmosSelected() 
   {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _detectionRadius);
   }
}
