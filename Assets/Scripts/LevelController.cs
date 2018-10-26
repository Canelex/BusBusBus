using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Camera cam;
    private LineController line;
    private BusController bus;
    public bool gameOver;
    public float sectorTime;
    private float sectorProgress;
    public int numSectors;
    private int currentSector;
    private bool cameraMoving;
    public float cameraTime;
    private float cameraProgress;
    private Vector3 cameraFinish;
    private Vector3 cameraStart;
    public Animator explosion;
    public GameObject gameOverCanvas;
    public GameObject nextLevelButton;
    public GameObject unlockLevelsButton;
    public GameObject replayButton;
    public GameObject levelSelectButton;
    public GameObject tipPanel;
    private bool tipsEnabled;
    
    void Start()
    {
        cam = Camera.main;
        line = FindObjectOfType<LineController>();
        bus = FindObjectOfType<BusController>();
        currentSector = 1;
        tipsEnabled = BetterPrefs.GetBool(BetterPrefs.KEY_TIPS_ENABLED, true); // Prefs
    }

    void Update()
    {
        if (!gameOver)
        {
            if (cameraMoving) // Camera is moving to next sector
            {
                cameraProgress += Time.deltaTime / cameraTime; // [0-1]

                if (cameraProgress > 1)
                {
                    cameraProgress = 1;
                    cameraMoving = false;
                    sectorProgress = 0;
                }

                cam.transform.position = Vector3.Lerp(cameraStart, cameraFinish, cameraProgress);
                line.UpdateLine(); // Update line after camera
            }
            else
            {
                sectorProgress += Time.deltaTime / sectorTime;

                if (sectorProgress > 1)
                {
                    sectorProgress = 1;

                    if (currentSector < numSectors) // More sectors to go
                    {
                        currentSector++;
                        cameraMoving = true;
                        cameraProgress = 0;
                        cameraStart = cam.transform.position;
                        cameraFinish = cameraStart + Vector3.up * cam.orthographicSize * 2;
                    }
                    else // Level cleared
                    {
                        gameOver = true;

                        // Level is completed!
                        int levelIndex = SceneManager.GetActiveScene().buildIndex;
                        BetterPrefs.SetBool(BetterPrefs.PREFIX_LEVEL_COMPLETED + levelIndex, true);

                        // Play victory sound
                        AudioManager.Instance.Play("Bell"); 

                        // Show UI overlay in a moment
                        levelSelectButton.SetActive(true);

                        // Is there a next level unlocked?
                        if (levelIndex - 1 < BetterPrefs.GetInt(BetterPrefs.KEY_LEVELS_UNLOCKED, 5))
                        {
                            nextLevelButton.SetActive(true);
                        }
                        else
                        {
                            unlockLevelsButton.SetActive(true);
                        }

                        Invoke("ShowGameOverCanvas", 0.75F);
                        
                    }
                }

                line.UpdateLine(); // Update line before bus
                bus.UpdatePosition(sectorProgress);
            }
        }
    }

    public void BusCrashedAt(Vector2 position)
    {
        // Play explosion effect
        Animator expl = Instantiate(explosion);
        expl.transform.position = position;
        Destroy(expl.gameObject, expl.GetCurrentAnimatorStateInfo(0).length);

        // Game Overr. Enable the correct buttons and show canvas in a second
        gameOver = true;
        replayButton.SetActive(true);
        levelSelectButton.SetActive(true);

        // Maybe show the hints panel
        if (tipPanel && tipsEnabled)
        {
            tipPanel.SetActive(true);
        }

        Invoke("ShowGameOverCanvas", 0.75F);
    }

    private void ShowGameOverCanvas()
    {
        AudioManager.Instance.Play("Pop"); // Pop sound (oneshot)
        gameOverCanvas.SetActive(true);
    }
}
