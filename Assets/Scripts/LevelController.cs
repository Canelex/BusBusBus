using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Related Components
    private BusController bus;
    private LineController line;
    private CanvasController canvas;
    // Level Settings
    public float sectorTime;
    public float cameraTime;
    public int numSectors;
    // Level Data
    private bool gameOver;
    private bool levelStarted;
    private int currentSector;
    private bool sectorInProgress;
    private float progress; // [0, 1]
    // Other stuff
    private Vector3 cameraStart;
    private Vector3 cameraStop;
    public Animator explosionPrefab;
    private bool showTips;


    void Start()
    {
        // Find the main parts of the level.
        bus = FindObjectOfType<BusController>();
        line = FindObjectOfType<LineController>();
        canvas = FindObjectOfType<CanvasController>();

        // Check if tips are enabled.
        showTips = BetterPrefs.GetBool(Globals.KEY_TIPS_ENABLED, Globals.DEFAULT_TIPS_ENABLED);
        if (!showTips)
        {
            DestroyAllHints();
        }
    }

    void Update()
    {
        if (!levelStarted)
        {
            // When player taps screen, start the level.
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        StartLevel();
                    }
                }
            }
        }

        if (IsLevelPlaying())
        {
            if (sectorInProgress)
            {
                UpdateSector();
            }
            else
            {
                UpdateCamera();
            }
        }
    }

    private void UpdateCamera()
    {
        progress += Time.deltaTime / cameraTime; // Update progress [0, 1]
        Camera.main.transform.position = Vector3.Lerp(cameraStart, cameraStop, Mathf.Min(1, progress));
        line.UpdateLine(); // Update line after camera

        if (progress > 1)
        {
            progress = 0;
            sectorInProgress = true;
        }
    }

    private void UpdateSector()
    {
        progress += Time.deltaTime / sectorTime; // Update progress [0, 1]
        line.UpdateLine(); // Update line before bus
        bus.UpdatePosition(Mathf.Min(1, progress));

        if (progress > 1)
        {
            progress = 0;

            if (currentSector < numSectors) // More sectors to go
            {
                currentSector++;
                sectorInProgress = false;
                cameraStart = Camera.main.transform.position;
                cameraStop = cameraStart + Vector3.up * Camera.main.orthographicSize * 2;
            }
            else
            {
                LevelCompleted();
            }
        }
    }

    private void LevelCompleted()
    {
        // Level cleared! Play victory bell
        gameOver = true;
        AudioManager.Instance.Play("Bell");

        // Save this victory in prefs
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        BetterPrefs.SetBool(Globals.PREFIX_LEVEL_COMPLETED + levelIndex, true);

        int totalLevels = SceneManager.sceneCountInBuildSettings;
        if (levelIndex + 1 < totalLevels) // There are more scenes.
        {
            // Show UI.
            int levelsUnlocked = BetterPrefs.GetInt(Globals.KEY_LEVELS_UNLOCKED, Globals.DEFAULT_LEVELS_UNLOCKED);
            bool nextLevelUnlocked = (levelIndex - 1) < levelsUnlocked;
            canvas.ShowVictoryUI(nextLevelUnlocked);
        }
        else
        {
            canvas.ShowGameCompleteUI();
        }
    }

    private void StartLevel()
    {
        levelStarted = true;
        sectorInProgress = true;
        currentSector = 1;
        DestroyAllHints();
        AudioManager.Instance.Play("Bus");
    }

    private void DestroyAllHints()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Hint"))
        {
            Destroy(go);
        }
    }

    public bool IsLevelPlaying()
    {
        return levelStarted && !gameOver;
    }

    public void BusCrashedAt(Vector2 position)
    {
        // Defeat! Play crash sound.
        gameOver = true;
        AudioManager.Instance.Play("Crash");

        // Play explosion effect
        Animator explosion = Instantiate(explosionPrefab);
        explosion.transform.position = position;
        Destroy(explosion.gameObject, explosion.GetCurrentAnimatorStateInfo(0).length);

        // Show UI
        canvas.ShowDefeatUI();
    }
}
