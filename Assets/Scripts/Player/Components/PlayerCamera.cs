using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; 
    private CinemachineTransposer transposer;
    private CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField]
    private NoiseSettings noiseProfile;
    private PlayerMovement playerMovement;

    private float regularSize = 5;
    private float aimingSize = 7.5f;
    private bool isAiming = false;
    private float zoomSpeed = 10;
    private float aimingCameraXOffset = 2;

    void Awake() {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);

        if (brain == null) {
            Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        GameObject camera = new GameObject("PLAYER - Camera");
        
        virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
        transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = new Vector3(0, 1.5f, -1);
        perlin = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_NoiseProfile = noiseProfile;
        SetShake(0);

        virtualCamera.m_Lens.OrthographicSize = regularSize;

        playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate() {
        float currSize = virtualCamera.m_Lens.OrthographicSize;
        bool shouldZoom = isAiming ? (currSize <= aimingSize) : (currSize >= regularSize);

        if (shouldZoom) {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize, 
                isAiming ? aimingSize : regularSize,
                zoomSpeed * Time.fixedDeltaTime
            );
        }

        transposer.m_FollowOffset = new Vector3(
            Mathf.Lerp(
                transposer.m_FollowOffset.x, 
                playerMovement.IsFacingRight() ? aimingCameraXOffset : -aimingCameraXOffset,
                zoomSpeed * Time.fixedDeltaTime
            ),
            transposer.m_FollowOffset.y,
            transposer.m_FollowOffset.z
        );
    }

    public void UpdateAimingState(bool _isAiming) {
        isAiming = _isAiming;
    }

    public void SetShake(float intensity) {
        perlin.m_AmplitudeGain = intensity;
    }
}
