using UnityEngine;

public class RotateHoverUtil : MonoBehaviour
{
    [SerializeField] private bool _usesLocalTime = true;
    private float _localTime;

    [SerializeField] private bool _canRotate;
    [SerializeField] private bool _canRotateBackAndForth;
    [SerializeField] private bool _canHover;
    [SerializeField] private Vector3 _degreesPerSecond = new Vector3(0f, 0f, 15f);
    
    [SerializeField] private float _amplitude = 0.5f;
    public float Amplitude => _amplitude;
    [SerializeField] private float _frequency = 1f;

    private Vector3 _posOffset = new Vector3();
    private Vector3 _tempPos = new Vector3();

    public float Sine { get; private set; }

    private void Awake()
    {
        _posOffset = transform.localPosition;
        _localTime = Random.Range(0, 1000);
        _frequency = Random.Range(_frequency, _frequency + 0.25f);
    }

    private void Update()
    {
        if (_usesLocalTime)
        {
            _localTime += Time.deltaTime;
        }
        else
        {
            _localTime = Time.fixedTime;
        }

        if (_canRotateBackAndForth)
        {
            float rotationDirection = _canRotateBackAndForth && Mathf.PingPong(_localTime, 1f) < 0.5f ? -1 : 1;
            transform.Rotate(Time.deltaTime * _degreesPerSecond * rotationDirection, Space.Self);
        }
        
        if (_canRotate)
        {
            transform.Rotate(Time.deltaTime * _degreesPerSecond, Space.Self);
        }
        
        if (_canHover)
        {
            _tempPos = _posOffset;

            Sine = Mathf.Sin(_localTime * Mathf.PI * _frequency);
            _tempPos.y += (Sine * _amplitude);

            transform.localPosition = _tempPos;
        }
    }
}