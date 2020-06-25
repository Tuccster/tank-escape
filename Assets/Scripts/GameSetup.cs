using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public bool _trackPlayerOnStart = true;

    private void Start()
    {
        // Bind the camera controller to the player tank if there is one
        if (Registry.GetAmountOfType<TankPlayer>() > 0 && _trackPlayerOnStart)
        {
            TankPlayer tankPlayer = Registry.GetRegister<TankPlayer>("player");
            CameraController cameraController = Registry.GetRegister<CameraController>("camera_controller");
            cameraController.SetTarget(tankPlayer.GetComponent<Transform>());
            TankPlayer.onRotateEvent += cameraController.SetRotation;
        }
    }
}
