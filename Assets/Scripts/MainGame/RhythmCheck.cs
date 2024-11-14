using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RhythmCheck : MonoBehaviour
    {
        public static RhythmCheck Instance { get; private set; }
        
        [Header("Music Info")]

        public float bpm = 120f;  
        public AudioSource musicSource; 
    
        private float _beatInterval;
    
        [Header("UI Settings")]

        public Image uiImage; 
        public Vector3 perfectScale = new Vector3(1.5f, 1.5f, 1.5f);
        public Vector3 goodScale = new Vector3(1.2f, 1.2f, 1.2f);
        public Vector3 normalScale = new Vector3(1.0f, 1.0f, 1.0f);

        // 節奏判定窗口
        public float perfectWindow = 0.15f;
        public float goodWindow = 0.3f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            _beatInterval = 60f / bpm;
            Debug.Log(_beatInterval);
        
            musicSource.Play();
        }

        private void Update()
        {
            UpdateBeatUI();
        }

        // 檢查攻擊的時機是否在節拍點上
        public string CheckAttackTiming()
        {
            float currentTime = musicSource.time; // 使用音樂播放時間，避免累積誤差

            // 計算最接近的節拍點
            float nearestBeatTime = Mathf.Round(currentTime / _beatInterval) * _beatInterval;

            // 計算操作與最近節拍點的偏差
            float timeDifference = Mathf.Abs(currentTime - nearestBeatTime);
            
            // 返回相應的精準度
            if (timeDifference <= perfectWindow)
            {
                return "Perfect";
            }
            else if (timeDifference <= goodWindow)
            {
                return "Good";
            }
            else
            {
                return "Normal";
            }
        }

        void UpdateBeatUI()
        {
            string timing = CheckAttackTiming();

            switch (timing)
            {
                case "Perfect":
                    uiImage.rectTransform.localScale = perfectScale;
                    uiImage.color = Color.green;
                    break;
                case "Good":
                    uiImage.rectTransform.localScale = goodScale;
                    uiImage.color = Color.blue;
                    break;
                case "Normal":
                    uiImage.rectTransform.localScale = normalScale;
                    uiImage.color = Color.grey;
                    break;
            }
        }
    }
