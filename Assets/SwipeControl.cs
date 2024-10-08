using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    private Vector2 _startMousePosition, _endMousePosition;
    private Rigidbody2D _rb;
    public float MaxSwipeForce = 40f;
    public float MinSwipeDistance = 0.05f;
    public float SensitivityMultiplier = 2f;
    public LineRenderer lineRenderer;
    private bool isSwipeMade = false;
    private SpriteRenderer _spriteRenderer;

    // ��� ��������
    public Color blinkColor = Color.red;
    public Color idleColor = Color.white;
    public Color swipeCompleteColor = Color.green;
    public float blinkSpeed = 2f; // �������� ��������

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer
        _rb.bodyType = RigidbodyType2D.Static; // ������������ �������� �� ������
        lineRenderer.enabled = false; // �������� ����� �� ������ ������
    }

    void Update()
    {
        // ���� ����� �� ��� ������, ���������� �������
        if (!isSwipeMade)
        {
            BlinkEffect(); // ��������� ������ ��������
        }

        // ������ ������ � �������� �����
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.enabled = true;  // �������� ����� ��� ���������
        }

        // � �������� ������ ��������� �����
        if (Input.GetMouseButton(0))
        {
            _endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DrawLine(_startMousePosition, _endMousePosition);  // ������������ �����
        }

        // ���������� ������
        if (Input.GetMouseButtonUp(0))
        {
            _endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Swipe();
            lineRenderer.enabled = false;  // �������� ����� ����� ������
        }
    }

    // ����� ��� �������� ����� ������ �������
    void BlinkEffect()
    {
        // ������ �����-����� ����� ��� ������� ��������
        float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        _spriteRenderer.color = Color.Lerp(idleColor, blinkColor, t); // ������� ��������� ����� �������
    }

    void Swipe()
    {
        Vector2 direction = _endMousePosition - _startMousePosition;
        float swipeDistance = direction.magnitude;

        if (swipeDistance < MinSwipeDistance)
        {
            return;
        }

        float swipeForce = Mathf.Clamp(swipeDistance * SensitivityMultiplier, 0, MaxSwipeForce);

        // ����� ������� ������ ���������� ������ � ���������� ��������
        if (!isSwipeMade)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic; // �������� ������
            isSwipeMade = true;
            _spriteRenderer.color = swipeCompleteColor; // ������ ��� ������� ����� ������
        }

        _rb.AddForce(-direction.normalized * swipeForce, ForceMode2D.Impulse);
    }

    // ��������� ����� �� ����� ������ � ������� ������� ������
    void DrawLine(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
