using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment snakeSegmentPrefab;

    private SnakeSegment _head, _tail;
    private GameController _game;
    private PlayerInputController _inputController;
    private Vector2 _currInput;
    private int _snakeLength;
    private float _clock, _snakeSpeed;

    public void Init(Vector2 startPos, Vector2 startDir, int startLength, float snakeSpeed)
    {
        // assert that the snake can have at least a head and a tail
        Assert.IsTrue(startLength > 1);
        _head = CreateSegment(null, null, startPos, startDir, _snakeLength);
        _tail = _head;
        _snakeLength = 1;
        
        // grow potentially several times depending on start config
        for (int segmentsLeft = startLength - 1; segmentsLeft > 0; --segmentsLeft) Grow();

        _snakeSpeed = snakeSpeed;
        _currInput = Vector2.zero;
    }

    public Vector2 CurrentInput() { return _currInput; }

    public void Grow()
    {
        // spawn the new segment in the correct location
        Vector2 newPos = _tail.pos - _tail.dir;
        SnakeSegment newSegment = CreateSegment(_tail, null, newPos, _tail.dir, _snakeLength);
        // adjust the references to neighboring segments
        _tail.prev = newSegment;
        newSegment.next = _tail;
        _tail = newSegment;
        // track change to length
        ++_snakeLength;
    }

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        _clock = 0.0f;
    }

    void Update()
    {
        _clock += Time.deltaTime;
        // duration between move steps, one unit at a time
        float timeToMoveStep = 1 / _snakeSpeed;
        // (0, 1) interval representing how far through the move step it currently is
        float moveProgress = _clock * _snakeSpeed;
        // interpolate the sprites forward based on direction and progress until next step
        Animate(moveProgress);

        if (_clock >= timeToMoveStep)
        {
            MoveStep();
        }
    }

    void MoveStep()
    {
        _clock = 0.0f;
        Vector2 tempDir = _currInput;

        // walk down the snake
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            // move the segment and propagate it's direction to the next one
            tempDir = curr.Move(tempDir);
            // trail sprite update behind move update by an extra segment
            // this is so that both neighbors of the updating segment have already moved
            // misses the head but that should never need to update anyway
            if (curr == _head) continue;
            curr.next.UpdateSprite();
        }

        // reset the interpolation on all of the sprites
        Animate(0);
        // get the input for the next step 
        _currInput = _inputController.queuedInput;
    }

    void Animate(float t)
    {
        Vector2 tempDir = _currInput;

        // walk down the snake
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            tempDir = curr.Interpolate(tempDir, t);
        }
    }

    SnakeSegment CreateSegment(SnakeSegment next, SnakeSegment prev, Vector2 startPos, Vector2 startDir, int index)
    {
        // instantiate a new segment
        var newSegmentObject = Instantiate(snakeSegmentPrefab);
        // name it according to the length of the snake at creation time
        newSegmentObject.name = $"Segment {index}";
        // get the controller for the new segment, initialize it and return
        SnakeSegment newSegment = newSegmentObject.GetComponent<SnakeSegment>();
        newSegment.Init(null, null, startPos, startDir, index);
        return newSegment;
    }

    [ContextMenu("log directions")]
    void LogDir()
    {
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            Debug.Log("segment " + curr.index + " has direction " + curr.dir);
        }
    }
}
