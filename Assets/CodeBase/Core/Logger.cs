using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CodeBase.Core
{
    public class Logger : Singleton<Logger>
    {
        [SerializeField] private TextMeshProUGUI debugAreaText = null;

        [SerializeField] private bool enableDebug = false;

        [SerializeField] private int maxLines = 15;

        private void Awake()
        {
            if (debugAreaText == null)
            {
                debugAreaText = GetComponent<TextMeshProUGUI>();
            }

            debugAreaText.text = string.Empty;
        }

        private void OnEnable()
        {
            debugAreaText.enabled = enableDebug;
            enabled = enableDebug;

            if (enabled)
            {
                debugAreaText.text +=
                    $"<color=\"white\">{GetTimestamp()} {this.GetType().Name} enabled</color>\n";
            }
        }

        public void LogInfo(string message)
        {
            ClearLines();

            debugAreaText.text += $"<color=\"green\">{GetTimestamp()} {message}</color>\n";
        }

        public void LogError(string message)
        {
            ClearLines();
            debugAreaText.text += $"<color=\"red\">{GetTimestamp()} {message}</color>\n";
        }

        public void LogWarning(string message)
        {
            ClearLines();
            debugAreaText.text += $"<color=\"yellow\">{GetTimestamp()} {message}</color>\n";
        }

        private void ClearLines()
        {
            if (debugAreaText.text.Split('\n').Count() >= maxLines)
            {
                debugAreaText.text = string.Empty;
            }
        }

        private static string GetTimestamp() => DateTime.Now.ToString("HH:mm:ss.fff");
    }
}