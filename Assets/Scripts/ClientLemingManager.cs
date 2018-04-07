using System.Runtime.Serialization.Formatters;
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

    private void Awake()
    {
        Record = new LemmingRunRecord();
        SpawnPosition = Leming.transform.position;
    }

    public int RecordID = 0;

    private void Update()
    {
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


    public bool Save;
    public int SimualtionFrameId = 0;

    private void FixedUpdate()
    {
        if (Simulate)
        {
           _input = RecordsStorage.Records[RecordID].GetOrGenerateNextMovement(SimualtionFrameId++);
        }
        else if (RecordRun)
            Record.AddMovement(_input);


        Leming.ManualFixedUpdate(_input);
        
        
        if (Save)
        {
            Save = false;
            RecordsStorage.Records.Add(Record);
        }
    }
}