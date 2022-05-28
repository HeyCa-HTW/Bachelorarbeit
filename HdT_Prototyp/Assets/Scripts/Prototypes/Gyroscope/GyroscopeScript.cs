//source: https://docs.unity3d.com/ScriptReference/Gyroscope.html

// Create a cube with camera vector names on the faces.
// Allow the device to show named faces as it is oriented.

using UnityEngine;

public class GyroscopeScript : MonoBehaviour
{
    private Gyroscope _gyro;
    private bool _gyroEnabled;
    private GameObject _cameraContainer;
    private Quaternion _rot;
    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        _cameraContainer = new GameObject("Camera Container");
        _cameraContainer.transform.position = transform.position;
        transform.SetParent(_cameraContainer.transform);

        _gyroEnabled = EnableGyro();


        
    }

    private void Update()
    {
        if (_gyroEnabled)
        {
            transform.localRotation = _gyro.attitude * _rot;
        }
    }

    //This is a legacy function, check out the UI section for other ways to create your UI
    void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        //GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + _gyro.rotationRate);
        if (_gyroEnabled)
        {
            guiStyle.fontSize = 40; //change the font size
            GUI.Label(new Rect(500, 300, 200, 40), "Gyro attitude" + _gyro.attitude, guiStyle);
            GUI.Label(new Rect(500, 400, 200, 40), "In Euler" + _gyro.attitude.eulerAngles, guiStyle);
            GUI.Label(new Rect(500, 500, 200, 40), "Gyro enabled : " + _gyro.enabled, guiStyle);
            

        }

    }


    /// <summary>
    /// activates the gyroscope, if the system has one
    /// </summary>
    /// <returns>true, if the gyroscope has been successfully activated</returns>
    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _gyro = Input.gyro;
            _gyro.enabled = true;

            _cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            _rot = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }
}
