using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuOptions;
    [SerializeField] TMP_Text gameGoalText;

    public GameObject textPopUp;
    public TMP_Text textPopUpDescription;
    public Image playerHPBar;
    public Image enemyHPBar;
    public GameObject playerDamageScreen;
    public GameObject player;
    public playercontroller playerScript;
    public GameObject playerHealthScreen;
    public GameObject playerJumpScreen;

    public bool isPaused;

    float timeScaleOriginal;

    int gameGoalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()        // initializes before start - reserve awake exclusively for managers
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playercontroller>();
        timeScaleOriginal = Time.timeScale;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == menuOptions)
            {
                menuOptions.SetActive(false);
                menuPause.SetActive(true);
                menuActive = menuPause;
                return;
            }

            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }

        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void openOptions()
    {
        menuPause.SetActive(false);
        menuOptions.SetActive(true);
        menuActive = menuOptions;
    }

    public void youLose()
    {
        StartCoroutine(ShowDelayLossMenu());
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalText.text = gameGoalCount.ToString("F0");

        if (gameGoalCount <= 0)
        {
            StartCoroutine(ShowDelayWinMenu());
        }
    }

    public int getGameGoalCount()
    {
        return gameGoalCount;
    }

    public Vector3 savedCheckpointPos;

    public void SetCheckpoint(Vector3 pos, GameObject player)
    {
        savedCheckpointPos = pos;
    }

    private IEnumerator ShowDelayLossMenu()
    {
        statePause();
        yield return new WaitForSecondsRealtime(3);

        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    private IEnumerator ShowDelayWinMenu()
    {
        statePause();
        yield return new WaitForSecondsRealtime(3);

        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    // having issues getting this to work with player controller will check back on this later on.
    //public void RespawnPlayer(GameObject player)
    //{
       // player.transform.position = savedCheckpointPos;

        //playercontroller pc = player.GetComponent<playercontroller>();
        //if (pc != null)
        //{
            //pc.RestoreToCheckpoint();
        //}
    //}

}
