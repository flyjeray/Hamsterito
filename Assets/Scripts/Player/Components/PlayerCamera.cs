using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; 

    void Awake() {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);

        if (brain == null) {
            Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        GameObject camera = new GameObject("PLAYER - Camera");
        
        virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
        virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
    }
}
