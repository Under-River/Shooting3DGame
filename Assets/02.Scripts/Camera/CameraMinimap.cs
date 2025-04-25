using UnityEngine;

public class CameraMinimap : MonoBehaviour
{
    [SerializeField] private float _startSize = 20f;
    [SerializeField] private float _zoomMinusSize = 1f;
    [SerializeField] private float _zoomPlusSize = 1f;
    [SerializeField] private float _zoomMaxSize = 25f;
    [SerializeField] private float _zoomMinSize = 15f;
    [SerializeField] private float _zoomSpeed = 10f;
    private float _zoomSize;
    private Camera _camera;
    private void Start()
    {
        _zoomSize = _startSize;
        _camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ZoomOut();
        }
        else if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ZoomIn();
        }
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _zoomSize, Time.deltaTime * _zoomSpeed);
    }
    private void ZoomIn()
    {
        if(_zoomSize > _zoomMinSize)
        {
            _zoomSize -= _zoomMinusSize;
        }
    }
    private void ZoomOut()
    {
        if(_zoomSize < _zoomMaxSize)
        {
            _zoomSize += _zoomPlusSize;
        }
        
    }
}
