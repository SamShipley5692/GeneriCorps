using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float scrollSpeed;

    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("SpeedUp Credits"))
        {
            rectTransform.anchoredPosition += new Vector2(0, (scrollSpeed * Time.deltaTime) * 10);
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
}
