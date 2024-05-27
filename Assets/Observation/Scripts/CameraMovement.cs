using System;
using UnityEngine;


    public class CameraMovement : MonoBehaviour
    {

        // Intégrer une sensibilité si possible
        private const float Sensitivity = 5f;

    
        //X Axis Clamp Handler
        float _xAxisClamp = 0f;
        //Y Axis Clamp Handler
        [SerializeField] private float mixYRot, maxYRot;
    
        //Get Camera
        public GameObject Camera;

        //Variables for Camera Swap handling
        private CameraInLevel _cam;
        public GameObject camerasHandler;

        //Variables for Camera Zoom Handling
        private Camera cam;
        private float scroll, zoom;
        [SerializeField] private float maxFOV, minFOV;
        
        
        private void Start()
        {
            cam = Camera.GetComponent<Camera>();
            _cam  = camerasHandler.GetComponent<CameraInLevel>();
            
            zoom = cam.fieldOfView;
            _cam._activeCam = GetActiveCam(_cam.CamList);
            _cam._nextCam = GetNextCam(_cam.CamList, _cam._activeCam);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwapCamera();
            }

            Cursor.lockState = CursorLockMode.Locked;
            RotateCamera();
            ZoomCamera();
        }

        private void SwapCamera()
        {
            _cam._activeCam.SetActive(false);
            _cam._nextCam.SetActive(true);
            
            _cam._activeCam = GetActiveCam(_cam.CamList);
            _cam._nextCam = GetNextCam(_cam.CamList, _cam._activeCam);
            

        }
        private void RotateCamera()
        {
            
            
            float mouseX = Input.GetAxis("Mouse X"); // Get X mouse movement
            float mouseY = Input.GetAxis("Mouse Y"); // Get Y mouse movement

            float xRotation = mouseX * Sensitivity;
            float yRotation = mouseY * Sensitivity;

            _xAxisClamp -= yRotation;
        

            Vector3 rotation = Camera.transform.rotation.eulerAngles;

            rotation.y  = Mathf.Clamp(rotation.y, mixYRot, maxYRot);
        
            rotation.x -= yRotation;
            rotation.y += xRotation;
            rotation.z = 0f;

            switch (_xAxisClamp)
            {
                case > 45:
                    _xAxisClamp = 45;
                    rotation.x = 45;
                    break;
                case < -45:
                    _xAxisClamp = -45;
                    rotation.x = -45;
                    break;
            }
        
            Camera.transform.rotation = Quaternion.Euler(rotation);
        }
        private static GameObject GetActiveCam(GameObject[] camList)
        {
            foreach (GameObject cam in camList)
            {
                if (cam.activeInHierarchy) {return cam;}
            }

            return null;
        }
        private static GameObject GetNextCam(GameObject[] camList, GameObject obj)
        {
            int length = camList.Length;

            for (int i = 0; i < length; i++)
            {
                GameObject cam = camList[i];
                if (cam == obj)
                {
                    if (i+1 == length) return camList[0];
                    return camList[i+1];
                }
            }
            return null;
        }

        private void ZoomCamera()
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0) zoom *= .90f;
            else if (scroll < 0) zoom *= 1.1f;
            zoom = Mathf.Clamp(zoom, minFOV, maxFOV);
            cam.fieldOfView = zoom;

        }
        
        
    }
