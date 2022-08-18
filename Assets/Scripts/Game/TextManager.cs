using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DG.Tweening;
using Game.State;
using UnityEngine;

namespace Game
{
    public class TextManager : Singleton<TextManager>
    {
        protected override void Awake()
        {
            base.Awake();
            _endTriangleStartPos = GameUIManager.Instance.endTriangle.transform.localPosition.y;

            var files = Directory.GetFiles(new Uri(Path.Combine(Application.streamingAssetsPath, "Dialogs")).AbsolutePath,
                "*.txt");
            foreach (var file in files)
            {
                var sr = new StreamReader(file);
                var strings = sr.ReadToEnd().Split('\n');
                _loadedData.Add(Path.GetFileNameWithoutExtension(file), strings.ToList());
            }
        }
    
        public bool isOpen;
        public int currentIndex;
        private readonly Dictionary<string, List<string>> _loadedData = new();
        private Sequence _textSequence, _triangleSequence;
        private readonly List<string> _parsedStrings = new();
        private float _endTriangleStartPos;
        private string _lastDialogKey;
    
        public void OnInputWithLast()
        {
            OnInput(_lastDialogKey);
        }
    
        public void OnInput(string key)
        {
            if (DOTween.IsTweening(GameUIManager.Instance.dialogBackgroundImage.transform, true) || DOTween.IsTweening((RectTransform)GameUIManager.Instance.letterBox.transform, true)) return;
            if (!isOpen)
            {
                GameUIManager.Instance.SetActivePlayerHud(false);
                GameUIManager.Instance.letterBox.gameObject.SetActive(true);
                StateManager.Instance.currentState = StateType.Waiting;
                ((RectTransform)GameUIManager.Instance.letterBox.transform).DOSizeDelta(new Vector2(0, 0), 0.7f)
                    .OnComplete(() =>
                    {
                        StateManager.Instance.currentState = StateType.Talking;
                        _lastDialogKey = key;
                        _textSequence?.Kill();
                        _textSequence = DOTween.Sequence();
                        currentIndex = 0;
                        PlayText(key, currentIndex);
                    });
            }
            else
            {
                if (_textSequence.IsActive() && _textSequence.IsPlaying())
                {
                    _textSequence.Kill(true);
                }
                else
                {
                    _textSequence?.Kill();
                    _textSequence = DOTween.Sequence();
                    PlayText(key, ++currentIndex);
                }
            }
        }

        private void Close()
        {
            DeactivateEndTriangle();
            GameUIManager.Instance.dialogBackgroundImage.transform.DOLocalMoveY(GameUIManager.Instance.dialogBackgroundStartTf.localPosition.y, 0.5f);
            GameUIManager.Instance.dialogBackgroundImage.transform.DOScale(0, 0.5f);
            ((RectTransform)GameUIManager.Instance.letterBox.transform).DOSizeDelta(new Vector2(0, 260f), 0.7f)
                .OnComplete(() =>
                {
                    GameUIManager.Instance.dialogBackgroundImage.gameObject.SetActive(false);
                    GameUIManager.Instance.letterBox.gameObject.SetActive(false);
                    GameUIManager.Instance.SetActivePlayerHud(true);
                    GameUIManager.Instance.dialogText.text = "";
                    StateManager.Instance.currentState = StateType.None;
                    isOpen = false;
                });
        }

        private void PlayText(string key, int index)
        {
            if (_loadedData[key].Count <= index)
            {
                Close();
                return;
            }
            DeactivateEndTriangle();
            _parsedStrings.Clear();
            GameUIManager.Instance.dialogText.text = "";
            var text = _loadedData[key][index];
            var charTag = ParseTag(ref text);
            ParseText(text);

            if (charTag == "fr")
            {
                DeactivateCharacters();
                GameUIManager.Instance.dialogCharacters[0].gameObject.SetActive(true);
            }
            else
            {
                DeactivateCharacters();
            }

            var sb = new StringBuilder();

            foreach (var str in _parsedStrings)
            {
                if (EventParser(str)) continue;
                sb.Append(str);
                var finalString = sb.ToString();
                _textSequence.Append(DOTween
                    .To(() => GameUIManager.Instance.dialogText.text, x => GameUIManager.Instance.dialogText.text = x, finalString, finalString.Length * 0.025f)
                    .SetEase(Ease.Linear));
            }
        
            if (!isOpen)
            {
                GameUIManager.Instance.dialogBackgroundImage.gameObject.SetActive(true);
                GameUIManager.Instance.dialogBackgroundImage.transform.DOLocalMoveY(GameUIManager.Instance.dialogBackgroundEndTf.localPosition.y, 0.5f);
                GameUIManager.Instance.dialogBackgroundImage.transform.DOScale(1, 0.5f)
                    .OnComplete(() =>
                    {
                        isOpen = true;
                        _textSequence.SetDelay(0.1f).OnKill(ActiveEndTriangle).OnComplete(ActiveEndTriangle).Restart();
                    });
            }
            else
            {
                _textSequence.SetDelay(0.1f).OnKill(ActiveEndTriangle).OnComplete(ActiveEndTriangle).Restart();
            }
        }

        private void ActiveEndTriangle()
        {
            GameUIManager.Instance.endTriangle.gameObject.SetActive(true);
            _triangleSequence?.Kill(true);
            _triangleSequence = DOTween.Sequence()
                .Prepend(GameUIManager.Instance.endTriangle.transform.DOLocalMoveY(_endTriangleStartPos + 5, 0.4f))
                .Append(GameUIManager.Instance.endTriangle.transform.DOLocalMoveY(_endTriangleStartPos, 0.4f).SetDelay(0.4f))
                .SetEase(Ease.Linear).SetLoops(-1);
            _triangleSequence.Restart();
        }

        private void DeactivateEndTriangle()
        {
            _triangleSequence?.Kill(true);
            GameUIManager.Instance.endTriangle.transform.DOLocalMoveY(_endTriangleStartPos, 0).Play();
            GameUIManager.Instance.endTriangle.gameObject.SetActive(false);
        }

        private void DeactivateCharacters()
        {
            foreach (var image in GameUIManager.Instance.dialogCharacters)
            {
                image.gameObject.SetActive(false);
            }
        }

        private bool EventParser(string text)
        {
            if (text.StartsWith("{w="))
            {
                var actionIndex = text.IndexOf("{w=", StringComparison.InvariantCulture);
                var endIndex = text.IndexOf('}', actionIndex);
                var sb = new StringBuilder();
                for (var j = actionIndex + 3; j < endIndex; j++)
                {
                    sb.Append(text[j]);
                }

                var delay = float.Parse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                _textSequence.Append(DOVirtual.Int(0, 1, delay, _ => { }));
                return true;
            }
            if (text.StartsWith("{CloseUpToPlayer="))
            {
                var actionIndex = text.IndexOf("{CloseUpToPlayer=", StringComparison.InvariantCulture);
                var endIndex = text.IndexOf('}', actionIndex);
                var sb = new StringBuilder();
                for (var j = actionIndex + 17; j < endIndex; j++)
                {
                    sb.Append(text[j]);
                }

                var duration = float.Parse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                _textSequence.AppendCallback(() => ActionEventManager.Instance.CloseUpToPlayer(duration));
                return true;
            }
            if (text.StartsWith("{ResetCameraPosition="))
            {
                var actionIndex = text.IndexOf("{ResetCameraPosition=", StringComparison.InvariantCulture);
                var endIndex = text.IndexOf('}', actionIndex);
                var sb = new StringBuilder();
                for (var j = actionIndex + 21; j < endIndex; j++)
                {
                    sb.Append(text[j]);
                }

                var duration = float.Parse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                _textSequence.AppendCallback(() => ActionEventManager.Instance.ResetCamSize(duration));
                return true;
            }
            if (text.StartsWith("{"))
            {
                var actionIndex = text.IndexOf("{", StringComparison.InvariantCulture);
                var endIndex = text.IndexOf('}', actionIndex);
                var sb = new StringBuilder();
                for (var j = actionIndex + 1; j < endIndex; j++)
                {
                    sb.Append(text[j]);
                }

                var str = sb.ToString();
                if (str == "CloseUpToPlayer")
                {
                    _textSequence.AppendCallback(() => ActionEventManager.Instance.CloseUpToPlayer(0.5f));
                }
                else if (str == "ResetCameraPosition")
                {
                    _textSequence.AppendCallback(() => ActionEventManager.Instance.ResetCamSize(0.5f));
                }
                else if (str == "CameraShake")
                {
                    _textSequence.AppendCallback(() => ActionEventManager.Instance.ImpulseRandom());
                }
                return true;
            }

            return false;
        }

        private void ParseText(string text)
        {
            var startIndex = text.IndexOf('"');
            text = text.Remove(startIndex, 1);
            var lastIndex = text.LastIndexOf('"');
            text = text.Remove(lastIndex, 1);

            var realText = text;
            while (realText.Contains('{') && realText.Contains('}'))
            {
                var openIndex = realText.IndexOf('{');
                var closeIndex = realText.IndexOf('}', openIndex);
                realText = realText.Remove(openIndex, closeIndex - openIndex + 1);
            }

            for (var i = 0; i < text.Length; i++)
            {
                var actionIndex = text.IndexOf("{", i, StringComparison.InvariantCulture);

                if (actionIndex != -1)
                {
                    _parsedStrings.Add(text.Substring(i, actionIndex - i));
                    var actionLastIndex = text.IndexOf("}", actionIndex, StringComparison.InvariantCulture);
                    _parsedStrings.Add(text.Substring(actionIndex, actionLastIndex - actionIndex + 1));
                    i = actionLastIndex;
                    continue;
                }

                _parsedStrings.Add(text.Substring(i, text.Length - i));
                break;
            }
        }

        private static string ParseTag(ref string text)
        {
            var split = text.Split(' ');
            if (split.Length < 2) throw new Exception("태그를 파싱 할 수 없습니다");
            text = text.Remove(0, split[0].Length + 1);
            return split[0];
        }
    }
}