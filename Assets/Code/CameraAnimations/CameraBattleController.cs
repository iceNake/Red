using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraBattleController : MonoBehaviour
{
    [Header("Walking Camera")]
    [SerializeField] private float walkOrthoSize = 8f;
    [SerializeField] private float transitionDuration = 0.8f;

    [Header("Battle Camera")]
    [SerializeField] private float wallMarginPadding = 1.5f;
    [SerializeField] private float battleYOffset = 1.5f;

    private CinemachineCamera _cam;
    private CinemachinePositionComposer _positionComposer;
    private Transform _playerTransform;
    private Transform _arenaAnchor;
    private Coroutine _transitionCoroutine;
    
    private Vector2 _defaultScreenOffset;
    private Vector2 _defaultDeadZoneSize;

    private void Awake()
    {
        _cam = GetComponent<CinemachineCamera>();
        _positionComposer = GetComponent<CinemachinePositionComposer>();
        
        if (_positionComposer != null)
        {
            _defaultScreenOffset = _positionComposer.Composition.ScreenPosition;
            _defaultDeadZoneSize = _positionComposer.Composition.DeadZone.Size;
        }
        
        GameObject anchorObj = new GameObject("CameraBattleAnchor");
        _arenaAnchor = anchorObj.transform;
    }

    private void OnEnable()
    {
        GameEvents.OnEnterCombatCamera += EnterCombatMode;
        GameEvents.OnExitCombatCamera += ExitCombatMode;
    }

    private void OnDisable()
    {
        GameEvents.OnEnterCombatCamera -= EnterCombatMode;
        GameEvents.OnExitCombatCamera -= ExitCombatMode;
    }

    private void EnterCombatMode(Vector3 leftWallPos, Vector3 rightWallPos)
    {
        if (_cam.Follow != null && _cam.Follow != _arenaAnchor) _playerTransform = _cam.Follow;
        
        if (_positionComposer != null)
        {
            var comp = _positionComposer.Composition;
            comp.ScreenPosition = Vector2.zero;
            comp.DeadZone.Size = Vector2.zero;
            _positionComposer.Composition = comp;
        }
        
        Vector3 centerPos = (leftWallPos + rightWallPos) * 0.5f;
        centerPos.y += battleYOffset;
        centerPos.z = 0f;
        
        float totalWidth = Mathf.Abs(rightWallPos.x - leftWallPos.x) + (wallMarginPadding * 2f);
        float aspect = Camera.main != null ? Camera.main.aspect : (16f / 9f);
        float targetOrthoSize = Mathf.Max((totalWidth * 0.5f) / aspect, walkOrthoSize);
        
        _arenaAnchor.position = centerPos;
        _cam.Follow = _arenaAnchor;

        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        
        _transitionCoroutine = StartCoroutine(SmoothZoomRoutine(targetOrthoSize));
    }

    private void ExitCombatMode()
    {
        if (_positionComposer != null)
        {
            var comp = _positionComposer.Composition;
            comp.ScreenPosition = _defaultScreenOffset;
            comp.DeadZone.Size = _defaultDeadZoneSize;
            _positionComposer.Composition = comp;
        }
        
        if (_playerTransform != null) _cam.Follow = _playerTransform;
        
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        
        _transitionCoroutine = StartCoroutine(SmoothZoomRoutine(walkOrthoSize));
    }

    private IEnumerator SmoothZoomRoutine(float targetSize)
    {
        float startSize = _cam.Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            float smoothT = t * t * (3f - 2f * t);

            _cam.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, smoothT);
            yield return null;
        }

        _cam.Lens.OrthographicSize = targetSize;
        _transitionCoroutine = null;
    }
}
