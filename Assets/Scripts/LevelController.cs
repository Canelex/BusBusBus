using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Camera cam;
    private BusController bus;
    private LineController line;
    private CanvasController canvas;
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
    public Animator explosionPrefab;
    private bool showTips;
    
    void Start()
    {
        // Find the main parts of the level.
        bus = FindObjectOfType<BusController>();
        line = FindObjectOfType<LineController>();
        canvas = FindObjectOfType<CanvasController>();
        cam = Camera.main;

        // Setup the level.
        gameOver = false;
        cameraMoving = false;
        currentSector = 1;
        sectorProgress = 0;
        cameraProgress = 0;

        // Check if tips are enabled.
        showTips = BetterPrefs.GetBool(Globals.KEY_TIPS_ENABLED, Globals.DEFAULT_TIPS_ENABLED);
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
            else // Sector is being played / Bus is moving
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
                    else
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
                }

                line.UpdateLine(); // Update line before bus
                bus.UpdatePosition(sectorProgress);
            }
        }
    }

    public void BusCrashedAt(Vector2 position)
    {
        // Play explosion effect
        Animator explosion = Instantiate(explosionPrefab);
        explosion.transform.position = position;
        Destroy(explosion.gameObject, explosion.GetCurrentAnimatorStateInfo(0).length);

        // Game over
        gameOver = true;

        // Show UI
        canvas.ShowDefeatUI();
    }
}
