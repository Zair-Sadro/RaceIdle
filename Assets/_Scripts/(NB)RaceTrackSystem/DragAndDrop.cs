using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Zenject.Inject] private RaceTrackManager _raceTrackManager;
    
    [Header("Components")]
    [SerializeField] private GameObject nextEvolutionCar;
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Drag data")]
    [SerializeField] private bool mouseButtonReleased = true;
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Vector3 startDragPosition;
    [SerializeField] private Quaternion startDragRotation;

    private void Awake()
    {
        //initialization
        if (!nextEvolutionCar) nextEvolutionCar = gameObject;
        if (!_camera) _camera = Camera.main;
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    }

    private Vector3 GetMousePosition()
    {
        return _camera.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        //start dragging
        mouseButtonReleased = false;
        _raceTrackManager.startDragEvent?.Invoke();

        //initialize positions
        startDragPosition = transform.position;
        startDragRotation = transform.rotation;
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        transform.position = _camera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }

    private void OnMouseUp()
    {
        //end dragging
        mouseButtonReleased = true;
        ResetTransform();
        _raceTrackManager.endDragEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        var thisGameObjectName = gameObject.name;
        var collisionGameObjectName = other.gameObject.name;

        if (mouseButtonReleased && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(nextEvolutionCar, transform.position, transform.rotation);
            mouseButtonReleased = false;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ResetTransform()
    {
        _rigidbody.Sleep();
        
        var transformObject = transform;
        transformObject.position = startDragPosition;
        transformObject.rotation = startDragRotation;
    }
}
