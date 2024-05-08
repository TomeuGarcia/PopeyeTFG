using System;
using System.Collections;
using InputSystem;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Modules.GameMenus.GameCredits
{
    public class CreditsDisplayer : MonoBehaviour
    {
        [SerializeField] private CreditsDisplayConfig _config;
        [SerializeField] private RectTransform _textsHolder;
        [SerializeField] private CanvasGroup _inputHolderGroup;
        
        private InputAction _fastInput;

        private bool _hasFinished;
        private float _accumulatedDistance;

        private RectTransform _lastSpawnedText;

        private float _currentScrollSpeed;
        
        private CreditsContent Content => _config.Content;
        private CreditsSettings Settings => _config.Settings;

        public Action OnCreditsFinished;
        

        private void Awake()
        {
            _fastInput = new PlayerAnchorInputControls().UI.Back;
            _fastInput.Enable();
            _fastInput.IsPressed();
            
            _currentScrollSpeed = _config.NormalScrollSpeed;
            
            _inputHolderGroup.alpha = 0;
        }

        private void OnDestroy()
        {
            _fastInput.Disable();
            StopAllCoroutines();
        }

        private void Update()
        {
            if (_hasFinished) return;
            
            if (_fastInput.WasPressedThisFrame())
            {
                _currentScrollSpeed = _config.FastScrollSpeed;
            }
            else if (_fastInput.WasReleasedThisFrame())
            {
                _currentScrollSpeed = _config.NormalScrollSpeed;
            }
        }


        public void StartCredits()
        {
            _accumulatedDistance = _config.StartScrollOffset;
            SpawnAllTexts();
            StartCoroutine(ScrollCredits());
            StartCoroutine(ShowScrollFastInput());
        }

        private IEnumerator ScrollCredits()
        {
            _hasFinished = false;
            
            while (!_hasFinished)
            {
                UpdateScrollingPosition();
                CheckHasFinished();
                yield return null;
            }

            float endStartScrollSpeed = _currentScrollSpeed;
            Timer stopScrollingTimer = new Timer(_config.StopScrollingDuration);
            while (!stopScrollingTimer.HasFinished())
            {
                stopScrollingTimer.Update(Time.deltaTime);

                float t = _config.StopScrollingEase.Evaluate(stopScrollingTimer.GetCounterRatio01());
                _currentScrollSpeed = Mathf.LerpUnclamped(endStartScrollSpeed, 0, t);
                UpdateScrollingPosition();
                yield return null;
            }
            
            _config.PlayFinishSound();
            yield return new WaitForSeconds(_config.DelayBeforeFinishing);
            
            OnCreditsFinished?.Invoke();
        }

        private void UpdateScrollingPosition()
        {
            _textsHolder.localPosition += Vector3.up * (_currentScrollSpeed * Time.deltaTime);
        }

        private void CheckHasFinished()
        {
            _hasFinished = _lastSpawnedText.position.y > Screen.height / 2f;
        }
        
        
        private IEnumerator ShowScrollFastInput()
        {
            yield return new WaitForSeconds(_config.DelayBeforeShowingInput);
            _inputHolderGroup.Fade(_config.ShowInputFade);
        }



        private void SpawnAllTexts()
        {
            SpawnImage(Content.GameLogo, Settings.GameLogoSettings, 0f);
            
            SpawnText(Content.Title, Settings.CreditsTitleSettings, Content.ExtraHeightGap);
            
            SpawnImage(Content.TeamLogo, Settings.TeamLogoSettings, 0f);

            foreach (CreditsBlock creditsBlock in Content.Blocks)    
            {
                SpawnText(creditsBlock.Title, Settings.BlockSettings, creditsBlock.ExtraHeightGap);
                
                foreach (CreditsBlockElement creditsBlockElement in creditsBlock.Elements)    
                {
                    SpawnText(creditsBlockElement.Title, Settings.BlockElementSettings, creditsBlockElement.ExtraHeightGap);
                    
                    foreach (string item in creditsBlockElement.Items)    
                    {
                        SpawnText(item, Settings.ItemSettings, 0f);
                    }
                }
            }

            SpawnText(Content.Closing, Settings.ClosingSettings, 0f);
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
        
        private void SpawnImage(Sprite content, ImageSettings textSettings, float extraHeightGap)
        {
            _accumulatedDistance += textSettings.HeightGap + extraHeightGap;

            Vector3 spawnPosition = Vector3.down * _accumulatedDistance;

            CreditsImage image = Instantiate(_config.ImagePrefab, _textsHolder);
            image.Init(content, textSettings);
            image.Transform.localPosition = spawnPosition;

            _lastSpawnedText = image.Transform;
        }
        
    }
}