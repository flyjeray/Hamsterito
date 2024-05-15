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
    private CinemachineConfiner confiner;
    private CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField]
    private NoiseSettings noiseProfile;
    private PlayerAttacking playerAttacking;
    private Player player;
    [SerializeField]
    private Collider2D levelCameraBounds;

    [SerializeField]
    private float regularSize = 5;
    [SerializeField]
    private float aimingSize = 10;
    private bool isAiming = false;
    private float restingCameraYOffset = 1.5f;
    private bool disableAimingZooming = false;

    void Awake() {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);

        if (brain == null) {
            Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        GameObject camera = new GameObject("PLAYER - Camera");
        
        virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
        transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = new Vector3(0, restingCameraYOffset, -1);
        confiner = camera.AddComponent<CinemachineConfiner>();
        confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
        confiner.m_BoundingShape2D = levelCameraBounds;
        virtualCamera.AddExtension(confiner);
        perlin = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_NoiseProfile = noiseProfile;
        SetShake(0);

        virtualCamera.m_Lens.OrthographicSize = regularSize;

        playerAttacking = GetComponent<PlayerAttacking>();
        player = GetComponent<Player>();
    }

    public void UpdateAimingState(bool _isAiming) {
        if (player.IsActive()) {
            isAiming = _isAiming;
            virtualCamera.m_Lens.OrthographicSize = isAiming ? aimingSize : regularSize;
            transposer.m_FollowOffset = new Vector3(
                0,
                playerAttacking.IsAiming() ? 0 : restingCameraYOffset,
                transposer.m_FollowOffset.z
            );
        }
    }

    public void SetShake(float intensity) {
        perlin.m_AmplitudeGain = intensity;
    }

    public void ZoomOnDeath() {
        transposer.m_XDamping = 0;
        transposer.m_YDamping = 0;
        transposer.m_ZDamping = 0;
        virtualCamera.m_Lens.OrthographicSize = 1;
    }
}
