using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Transform _transform;
    private Transform _target;

    private IEnumerator _followTarget;

    private void Awake()
    {
        _camera = transform.GetChild(0).GetComponent<Camera>();
        _transform = gameObject.GetComponent<Transform>();

        Registry.AddRegister("camera", _camera);
        Registry.AddRegister("camera_controller", this);

        _followTarget = FollowTarget();
    }

    private void Start()
    {
        //_targetPosition = Registry.GetRegister<TankPlayer>("player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float scrollValue = Input.GetAxis("Mouse ScrollWheel");
            if (scrollValue > 0f) _camera.transform.position += _camera.transform.forward * 1;
            if (scrollValue < 0f) _camera.transform.position += _camera.transform.forward * -1;
        }
    }

    public void SetTarget(Transform target)
    {
        StopCoroutine(_followTarget);
        _target = target;
        StartCoroutine(_followTarget);
    }

    private IEnumerator FollowTarget()
    {   
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (_target != null)
        {
            _transform.position = _target.position;
            yield return delay;
        }
    }

    public void SetRotation(Quaternion rotation)
    {
        _transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }
}
