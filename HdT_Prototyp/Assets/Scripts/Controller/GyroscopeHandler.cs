//source: https://docs.unity3d.com/ScriptReference/Gyroscope.html

// Create a cube with camera vector names on the faces.
// Allow the device to show named faces as it is oriented.

using System.Collections;
using UnityEngine;

public class GyroscopeHandler : MonoBehaviour
{
    [SerializeField]
    private bool _transitionEnabled;

    [SerializeField]
    private ViewTransitionController _transitionController;

    private bool _gyroEnabled;
    private Gyroscope _gyro;

    private const float MIN_VALUE = 50f;

    private const float MAX_VALUE = 100f;
    
    //private GameObject _cameraContainer;
    //private Quaternion _rot;
    //private GUIStyle guiStyle = new GUIStyle();

    
    private float _phonePitch;



    void Start()
    {
        //_cameraContainer = new GameObject("Camera Container");
        //_cameraContainer.transform.position = transform.position;
        //transform.SetParent(_cameraContainer.transform);

        _gyroEnabled = EnableGyro();
    }


    private void Update()
    {
        if (_transitionEnabled && _gyroEnabled)
        {
            //adjust camera rotation to phone rotation;
            //transform.localRotation = _gyro.attitude * _rot;

            _phonePitch = GetPhonePitch();
            if(_transitionController.CurrentView == Views.arView && _phonePitch > MIN_VALUE && _phonePitch < MAX_VALUE)
            {
                StartCoroutine("switchUIMode", Views.vrView);
                //_UITransitionManager.Show2DUI();
            } else if (_transitionController.CurrentView == Views.vrView  && !(_phonePitch > MIN_VALUE && _phonePitch < MAX_VALUE))
            {
                //_UITransitionManager.Hide2DUI();
                StartCoroutine("switchUIMode", Views.arView);

            }
        }
    }

    //This is a legacy function, check out the UI section for other ways to create your UI
    /*void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        //GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + _gyro.rotationRate);
        if (_gyroEnabled)
        {
            guiStyle.fontSize = 40; //change the font size
            GUI.Label(new Rect(200, 300, 200, 40), "Gyro attitude" + _gyro.attitude, guiStyle);
            GUI.Label(new Rect(200, 400, 200, 40), "In Euler" + _gyro.attitude.eulerAngles, guiStyle);
            GUI.Label(new Rect(200, 500, 200, 40), "X Rotation" + _phonePitch, guiStyle);
            GUI.Label(new Rect(200, 600, 200, 40), "Y Rotation" + GetYRotation(), guiStyle);

            GUI.Label(new Rect(200, 700, 200, 40), "Z Rotation" + GetZRotation(), guiStyle);
            //GUI.Label(new Rect(200, 500, 200, 40), "X Rotation" + GetXRotation(), guiStyle);


            GUI.Label(new Rect(200, 800, 200, 40), "Gyro enabled : " + _gyro.enabled, guiStyle);
        }
    
    }*/


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

            //_cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            //_rot = new Quaternion(0, 0, 1, 0);
            _phonePitch = GetPhonePitch();


            return true;
        }
        return false;
    }

    float GetPhonePitch()
    {
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = GetDeviceRotation();
        Quaternion eliminationOfYZ = Quaternion.Inverse(
        Quaternion.FromToRotation(referenceRotation * Vector3.right,
                                deviceRotation * Vector3.right));
        Quaternion rotationX = eliminationOfYZ * deviceRotation;
        return rotationX.eulerAngles.x;
    }


    /*float GetZRotation()
    {
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = GetDeviceRotation();
        Quaternion eliminationOfXY = Quaternion.Inverse(
        Quaternion.FromToRotation(referenceRotation * Vector3.forward,
                                deviceRotation * Vector3.forward));
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        return rotationZ.eulerAngles.z;
    }

    float GetYRotation()
    {
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = GetDeviceRotation();
        Quaternion eliminationOfXZ = Quaternion.Inverse(
        Quaternion.FromToRotation(referenceRotation * Vector3.up,
                                deviceRotation * Vector3.up));
        Quaternion rotationY = eliminationOfXZ * deviceRotation;
        return rotationY.eulerAngles.y;
    }*/

   

    Quaternion GetDeviceRotation()
    {
        if(_gyro!= null)
        {
            return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * _gyro.attitude * new Quaternion(0, 0, 1, 0);
        }
        return new Quaternion();
    }



    private IEnumerator switchUIMode(Views newView)
    {
        yield return new WaitForSeconds(1.5f);

        if (newView == Views.vrView && _transitionController.CurrentView == Views.arView && _phonePitch > MIN_VALUE && _phonePitch < MAX_VALUE)
        {
            _transitionController.SwitchToVR();
        }
        else if (newView == Views.arView && _transitionController.CurrentView == Views.vrView && !(_phonePitch > MIN_VALUE && _phonePitch < MAX_VALUE))
        {
            _transitionController.SwitchToAR();
        }

    }
}
