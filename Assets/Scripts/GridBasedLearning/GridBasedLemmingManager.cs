using UnityEngine;

namespace DefaultNamespace
{
    public class GridBasedLemmingManager : MonoBehaviour
    {
        
        private readonly PositionToGridTransformer _positionTransformer = PositionToGridTransformer.Instance;

        public LemmingRunHistory _history;

        public LemingMovementController Leming;

        public bool Simulate;

        private void Awake()
        {
            _history = new LemmingRunHistory();

            if (Leming == null)
            {
                Leming = FindObjectOfType<LemingMovementController>();
            }

            Leming.OnDead += (controller, killer) =>
            {
                BloodManager.instance.ShowKillEffect(controller.transform.position);
                _history.Kill(killer);
                GlobalScore.CorrectScore(_history);
                _history = new LemmingRunHistory();
                GetNexStep(SpawnPosition);
                controller.Respawn(SpawnPosition);
            };

            Leming.OnExit += controller =>
            {
                _history.Win();
                GlobalScore.CorrectScore(_history);
                _history = new LemmingRunHistory();
            };

            SpawnPosition = Leming.transform.position;
        }

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


        private int _previousFrameId = 0;
        private RaycastHit2D[] _raycastResults = new RaycastHit2D[100];
        private static readonly GridMapScore GlobalScore = GridMapScore.GLOBAL_SCORE;
        private LemmingMovementDirection _input;
        private Vector3 SpawnPosition;

        private void FixedUpdate()
        {
            if (Simulate)
            {
                _input = GetNexStep(Leming.transform.position);
            }
            

            Leming.ManualFixedUpdate(_input);
        }
        

        LemmingMovementDirection GetNexStep(Vector3 position)
        {
            var coord = _positionTransformer.GetGridCoord(position);
            var direction = GlobalScore.GetBestDirection(coord);
            _history.AddRecord(coord, direction);
            return direction;
        }
    }
    
}