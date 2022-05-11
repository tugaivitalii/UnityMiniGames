using UnityEngine;
public class MouseLook : MonoBehaviour
{
    #region PUBLIC FIELDS

    [Header("Mouse Lock")]
    public bool isMouseLocked;

    [Header("Camera Field of View")]
    public bool isFieldOfViewEnabled;
    public float cameraFieldOfViewMin;
    public float cameraFieldOfViewMax;
    public float fieldOfViewIncrement;

    [Header("Camera Rotate X Settings")]
    public float cameraRotateXMin;
    public float cameraRotateXMax;

    [Header("Mouse Smoothing")]
    public float mouseSmooth;
    #endregion

    #region PRIVATE FIELDS

    private float m_mouseX;
    private float m_mouseY;
    private float m_rotateX;
    private float m_mouseScroll;
    private Transform m_parent;
    private float m_fieldOfView;
    private Camera m_camera;

    #endregion

    #region UNITY ROUTINES

    private void Awake()
    {
        #region Initialize Fields

        m_camera = Camera.main;
        m_parent = transform.parent;
        if (m_camera != null)
        {
            m_fieldOfView = m_camera.fieldOfView;
        }

        #endregion
    }

    private void Update()
    {
        MouseInputs();
        RotatePlayerY();
        CameraRotateX();
        CameraZoom();
    }

    #endregion

    #region HELPER ROUTINES

    private void MouseInputs()
    {
        #region Collect Mouse Inputs

        m_mouseX = Input.GetAxis("Mouse X") * mouseSmooth;
        m_mouseY = Input.GetAxis("Mouse Y") * mouseSmooth;
        m_mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        MouseLock();

        #endregion
    }

    private void RotatePlayerY()
    {
        #region  Rotate Player

        m_parent.Rotate(Vector3.up * m_mouseX);

        #endregion
    }

    private void CameraRotateX()
    {
        #region Rotate Camera X Axis

        m_rotateX += m_mouseY;
        m_rotateX = Mathf.Clamp(m_rotateX, cameraRotateXMin, cameraRotateXMax);
        m_camera.transform.localRotation = Quaternion.Euler(-m_rotateX, 0f, 0f);

        #endregion
    }

    private void MouseLock()
    {
        #region Lock Mouse Cursor

        if (isMouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        #endregion
    }

    private void CameraZoom()
    {
        if (isFieldOfViewEnabled)
        {
            if (m_mouseScroll > 0.0f)
            {
                if (m_fieldOfView + fieldOfViewIncrement >= cameraFieldOfViewMin &&
                    m_fieldOfView + fieldOfViewIncrement <= cameraFieldOfViewMax)
                {
                    m_fieldOfView += fieldOfViewIncrement;
                    m_camera.fieldOfView = m_fieldOfView;
                }
            }

            if (m_mouseScroll < 0.0f)
            {
                if (m_fieldOfView - fieldOfViewIncrement >= cameraFieldOfViewMin &&
                    m_fieldOfView - fieldOfViewIncrement <= cameraFieldOfViewMax)
                {
                    m_fieldOfView -= fieldOfViewIncrement;
                    m_camera.fieldOfView = m_fieldOfView;
                }
            }
        }
    }

    #endregion
}