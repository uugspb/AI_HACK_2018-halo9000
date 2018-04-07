using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BloodManager : MonoBehaviour {

    [SerializeField]
    private Sprite[] _bloods;

    float maxDist = 3f;
    float explosionTtl = 5f;
    int bloodsPerExplosion = 3;
    int maxBloods = 100;

    private GameObject _bloodExplosionPrefab;

    List<GameObject> _bloodObjects = new List<GameObject>();

    public static BloodManager instance;

    private void Awake()
    {
        instance = this;
        _bloodExplosionPrefab = Resources.Load<GameObject>("Effects/BloodExplosion");
    }

    void Update () {
		
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 killPoint = Input.mousePosition;
            killPoint = Camera.main.ScreenToWorldPoint(killPoint);
            killPoint.z = -1;
            ShowKillEffect(killPoint);
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
        int layer = 1 << LayerMask.NameToLayer("Default");
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
