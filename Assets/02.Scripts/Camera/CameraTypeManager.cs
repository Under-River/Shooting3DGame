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
    public CameraType CameraType => _cameraType;

    public static CameraTypeManager Instance;

    private void Awake()
    {
        Instance = this;
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
        }
        else if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            _cameraType = CameraType.TPS;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            _cameraType = CameraType.QuarterView;
        }
    }
}
