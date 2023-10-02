using NaughtyAttributes;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private RaceTrackManager _raceTrackManager => InstantcesContainer.Instance.RaceTrackManager;
    
    [Header("Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _mergeCollider;
   

    [Header("Drag data")]
    [SerializeField] private bool mouseButtonReleased = true;
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Vector3 startDragPosition;
    [SerializeField] private Quaternion startDragRotation;

    private Rigidbody _rigidbody;
    private void Awake()
    {

        if (!_camera) _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private Vector3 GetMousePosition()
    {
        return _camera.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        //start dragging
        mouseButtonReleased = false;
        _mergeCollider.enabled = true;
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
        _mergeCollider.enabled = false;
        ResetTransform();
        _raceTrackManager.endDragEvent?.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!mouseButtonReleased)
        {
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        var thisGameObjectName = gameObject.name;
        var collisionGameObjectName = other.gameObject.name;

        if (mouseButtonReleased && thisGameObjectName == collisionGameObjectName)
        {
          //  Instantiate(nextEvolutionCar, transform.position, transform.rotation);
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

public class MergeDetect : MonoBehaviour
{
    protected MergeMaster _mergeMaster;
    [SerializeField] protected CarData _carData;
    private string _tag => gameObject.tag;
    [SerializeField,Tag] protected string reqTag;
    
    public CarData CarMergeData => _carData;

    public void SetMergeMaster(MergeMaster mm) => _mergeMaster = mm;
  
}
