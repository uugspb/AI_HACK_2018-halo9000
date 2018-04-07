using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BloodManager : MonoBehaviour {

    [SerializeField]
    private Sprite[] _bloods;

    [SerializeField]
    private GameObject[] _graves;

    float maxDist = 3f;
    float maxDistGrave = 5f;
    float explosionTtl = 5f;
    int bloodsPerExplosion = 3;
    int maxBloods = 100;
    int maxGraves = 100;

    private GameObject _bloodExplosionPrefab;

    List<GameObject> _bloodObjects = new List<GameObject>();
    List<GameObject> _graveObjects = new List<GameObject>();

    public static BloodManager instance;

    private void Awake()
    {
        instance = this;
        _bloodExplosionPrefab = Resources.Load<GameObject>("Effects/BloodExplosion");
    }

    void Update () {
        if (HUD.gameMode == GameMode.Play && Input.GetMouseButtonDown(0))
        {
            Vector3 killPoint = Input.mousePosition;
            killPoint = Camera.main.ScreenToWorldPoint(killPoint);
            killPoint.z = -1;
            ShowKillEffect(killPoint);
            SpawnGrave(killPoint);
        }
	}

    public void SpawnGrave(Vector3 killPoint)
    {
        killPoint.z = 1;
        int layer = 1 << LayerMask.NameToLayer("Default");
        var hit = Physics2D.Raycast(killPoint, Vector2.down, maxDistGrave, layer);
        if (hit.collider != null)
        {
            GameObject grave = Instantiate<GameObject>(_graves[Random.Range(0, _graves.Length)], killPoint, Quaternion.identity);
            Collider2D collider = grave.GetComponentInChildren<Collider2D>();
            collider.enabled = false;
            _graveObjects.Add(grave);
            if (_graveObjects.Count > maxGraves)
            {
                Destroy(_graveObjects[0]);
                _graveObjects.RemoveAt(0);
            }

            SpriteRenderer sprite = grave.GetComponentInChildren<SpriteRenderer>();
            sprite.color = new Color(1, 1, 1, 0);
            sprite.DOFade(1, 1);
            grave.transform.DOMoveY(hit.point.y, 2).SetEase(Ease.OutBounce).
                OnComplete(() => collider.enabled = true);
        }
    }


    public void ShowKillEffect(Vector3 killPoint)
    {
        GameObject expl = Instantiate<GameObject>(_bloodExplosionPrefab, killPoint, Quaternion.identity);
        Destroy(expl, explosionTtl);

        for(int i=0; i < bloodsPerExplosion; i++)
            SpawnMeats(killPoint);
    }

    void SpawnMeats(Vector3 killPoint)
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        int layer = LayerMask.GetMask("Default", "Grave");
        var hit = Physics2D.Raycast(killPoint, dir, maxDist, layer);
        if (hit.collider != null)
        {
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, -1);
            GameObject blood = new GameObject("Blood");
            blood.transform.SetParent(transform);
            blood.transform.position = hitPoint;
            SpriteRenderer sprite = blood.AddComponent<SpriteRenderer>();
            sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            sprite.sprite = _bloods[Random.Range(0, _bloods.Length)];
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(0.5f, 1.5f);
            scale.y = Random.Range(0.5f, 1.5f);
            blood.transform.localScale = scale;

            sprite.color = new Color(1f, 1f, 1f, 0f);
            sprite.DOFade(1f, 2f);

            _bloodObjects.Add(blood);

            if (_bloodObjects.Count > maxBloods)
            {
                Destroy(_bloodObjects[0]);
                _bloodObjects.RemoveAt(0);
            }
        }
    }
}
