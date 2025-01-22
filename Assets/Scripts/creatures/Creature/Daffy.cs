using UnityEngine;

public class Daffy : MonoBehaviour
{
    private GameManager _gameManager;
    SpriteRenderer _spr;
    Rigidbody2D _rb;
    BoxCollider2D _col;
    [SerializeField] public float _speed;
    [SerializeField] public float _distance;
    [SerializeField] private Vector2[] _patrolPointGizmo;
    [SerializeField] private float _limitDistanceToEdge = 0.1f;
    private Vector2[] _patrolPoints;
    private int _currentPatrolIndex = 0;
    private bool _returningPath;
    
    [SerializeField] Transform _Player;
    [SerializeField] Transform _CastDot;
    [SerializeField] float _agroRange;
    private bool _IsChasing = false;
    
    

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (_patrolPointGizmo != null)
        {
            foreach (Vector2 patrolPoint in _patrolPointGizmo)
            {
                if (patrolPoint != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector2(patrolPoint.x + transform.position.x, patrolPoint.y + transform.position.y) , .3f);
                }
            }
        }
    }
    #endif

    private void Awake() 
    {
        _spr = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    
        _patrolPoints = _patrolPointGizmo;
        for(int i = 0; i < _patrolPointGizmo.Length; i++)
            _patrolPoints[i] = new Vector2(_patrolPointGizmo[i].x + transform.position.x, 0 + transform.position.y);
    }

    private void Update()
    {
        _IsChasing = SeePlayer(_agroRange);

        if (_IsChasing)
            ChasePlayer();
        else
            Walk();
    }

    private void Walk()
    {
        int _nextPatrolIndex = _returningPath ? _currentPatrolIndex - 1 : _currentPatrolIndex + 1;

        if (_nextPatrolIndex >= _patrolPoints.Length || _nextPatrolIndex < 0)
            _returningPath = !_returningPath;

        _spr.flipX = !_returningPath;  

        float _distanceToEdgePoint = Vector2.Distance(transform.position, _patrolPoints[_currentPatrolIndex]);
        if (_distanceToEdgePoint < _limitDistanceToEdge)
        {
            _currentPatrolIndex = _nextPatrolIndex;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[_currentPatrolIndex],_speed * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        if (_Player.position.x < transform.position.x)
        {
            _spr.flipX = true;
        }
        else
        {
            _spr.flipX = false;
        }
        Vector3 _playerposx = new Vector3(
            _Player.position.x - Mathf.Sign(_Player.position.x - transform.position.x) * _distance, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, _playerposx, _speed * Time.deltaTime);
    }

    private bool SeePlayer(float _Distance)
    {
        bool val = false;
        float followDistance = _spr.flipX? -_Distance : _Distance;
        
        Vector2 pos = _CastDot.position + Vector3.right * followDistance; 
        RaycastHit2D hit = Physics2D.Linecast(_CastDot.position, pos, 1 << LayerMask.NameToLayer("Player"));

        if(hit.collider != null)
        {
            val = hit.collider.gameObject.CompareTag("Player");
            Debug.DrawLine(_CastDot.position, pos, Color.blue);
        }
        
        return val;
    }
}