using System.Collections;
using UnityEngine;

public class OneWayTest : MonoBehaviour
{
    private CapsuleCollider2D _playerCol;
    private BoxCollider2D _platformCol;
    [SerializeField] private float _delayToReturnCollision = 0.1f;

    void Start()
    {
        _playerCol = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
        _platformCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float bottomPlayerPos = _playerCol.bounds.center.y - _playerCol.bounds.extents.y; // Player's feet position
        float topPlatformPos = _platformCol.bounds.center.y - _platformCol.bounds.extents.y;
        bool touchingPlatform = Physics2D.BoxCast(_platformCol.bounds.center, _platformCol.bounds.size, 0f, Vector2.up, .1f, LayerMask.GetMask("Player"));
        bool onTopOfPlatform = touchingPlatform && bottomPlayerPos > topPlatformPos;

        if (!onTopOfPlatform)
            return;

        if (PlayerInputManager.FrameInput.Move.y < 0)
        {
            StartCoroutine(DeactivatePlatformCollider());
        }
    }

    IEnumerator DeactivatePlatformCollider()
    {
        _platformCol.enabled = false;

        // Wait for the player to move away from the platform
        yield return new WaitUntil(() =>
        {
            float bottomPlayerPos = _playerCol.bounds.center.y - _playerCol.bounds.extents.y;
            float topPlatformPos = _platformCol.bounds.center.y - _platformCol.bounds.extents.y;
            return bottomPlayerPos < topPlatformPos;
        });

       yield return new WaitForSeconds(_delayToReturnCollision);// Small delay to ensure smooth transition

        _platformCol.enabled = true;
    }
}