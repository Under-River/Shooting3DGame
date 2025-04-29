using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum CameraType
{
    FPS,
    TPS,
    QuarterView,
}
public class CameraTypeManager : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;
    private Camera _cam;
    private int _defaultCullingMask;
    private int _fpsCullingMask;
    private int _tpsCullingMask;
    private int _miniMapCullingMask;    

    public CameraType CameraType => _cameraType;
    public static CameraTypeManager Instance;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }
    private void Start()
    {
        _cameraType = CameraType.FPS;
        _fpsCullingMask = LayerMask.NameToLayer("FPS");
        _tpsCullingMask = LayerMask.NameToLayer("TPS");
        _miniMapCullingMask = LayerMask.NameToLayer("Minimap");
        _defaultCullingMask = _cam.cullingMask;
        _cam.cullingMask = _defaultCullingMask & ~((1 << _tpsCullingMask | 1 << _miniMapCullingMask));
    }
    private void Update()
    {
        SwitchFollowType();
    }

    private void SwitchFollowType()
    {
        if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            _cameraType = CameraType.FPS;
            _cam.cullingMask = _defaultCullingMask & ~((1 << _tpsCullingMask | 1 << _miniMapCullingMask));
        }
        else if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            _cameraType = CameraType.TPS;
            _cam.cullingMask = _defaultCullingMask & ~((1 << _fpsCullingMask | 1 << _miniMapCullingMask));
        }
        else if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            _cameraType = CameraType.QuarterView;
            _cam.cullingMask = _defaultCullingMask & ~(1 << _miniMapCullingMask);
        }
    }
}
