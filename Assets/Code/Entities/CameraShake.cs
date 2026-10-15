using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;

    [Header("Vibraciones Camara")]
    [SerializeField] private float _shakeX;
    [SerializeField] private float _shakeY;

    private void OnEnable()
    {
        GameEvents.OnPlayerHit += CameraMoveGenerate;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHit -= CameraMoveGenerate;
    }


    private void CameraMoveGenerate()
    {
        float velocityRandomX = Random.Range(-_shakeX, _shakeX);
        float velocityRandomY = Random.Range(-_shakeY, _shakeY);

        Vector2 velocityShake = new(velocityRandomX, velocityRandomY);

        _cinemachineImpulseSource.GenerateImpulse(velocityShake);
    }
}
