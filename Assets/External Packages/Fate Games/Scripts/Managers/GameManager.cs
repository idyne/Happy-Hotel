﻿using UnityEngine;
using System.IO;
using System.Collections;

namespace FateGames
{
    public class GameManager : MonoBehaviour
    {
        #region Properties
        private LevelManager levelManager;
        [SerializeField] private int targetFrameRate = -1;
        public LevelManager LevelManager { get => levelManager; }
        #endregion

        private void Initialize()
        {
            if (!AvoidDuplication()) return;
            SaveSystem.LoadPlayerData(() =>
            {
                Application.targetFrameRate = targetFrameRate;
                AnalyticsManager.Initialize();
                FacebookManager.Initialize(SceneManager.LoadCurrentLevel);
                StartCoroutine(AutoSaveCoroutine());
            }
            );
        }

        public void OnLevelWasLoaded(int level)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        private IEnumerator AutoSaveCoroutine()
        {
            yield return new WaitForSeconds(10f);
            SaveSystem.SavePlayerData();
            StartCoroutine(AutoSaveCoroutine());
        }

        #region Unity Callbacks

        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            if (State != GameState.LOADING_SCREEN)
                CheckInput();
        }

        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance { get => instance; }


        private bool AvoidDuplication()
        {
            if (!instance)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                return true;
            }
            else
                DestroyImmediate(gameObject);
            return false;
        }

        #endregion

        private void CheckInput()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.S))
                    TakeScreenshot();
                if (Input.GetKeyDown(KeyCode.X) && State == GameState.IN_GAME)
                    SceneManager.FinishLevel(true);
                else if (Input.GetKeyDown(KeyCode.C) && State == GameState.IN_GAME)
                    SceneManager.FinishLevel(false);
            }
            if (Input.GetMouseButtonDown(0) && State == GameState.START_SCREEN)
                SceneManager.StartLevel();
        }
        private void TakeScreenshot()
        {
            string folderPath = Directory.GetCurrentDirectory() + "/Screenshots/";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var screenshotName =
                                    "Screenshot_" +
                                    System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                    ".png";
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
            Debug.Log(folderPath + screenshotName);
        }

        #region State Management
        private GameState state = GameState.LOADING_SCREEN;
        public GameState State { get => state; }


        public void UpdateGameState(GameState newState)
        {
            state = newState;
        }
        #endregion

    }
    public enum GameState { LOADING_SCREEN, START_SCREEN, IN_GAME, PAUSE_SCREEN, FINISHED, COMPLETE_SCREEN }
}