using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    public static MapBoundary Instance { get; private set; }

    [Header("Ukuran Map")]
    public Vector2 mapSize = new Vector2(100f, 100f); // lebar x tinggi
    public Vector2 mapCenter = Vector2.zero;

    [Header("Visual Border")]
    public bool drawBorder = true;
    public float borderThickness = 0.5f;
    public Color borderColor = Color.red;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (drawBorder) CreateBorderVisual();
    }

    // Dipanggil objek lain buat nge-clamp posisinya
    public Vector2 Clamp(Vector2 pos, float radius = 0f)
    {
        float halfW = mapSize.x / 2f - radius;
        float halfH = mapSize.y / 2f - radius;

        pos.x = Mathf.Clamp(pos.x, mapCenter.x - halfW, mapCenter.x + halfW);
        pos.y = Mathf.Clamp(pos.y, mapCenter.y - halfH, mapCenter.y + halfH);
        return pos;
    }

    public bool IsInside(Vector2 pos)
    {
        return Mathf.Abs(pos.x - mapCenter.x) <= mapSize.x / 2f &&
               Mathf.Abs(pos.y - mapCenter.y) <= mapSize.y / 2f;
    }

    // Posisi random di dalam map (berguna buat TrashSpawner)
    public Vector2 RandomPoint(float margin = 1f)
    {
        float x = Random.Range(mapCenter.x - mapSize.x / 2f + margin, mapCenter.x + mapSize.x / 2f - margin);
        float y = Random.Range(mapCenter.y - mapSize.y / 2f + margin, mapCenter.y + mapSize.y / 2f - margin);
        return new Vector2(x, y);
    }

    void CreateBorderVisual()
    {
        GameObject border = new GameObject("MapBorder");
        border.transform.SetParent(transform);

        LineRenderer lr = border.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.loop = true;
        lr.positionCount = 4;
        lr.startWidth = lr.endWidth = borderThickness;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = borderColor;
        lr.sortingOrder = 10;

        float hw = mapSize.x / 2f;
        float hh = mapSize.y / 2f;
        lr.SetPosition(0, new Vector3(mapCenter.x - hw, mapCenter.y - hh, 0));
        lr.SetPosition(1, new Vector3(mapCenter.x - hw, mapCenter.y + hh, 0));
        lr.SetPosition(2, new Vector3(mapCenter.x + hw, mapCenter.y + hh, 0));
        lr.SetPosition(3, new Vector3(mapCenter.x + hw, mapCenter.y - hh, 0));
    }

    // Biar kelihatan di Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mapCenter, mapSize);
    }
}