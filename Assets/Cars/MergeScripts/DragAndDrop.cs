using NaughtyAttributes;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private RaceTrackManager _raceTrackManager => InstantcesContainer.Instance.RaceTrackManager;

    public Camera RaceCamera { set => _camera = value; }

    [Header("Components")]
    [SerializeField] private Camera _camera;

    [Header("Drag data")]
    [SerializeField] private bool mouseButtonReleased = true;
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Vector3 startDragPosition;
    [SerializeField] private Quaternion startDragRotation;
    [SerializeField] private float _dragHeight = 3.3f;

    private Rigidbody _rigidbody;
    public bool NoDragNow = false;

    private void Start()
    {

        _rigidbody = GetComponent<Rigidbody>();
    }

    private Vector3 GetMousePosition()
    {
        return _camera.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        if(NoDragNow) return;

        
        //start dragging
        mouseButtonReleased = false;
        _raceTrackManager.StopCars();


        //initialize positions
        startDragPosition = transform.position;
        startDragRotation = transform.rotation;
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        if (NoDragNow) return;
        var pos = _camera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        transform.position = new Vector3(pos.x,_dragHeight,pos.z);
    }

    private void OnMouseUp()
    {
        if (NoDragNow) return;
        //end dragging
        mouseButtonReleased = true;
      
        ResetTransform();
        _raceTrackManager.StartCars();

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
        //var thisGameObjectName = gameObject.name;
        //var collisionGameObjectName = other.gameObject.name;

        //if (mouseButtonReleased && thisGameObjectName == collisionGameObjectName)
        //{
        //  //  Instantiate(nextEvolutionCar, transform.position, transform.rotation);
        //    mouseButtonReleased = false;
        //    Destroy(other.gameObject);
        //    Destroy(gameObject);
        //}
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
    [SerializeField] protected CarAI _carAI;

    public CarAI CarAI=>_carAI;

   private string _tag => gameObject.tag;
    [SerializeField,Tag] protected string reqTag;

    public void SetMergeMaster(MergeMaster mm) => _mergeMaster = mm;
  
}
