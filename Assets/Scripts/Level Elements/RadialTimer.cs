using System;
using System.Collections;
using UnityEngine;

public class RadialTimer : MonoBehaviour
{
    [SerializeField] private float _totalTime = 5f; // Total time for the timer
    [SerializeField] private Color _timerColor = Color.green; // Color of the timer
    [SerializeField] private int _segments = 25; // Number of segments for the circle
    [SerializeField] private float _circleRadius = 1f; // Radius of the circle
    [SerializeField] private float _lineWidth = 0.1f; // Width of the timer line
    [SerializeField] private bool _startOnAwake;

    private LineRenderer _lr;
    private Switch _sw;
    private Flipper _fl;
    private float _timeLeft;

    private void Awake()
    {
        _lr = gameObject.AddComponent<LineRenderer>();
        _sw = GetComponent<Switch>();
        _fl = GetComponent<Flipper>();
    }

    private void Start()
    {
        if (_startOnAwake)
        {
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        
        _lr.useWorldSpace = false;
        
        _lr.startWidth = _lineWidth;
        _lr.endWidth = _lineWidth;

        _lr.material = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>().material : new Material(Shader.Find("Sprites/Default"));
        
        _lr.startColor = _timerColor;
        _lr.endColor = _timerColor;

        _timeLeft = _totalTime;

        _lr.positionCount = 0;
        
        CreateCircle();
        
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        _lr.positionCount = 0;
    }

    private IEnumerator UpdateTimer()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;

            // Calculate percentage of time remaining
            float timePercent = _timeLeft / _totalTime;

            // Calculate how many segments to show based on remaining time
            int segmentsToShow = Mathf.RoundToInt(_segments * timePercent);

            // Update the line renderer's positions
            UpdateCircle(segmentsToShow);
            
            yield return null;
        }

        // Timer expired
        _sw.TimerEnded();
        _fl.TimerEnded();
    }

    // Create circle vertices
    private void CreateCircle()
    {
        Vector3[] positions = new Vector3[_segments];

        float angle = 360f / _segments;

        for (int i = 0; i < _segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle * i) * _circleRadius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle * i) * _circleRadius;
            positions[i] = new Vector3(x, y, 0);
        }

        _lr.positionCount = _segments;
        _lr.SetPositions(positions);
    }

    // Update circle vertices to show remaining time
    private void UpdateCircle(int segmentsToShow)
    {
        _lr.positionCount = segmentsToShow;
    }
}
