using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PingPongAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _stateName = "IDLE";
    [SerializeField] private float _speed = 1f;

    private int _stateHash;
    private bool _goingForward = true;
    private float _normalizedTime = 0f;
    private bool _hasValidState = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _stateHash = Animator.StringToHash(_stateName);
        _animator.speed = 0f; 

        if (!_animator.HasState(0, _stateHash))
        {
            Debug.LogError($"[PingPongAnimation] Состояние с именем '{_stateName}' не найдено на базовом слое Animator Controller!");
            _hasValidState = false;
        }
        else
        {
            _hasValidState = true;
        }
    }

    void Update()
    {
        if (!_hasValidState) return;

        if (_goingForward)
        {
            _normalizedTime += Time.deltaTime * _speed;
            if (_normalizedTime >= 1f)
            {
                _normalizedTime = 1f;
                _goingForward = false;
            }
        }
        else
        {
            _normalizedTime -= Time.deltaTime * _speed;
            if (_normalizedTime <= 0f)
            {
                _normalizedTime = 0f;
                _goingForward = true;
            }
        }

        _animator.Play(_stateHash, 0, _normalizedTime);
    }
}