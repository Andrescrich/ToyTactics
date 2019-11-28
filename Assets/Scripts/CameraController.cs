using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody cuerpo;
    [SerializeField]
    float sensibilidad;
    [SerializeField]
    float sensibilidadZoom;
    [SerializeField]
    float maxX;
    [SerializeField]
    float maxZ;

    Transform transformCamara;
    Camera cam;

    Vector3 originPos;
    float limX, limZ;
    float limZoomMin, limZoomMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        transformCamara = gameObject.transform;
        cam = GetComponent<Camera>();

        sensibilidad = 5f;
        sensibilidadZoom = 5f;
        maxX = 3f;
        maxZ = 3f;
        limZoomMin = 3f;
        limZoomMax = 5f;
        float firstX = transformCamara.localPosition.x;
        float firstY = transformCamara.localPosition.y;
        float firstZ = transformCamara.localPosition.z;
        originPos = new Vector3(firstX, firstY, firstZ);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (sensibilidadZoom > 10f) sensibilidadZoom = 10f;
        else if (sensibilidadZoom < 0.1f) sensibilidadZoom = 0.1f;

        float currentZoom = cam.orthographicSize;
        float newZoom = currentZoom;

        if (Input.GetKey(KeyCode.M))
            newZoom += sensibilidadZoom / 60f;
        if (Input.GetKey(KeyCode.N))
            newZoom -= sensibilidadZoom / 60f;

        if (newZoom > limZoomMax) newZoom = limZoomMax;
        else if (newZoom < limZoomMin) newZoom = limZoomMin;

        cam.orthographicSize = newZoom;

        ////////////////////////////////////////////////////////////////////

        if (sensibilidad > 10f) sensibilidad = 10f;
        else if (sensibilidad < 0.1f) sensibilidad = 0.1f;
        float currentX = transformCamara.localPosition.x;
        float currentY = transformCamara.localPosition.y;
        float currentZ = transformCamara.localPosition.z;

        float newX = currentX + Input.GetAxis("Vertical") * sensibilidad / 60;
        float newZ = currentZ + Input.GetAxis("Horizontal") * -sensibilidad / 60;

        float deltaX = maxX;
        float deltaZ = maxZ;

        float limXT = originPos.x + deltaX;
        float limZT = originPos.z + deltaZ;
        float limXB = originPos.x - deltaX;
        float limZB = originPos.z - deltaZ;

        if (newX < limXB) newX = limXB;
        else if (newX > limXT) newX = limXT;
        if (newZ < limZB) newZ = limZB;
        else if (newZ > limZT) newZ = limZT;

        transformCamara.localPosition = new Vector3(newX, currentY, newZ);
    }
}
