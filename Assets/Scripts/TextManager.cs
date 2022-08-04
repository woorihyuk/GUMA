using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _gameUIManager = FindObjectOfType<GameUIManager>();

        var files = Directory.GetFiles(new Uri(Path.Combine(Application.streamingAssetsPath, "Dialogs")).AbsolutePath,
            "*.txt");
        foreach (var file in files)
        {
            var sr = new StreamReader(file);
            var strings = sr.ReadToEnd().Split('\n');
            _loadedData.Add(Path.GetFileNameWithoutExtension(file), strings.ToList());
        }
    }

    public TMP_Text dialogText;
    public Image dialogBackgroundImage;
    public bool isOpen;
    public int currentIndex;
    private readonly Dictionary<string, List<string>> _loadedData = new();
    private Sequence _textSequence;
    private readonly List<string> _parsedStrings = new();
    private GameUIManager _gameUIManager;

    public void OnInput(string key)
    {
        if (!isOpen)
        {
            ((RectTransform)_gameUIManager.letterBox.transform).DOSizeDelta(new Vector2(0, 0), 0.7f)
                .OnComplete(() =>
                {
                    dialogBackgroundImage.gameObject.SetActive(true);
                    _textSequence?.Kill();
                    _textSequence = DOTween.Sequence();
                    currentIndex = 0;
                    PlayText(key, currentIndex);
                    isOpen = true;
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
        dialogBackgroundImage.gameObject.SetActive(false);
        ((RectTransform)_gameUIManager.letterBox.transform).DOSizeDelta(new Vector2(0, 260f), 0.7f)
            .OnComplete(() =>
            {
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
        _parsedStrings.Clear();
        dialogText.text = "";
        var text = _loadedData[key][index];
        var charTag = ParseTag(ref text);
        ParseText(text);

        if (charTag == "fr")
        {
            // 불
        }

        var sb = new StringBuilder();

        foreach (var str in _parsedStrings)
        {
            if (EventParser(str)) continue;
            sb.Append(str);
            var finalString = sb.ToString();
            _textSequence.Append(DOTween
                .To(() => dialogText.text, x => dialogText.text = x, finalString, finalString.Length * 0.025f)
                .SetEase(Ease.Linear));
        }

        _textSequence.Restart();
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
                _textSequence.AppendCallback(() => ActionEventManager.Instance.CloseUpToPlayer());
            }
            else if (str == "ResetCameraPosition")
            {
                _textSequence.AppendCallback(() => ActionEventManager.Instance.ResetCamSize());
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