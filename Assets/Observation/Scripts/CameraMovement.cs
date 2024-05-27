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

        private GameObject _nextcam;
        private GameObject _activecam;
        private CameraInLevel _cam;
        public GameObject camerasHandler;


        private void Start()
        {
            _cam  = camerasHandler.GetComponent<CameraInLevel>();

            _activecam = GetActiveCam(_cam.CamList);
            _nextcam = GetNextCam(_cam.CamList, _activecam);
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
        }

        private void SwapCamera()
        {
            _activecam.SetActive(false);
            _nextcam.SetActive(true);
            
            _activecam = GetActiveCam(_cam.CamList);
            _nextcam = GetNextCam(_cam.CamList, _activecam);
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
                if (cam.activeInHierarchy) return cam;
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
    }
