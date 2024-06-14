using UnityEngine;
using UnityEngine.UI;

public class PhysicsInteraction : MonoBehaviour
{
    [SerializeField] private float maxGrabDistance = 20f; // Maximum raycast distance to check if there is a rigidbody on the way
    [SerializeField] private float maxEmptyDistance = 25f; // Maximum empty distance from the camera when moving shpere with mouse
    [SerializeField] private float grabSpring = 40f;  // Adjust grab strength values / SpringJoint values
    [SerializeField] private float grabDamper = 0.2f; // Adjust grab strength values / SpringJoint values
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private float emptyMoveSpeed = 0.1f; // move empty with mouse speed 
    [SerializeField] private Image imgCanGrab;  // Can grab image
    [SerializeField] private Image imgIsGrab;  // Is grabbing image
    [SerializeField] private Image imgCrosshair;  // Crosshair

    private Rigidbody _hitRigidbody;
    private bool _isShooting;
    private RaycastHit _hitInfo;

    private GameObject _empty;
    private Rigidbody _emptyRb;

    [SerializeField] private Camera mainCamera;
    private void Start()
    {
        _empty = new GameObject();
        _empty.transform.parent = mainCamera.transform;
        _empty.AddComponent<Rigidbody>();
        _emptyRb = _empty.GetComponent<Rigidbody>();
        _emptyRb.isKinematic = true;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (imgCanGrab.isActiveAndEnabled || imgIsGrab.isActiveAndEnabled)
            {
                imgCrosshair.gameObject.SetActive(false);
            }
            else
            {
                imgCrosshair.gameObject.SetActive(true);
            }

            ShootRaycast();

            if (Input.GetMouseButton(0) && _isShooting)
            {
                ApplySpringConstraint();
                MoveEmptyWithMouse();

                if (Input.GetMouseButtonDown(1) && _hitRigidbody != null)
                {
                    ThrowObject(_hitRigidbody);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetShooting();
            }
        }
        else
        {

        }
    }

    private void ShootRaycast()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out _hitInfo, maxGrabDistance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hitRigidbody = _hitInfo.collider.GetComponent<Rigidbody>();
                if (_hitInfo.transform.GetComponent<Keypad>())
                {
                    _hitInfo.transform.GetComponent<Keypad>().Load();
                    HideImage(imgCanGrab);
                    return;
                }
                if (_hitInfo.transform.GetComponent<Note>())
                {
                    _hitInfo.transform.GetComponent<Note>().Load();
                    HideImage(imgCanGrab);
                    return;
                }
                MoveEmpty(_hitInfo.point);
                _isShooting = true;
            }

            if (_isShooting == false)
            {
                DisplayImage(imgCanGrab); // Display the first image if raycasting to a rigidbody                
            }
            else HideImage(imgCanGrab);

            if (!_hitInfo.collider.GetComponent<Rigidbody>()) HideImage(imgCanGrab);
        }
        else HideImage(imgCanGrab);
    }

    private void MoveEmpty(Vector3 position)
    {
        _empty.transform.position = position;
        _empty.transform.parent = mainCamera.transform;
    }

    private void ApplySpringConstraint()
    {
        if (_hitRigidbody && _empty)
        {
            SpringJoint spring = _hitRigidbody.gameObject.GetComponent<SpringJoint>();
            if (!spring)
            {
                spring = _hitRigidbody.gameObject.AddComponent<SpringJoint>();
                spring.autoConfigureConnectedAnchor = false;
                spring.connectedBody = _emptyRb;
                spring.connectedAnchor = Vector3.zero;
                spring.spring = grabSpring;
                spring.damper = grabDamper;

                spring.massScale = 1f;
                spring.minDistance = 0.1f;
                spring.maxDistance = 0f;

                // Set the spring joint anchors
                Vector3 localHitPoint = _hitInfo.point - _hitRigidbody.gameObject.transform.position;
                Vector3 scaleOfHitObject = _hitRigidbody.gameObject.transform.localScale;
                Quaternion rotation = Quaternion.Euler(_hitRigidbody.transform.rotation.eulerAngles);
                rotation.x *= -1;
                rotation.y *= -1;
                rotation.z *= -1;
                Vector3 rotatedLocalHitPoint = rotation * localHitPoint;
                rotatedLocalHitPoint = new Vector3(
                    rotatedLocalHitPoint.x / scaleOfHitObject.x,
                    rotatedLocalHitPoint.y / scaleOfHitObject.y,
                    rotatedLocalHitPoint.z / scaleOfHitObject.z);
                //Debug.Log(rotatedLocalHitPoint);
                spring.anchor = rotatedLocalHitPoint;

                DisplayImage(imgIsGrab);
            }
        }
    }

    private void MoveEmptyWithMouse()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        if (_empty)
        {
            float scrollWheelFactor = Input.GetAxis("Mouse ScrollWheel") * 5;
            Vector3 emptyMovement = mainCamera.transform.forward * (emptyMoveSpeed * mouseY) + scrollWheelFactor * Camera.main.transform.forward;
            Vector3 emptyPos = _empty.transform.position + emptyMovement;

            // Clamp empty position relative to the camera
            float emptyDistanceFromCamera = Vector3.Distance(Camera.main.transform.position, emptyPos);
            if (emptyDistanceFromCamera <= maxEmptyDistance && emptyDistanceFromCamera > 0.4f)
            {
                _empty.transform.position = emptyPos;
            }
        }
    }

    private void ResetShooting()
    {
        if (_hitRigidbody)
        {
            Destroy(_hitRigidbody.GetComponent<SpringJoint>());
        }

        _isShooting = false;
        _hitRigidbody = null;
        HideImage(imgIsGrab);
        HideImage(imgCanGrab);
    }

    private void ThrowObject(Rigidbody objectToThrow)
    {
        Vector3 throwDirection = mainCamera.transform.forward;
        objectToThrow.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        ResetShooting();
    }

    private void DisplayImage(Image img)
    {
        img.gameObject.SetActive(true);
    }

    private void HideImage(Image img)
    {
        img.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_empty != null)
        {
            Gizmos.DrawWireSphere(_empty.transform.position, 0.2f);
        }
    }
}