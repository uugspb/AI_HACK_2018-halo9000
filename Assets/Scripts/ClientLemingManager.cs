using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ClientLemingManager : MonoBehaviour
{
    public RecordsStorage RecordsStorage;

    public LemingMovementController Leming;
    [SerializeField] private LemmingMovementDirection _input;

    public LemmingRunRecord Record;
    public Vector3 SpawnPosition;


    public bool RecordRun;

    public bool Simulate;

    private int _minimumFrameDistanceToForceMutation = 100;
    private void Awake()
    {
        Record = RecordsStorage.GetNewRecord();
        //[RecordID];

        if (Leming == null)
        {
            Leming = FindObjectOfType<LemingMovementController>();
        }

        Leming.OnDead += (controller, killer) =>
        {
            
            BloodManager.instance.ShowKillEffect(controller.transform.position);
            Record.Mutate(SimualtionFrameId, _previousFrameId);
            _previousFrameId = SimualtionFrameId;


            if(killer == Killer.Player)
                BloodManager.instance.SpawnGrave(controller.transform.position);
            SimualtionFrameId = 0;
            controller.Respawn(SpawnPosition);
        };

        Leming.OnExit += controller => { WinnersTable.WinnersData.Add(Record); };

        SpawnPosition = Leming.transform.position;
    }

    public int RecordID = 0;

    private void Update()
    {
        KillLemmingsOnMouseClick();

        if (Input.GetKeyDown(KeyCode.Space))
            Leming.transform.position = SpawnPosition;


        _input = 0;
        var horizontal = Input.GetAxis("Horizontal");
        _input |= ((horizontal > 0)
            ? LemmingMovementDirection.Right
            : horizontal < 0
                ? LemmingMovementDirection.Left
                : LemmingMovementDirection.None);
        var vertical = Input.GetAxis("Vertical");
        _input |= (vertical > 0 ? LemmingMovementDirection.Jump : LemmingMovementDirection.None);
    }

    private void KillLemmingsOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hitedCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _raycastResults, Mathf.Infinity);
            if (hitedCount > 0)
            {
                for (var i = 0; i < hitedCount; i++)
                {
                    var colliderGameObject = _raycastResults[i].collider.gameObject;
                    var lemingMovementController = colliderGameObject.GetComponent<LemingMovementController>();
                    if (lemingMovementController != null)
                    {
                        lemingMovementController.Die(Killer.Player);
                    }
                }
            }
        }
    }


    public bool Save;
    public int SimualtionFrameId = 0;
    private int _previousFrameId = 0;
    private RaycastHit2D[] _raycastResults = new RaycastHit2D[100];

    private void FixedUpdate()
    {
        if (Simulate)
        {
            _input = Record.GetOrGenerateNextMovement(SimualtionFrameId++);
        }
        else if (RecordRun)
            Record.AddMovement(_input);

        Leming.ManualFixedUpdate(_input);
//        if (Save)
//        {
//            Save = false;
//            RecordsStorage.Records.Add(Record);
//        }
    }
}