using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum SegmentType
{
    None = 0,
    Head = 1,
    Body = 2,
    TurnRight = 3,
    TurnLeft = 4,
    Tail = 5
};

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment next, prev;
    public Vector2 pos, dir;
    public int index;
    public SegmentType segmentType;
    public SnakeSpriteController spriteController;
    
    private GameController _game;
    private Rigidbody2D _rb;

    // ---------------------------- initializer ---------------------------------------

    public void Init(SnakeSegment next, SnakeSegment prev, Vector2 pos, Vector2 dir, int index)
    {
        this.next = next;
        this.prev = prev;
        this.pos = pos;
        this.dir = dir;
        this.index = index;

        MoveAndRotate(pos, dir);
    }

    // ------------------------ main update methods -------------------------------------

    // move the snake segment one unit in a given direction
    // rotates the segment to face to new direction
    // returns the direction the segment was facing before moving
    public Vector2 Move(Vector2 direction)
    {
        Vector2 lastDir = dir;
        dir = direction;
        pos += direction;
        if (OutOfBounds(pos)) _game.EndGame();
        MoveAndRotate(pos, dir);
        return lastDir;
    }

    // update bookkeeping for type segment and associated sprite based on movement of neighbors
    public void UpdateSegment()
    {
        // no orphaned segments allowed
        Assert.IsTrue(next != null || prev != null);
        // copy the type of the current segment to check if it changed later
        SegmentType currType = segmentType;
        
        // determine the correct segment type based on neighboring segments
        if (next == null) segmentType = SegmentType.Head;
        else if (prev == null) segmentType = SegmentType.Tail;
        else
        {
            float angle = Vector2.SignedAngle(prev.pos - pos, next.pos - pos);
            if (angle == -90) segmentType = SegmentType.TurnLeft;
            else if (angle == 90) segmentType = SegmentType.TurnRight;
            else segmentType = SegmentType.Body;
        }

        // stop here if the segment type didn't actually change
        if (currType == segmentType) return;

        spriteController.SetSprite(segmentType);
    }

    private void MoveAndRotate(Vector2 pos, Vector2 dir)
    {
        // rotation is relative to the x-axis
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        _rb.MoveRotation(Quaternion.Euler(0.0f, 0.0f, angle));
        _rb.MovePosition(new Vector3(pos.x, pos.y, 0.0f));
    }

    // ------------------------ sprite management ------------------------------------------

    // handle sprite animation for any frame partially between move steps
    public Vector2 Animate(Vector2 direction, float t)
    {
        // do not animate corners
        if (IsCorner()) return dir;
        // clamp animation for body segments next to corners
        if (IsBody() && (next.IsCorner() || prev.IsCorner())) t = Mathf.Min(0.5f, t);

        // only translate if direction didn't change
        if (direction == dir) TranslateSprite(t);
        else RotateSprite(direction, t);

        return dir;
    }
    
    public void ResetSprite() { spriteController.ResetSpriteTransform(); }

    private void RotateSprite(Vector2 direction, float t)
    {
        // angle is relative to the direction of the segment
        float angle = Vector2.SignedAngle(dir, direction) * t;
        spriteController.RotateSprite(angle);
    }

    private void TranslateSprite(float t) {
        // position is in absolute terms, a relative position would rotate with the segment
        Vector2 lerp = pos + dir * t;
        spriteController.TranslateSprite(lerp);
    }

    // ------------------------ lifecycle methods ----------------------------

    void Start() { UpdateSegment(); }

    void Awake()
    { 
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // make sure the game isn't paused
        if (Time.timeScale == 0) return; 
        if (collider.CompareTag("Player"))
        {
            Debug.Log("snake collided, ending game");

            _game.EndGame();
        }
    }

    // -------------------------- helpers -----------------------

    private bool IsCorner() { return segmentType == SegmentType.TurnRight || segmentType == SegmentType.TurnLeft; }
    private bool IsBody() { return segmentType == SegmentType.Body; }

    private bool OutOfBounds(Vector2 pos)
    {
        Vector2 levelSize = _game.GetLevelSize();
        return (pos.x < 0 || pos.x >= levelSize.x || pos.y < 0 || pos.y >= levelSize.y);
    }
}