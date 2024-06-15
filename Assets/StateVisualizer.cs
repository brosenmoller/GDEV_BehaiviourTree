using TMPro;
using UnityEngine;

public class StateVisualizer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stateText;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetText(string text)
    {
        stateText.text = text;
    }

    public void SetText(string text, Color color)
    {
        stateText.text = text;
        stateText.color = color;
    }

    public void SetColor(Color color)
    {
        stateText.color = color;
    }

    private void Update()
    {
        Vector3 direction = mainCamera.transform.position - transform.position;
        Vector3 flatDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        transform.forward = flatDirection;
    }
}
