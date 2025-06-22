using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float scrollSpeed;
    public string levelName;

    private float deadZone = 775;   // setting the Y axis transform location to set back to main menu

    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowMainMenuAfterCredits();

        if (Input.GetButton("SpeedUp Credits"))
        {
            rectTransform.anchoredPosition += new Vector2(0, (scrollSpeed * Time.deltaTime) * 10);
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }

    private IEnumerator ShowMainMenuAfterCredits()
    {
        yield return new WaitForSecondsRealtime(70);
        SceneManager.LoadScene(levelName);
    }
}
