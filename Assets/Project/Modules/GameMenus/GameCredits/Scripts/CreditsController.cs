using System;
using System.Collections;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private CreditsDisplayConfig _config;
        [SerializeField] private RectTransform _textsHolder;

        private bool _hasFinished;
        private float _accumulatedDistance;

        private RectTransform _lastSpawnedText;
        
        private CreditsContent Content => _config.Content;
        private CreditsSettings Settings => _config.Settings;

        private void Start()
        {
            StartCredits();
        }


        private void StartCredits()
        {
            _accumulatedDistance = 500f;
            SpawnAllTexts();
            StartCoroutine(ScrollCredits());
        }

        private IEnumerator ScrollCredits()
        {
            _hasFinished = false;
            
            while (!_hasFinished)
            {
                _textsHolder.localPosition += Vector3.up * (_config.NormalScrollSpeed * Time.deltaTime);
                CheckHasFinished();
                yield return null;
            }
        }

        private void CheckHasFinished()
        {
            _hasFinished = _lastSpawnedText.position.y > Screen.height / 2f;
        }


        private void SpawnAllTexts()
        {
            SpawnText(Content.Title, Settings.CreditsTitleSettings, Content.HeightGap);
        
            foreach (CreditsBlock creditsBlock in Content.Blocks)    
            {
                SpawnText(creditsBlock.Title, Settings.BlockSettings, creditsBlock.HeightGap);
                
                foreach (CreditsBlockElement creditsBlockElement in creditsBlock.Elements)    
                {
                    SpawnText(creditsBlockElement.Title, Settings.BlockElementSettings, creditsBlockElement.HeightGap);
                    
                    foreach (string item in creditsBlockElement.Items)    
                    {
                        SpawnText(item, Settings.ItemSettings, 0);
                    }
                }
            }
        }
        

        private void SpawnText(string content, TextSettings textSettings, float extraHeightGap)
        {
            _accumulatedDistance += textSettings.HeightGap + extraHeightGap;

            Vector3 spawnPosition = Vector3.down * _accumulatedDistance;

            CreditsText text = Instantiate(_config.TextPrefab, _textsHolder);
            text.Init(content, textSettings);
            text.Transform.localPosition = spawnPosition;

            _lastSpawnedText = text.Transform;
        }
        
    }
}