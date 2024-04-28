using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; 
    private CinemachineTransposer transposer;

    private float regularSize = 5;
    private float aimingSize = 7.5f;
    private bool isAiming = false;
    private float zoomSpeed = 24;

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

        virtualCamera.m_Lens.OrthographicSize = regularSize;
    }

    void FixedUpdate() {
        float currSize = virtualCamera.m_Lens.OrthographicSize;
        bool shouldZoom = isAiming ? (currSize <= aimingSize) : (currSize >= regularSize);
        if (shouldZoom) {
            virtualCamera.m_Lens.OrthographicSize += (isAiming ? 1 : -1) * zoomSpeed * Time.fixedDeltaTime;
        }
    }

    public void UpdateAimingState(bool _isAiming) {
        isAiming = _isAiming;
    }
}
