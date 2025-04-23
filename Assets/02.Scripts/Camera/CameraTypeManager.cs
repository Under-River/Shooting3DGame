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
        _cam.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
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
            _cam.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
        }
        else if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            _cameraType = CameraType.TPS;
            _cam.cullingMask = -1;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            _cameraType = CameraType.QuarterView;
            _cam.cullingMask = -1;
        }
    }
}
