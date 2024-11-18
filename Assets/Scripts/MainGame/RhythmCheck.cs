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
        private Vector3 _perfectScale = new Vector3(1.5f, 1.5f, 1.5f);
        private Vector3 _goodScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 _normalScale = new Vector3(1.0f, 1.0f, 1.0f);

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

        public string CheckAttackTiming()
        {
            float currentTime = musicSource.time; 

            float nearestBeatTime = Mathf.Round(currentTime / _beatInterval) * _beatInterval;

            float timeDifference = Mathf.Abs(currentTime - nearestBeatTime);
            
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
                    uiImage.rectTransform.localScale = _perfectScale;
                    uiImage.color = Color.green;
                    break;
                case "Good":
                    uiImage.rectTransform.localScale = _goodScale;
                    uiImage.color = Color.blue;
                    break;
                case "Normal":
                    uiImage.rectTransform.localScale = _normalScale;
                    uiImage.color = Color.grey;
                    break;
            }
        }
    }
