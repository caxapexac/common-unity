using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Gui.Widgets
{
    /// <summary>
    /// A console to display Unity's debug logs in-game.
    /// </summary>
    internal class Console : MonoBehaviour
    {
        public static Version Version = new Version(1, 0, 0);

        #region Inspector Settings

        /// <summary>
        /// The hotkey to show and hide the console window.
        /// </summary>
        public KeyCode ToggleKey = KeyCode.BackQuote;

        /// <summary>
        /// Whether to open as soon as the game starts.
        /// </summary>
        public bool OpenOnStart = false;

        /// <summary>
        /// Whether to open the window by shaking the device (mobile-only).
        /// </summary>
        public bool ShakeToOpen = true;

        /// <summary>
        /// The (squared) acceleration above which the window should open.
        /// </summary>
        public float ShakeAcceleration = 3f;

        /// <summary>
        /// Whether to only keep a certain number of logs, useful if memory usage is a concern.
        /// </summary>
        public bool RestrictLogCount = false;

        /// <summary>
        /// Number of logs to keep before removing old ones.
        /// </summary>
        public int MaxLogCount = 1000;

        #endregion

        private static readonly GUIContent _clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private static readonly GUIContent _collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        private const int Margin = 20;
        private const string WindowTitle = "Console";

        private static readonly Dictionary<LogType, Color> _logTypeColors = new Dictionary<LogType, Color>
        {
            {LogType.Assert, Color.white},
            {LogType.Error, Color.red},
            {LogType.Exception, Color.red},
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow},
        };

        private bool _isCollapsed;
        private bool _isVisible;
        private readonly List<Log> _logs = new List<Log>();
        private readonly ConcurrentQueue<Log> _queuedLogs = new ConcurrentQueue<Log>();

        private Vector2 _scrollPosition;
        private readonly Rect _titleBarRect = new Rect(0, 0, 10000, 20);
        private Rect _windowRect = new Rect(Margin, Margin, Screen.width - (Margin * 2), Screen.height - (Margin * 2));

        private readonly Dictionary<LogType, bool> _logTypeFilters = new Dictionary<LogType, bool>
        {
            {LogType.Assert, true},
            {LogType.Error, true},
            {LogType.Exception, true},
            {LogType.Log, true},
            {LogType.Warning, true},
        };

        #region MonoBehaviour Messages

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= HandleLogThreaded;
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        private void OnGUI()
        {
            if (!_isVisible)
            {
                return;
            }

            _windowRect = GUILayout.Window(123456, _windowRect, DrawWindow, WindowTitle);
        }

        private void Start()
        {
            if (OpenOnStart)
            {
                _isVisible = true;
            }
        }

        private void Update()
        {
            UpdateQueuedLogs();

            if (Input.GetKeyDown(ToggleKey))
            {
                _isVisible = !_isVisible;
            }

            if (ShakeToOpen && Input.acceleration.sqrMagnitude > ShakeAcceleration)
            {
                _isVisible = true;
            }
        }

        #endregion

        private void DrawCollapsedLog(Log log)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(log.GetTruncatedMessage());
            GUILayout.FlexibleSpace();
            GUILayout.Label(log.Count.ToString(), GUI.skin.box);

            GUILayout.EndHorizontal();
        }

        private void DrawExpandedLog(Log log)
        {
            for (var i = 0; i < log.Count; i += 1)
            {
                GUILayout.Label(log.GetTruncatedMessage());
            }
        }

        private void DrawLog(Log log)
        {
            GUI.contentColor = _logTypeColors[log.Type];

            if (_isCollapsed)
            {
                DrawCollapsedLog(log);
            }
            else
            {
                DrawExpandedLog(log);
            }
        }

        private void DrawLogList()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // Used to determine height of accumulated log labels.
            GUILayout.BeginVertical();

            var visibleLogs = _logs.Where(IsLogVisible);

            foreach (Log log in visibleLogs)
            {
                DrawLog(log);
            }

            GUILayout.EndVertical();
            var innerScrollRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndScrollView();
            var outerScrollRect = GUILayoutUtility.GetLastRect();

            // If we're scrolled to bottom now, guarantee that it continues to be in next cycle.
            if (Event.current.type == EventType.Repaint && IsScrolledToBottom(innerScrollRect, outerScrollRect))
            {
                ScrollToBottom();
            }

            // Ensure GUI colour is reset before drawing other components.
            GUI.contentColor = Color.white;
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(_clearLabel))
            {
                _logs.Clear();
            }

            foreach (LogType logType in Enum.GetValues(typeof(LogType)))
            {
                var currentState = _logTypeFilters[logType];
                var label = logType.ToString();
                _logTypeFilters[logType] = GUILayout.Toggle(currentState, label, GUILayout.ExpandWidth(false));
                GUILayout.Space(20);
            }

            _isCollapsed = GUILayout.Toggle(_isCollapsed, _collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        private void DrawWindow(int windowId)
        {
            DrawLogList();
            DrawToolbar();

            // Allow the window to be dragged by its title bar.
            GUI.DragWindow(_titleBarRect);
        }

        private Log? GetLastLog()
        {
            if (_logs.Count == 0)
            {
                return null;
            }

            return _logs.Last();
        }

        private void UpdateQueuedLogs()
        {
            while (_queuedLogs.TryDequeue(out var log))
            {
                ProcessLogItem(log);
            }
        }

        private void HandleLogThreaded(string message, string stackTrace, LogType type)
        {
            var log = new Log
            {
                Count = 1,
                Message = message,
                StackTrace = stackTrace,
                Type = type,
            };

            // Queue the log into a ConcurrentQueue to be processed later in the Unity main thread,
            // so that we don't get GUI-related errors for logs coming from other threads
            _queuedLogs.Enqueue(log);
        }

        private void ProcessLogItem(Log log)
        {
            var lastLog = GetLastLog();
            var isDuplicateOfLastLog = lastLog.HasValue && log.Equals(lastLog.Value);

            if (isDuplicateOfLastLog)
            {
                // Replace previous log with incremented count instead of adding a new one.
                log.Count = lastLog.Value.Count + 1;
                _logs[_logs.Count - 1] = log;
            }
            else
            {
                _logs.Add(log);
                TrimExcessLogs();
            }
        }

        private bool IsLogVisible(Log log)
        {
            return _logTypeFilters[log.Type];
        }

        private bool IsScrolledToBottom(Rect innerScrollRect, Rect outerScrollRect)
        {
            var innerScrollHeight = innerScrollRect.height;

            // Take into account extra padding added to the scroll container.
            var outerScrollHeight = outerScrollRect.height - GUI.skin.box.padding.vertical;

            // If contents of scroll view haven't exceeded outer container, treat it as scrolled to bottom.
            if (outerScrollHeight > innerScrollHeight)
            {
                return true;
            }

            // Scrolled to bottom (with error margin for float math)
            return Mathf.Approximately(innerScrollHeight, _scrollPosition.y + outerScrollHeight);
        }

        private void ScrollToBottom()
        {
            _scrollPosition = new Vector2(0, int.MaxValue);
        }

        private void TrimExcessLogs()
        {
            if (!RestrictLogCount)
            {
                return;
            }

            var amountToRemove = _logs.Count - MaxLogCount;

            if (amountToRemove <= 0)
            {
                return;
            }

            _logs.RemoveRange(0, amountToRemove);
        }
    }


    /// <summary>
    /// A basic container for log details.
    /// </summary>
    internal struct Log
    {
        public int Count;
        public string Message;
        public string StackTrace;
        public LogType Type;

        /// <summary>
        /// The max string length supported by UnityEngine.GUILayout.Label without triggering this error:
        /// "String too long for TextMeshGenerator. Cutting off characters."
        /// </summary>
        private const int MaxMessageLength = 16382;

        public bool Equals(Log log)
        {
            return Message == log.Message && StackTrace == log.StackTrace && Type == log.Type;
        }

        /// <summary>
        /// Return a truncated message if it exceeds the max message length.
        /// </summary>
        public string GetTruncatedMessage()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return Message;
            }

            return Message.Length <= MaxMessageLength ? Message : Message.Substring(0, MaxMessageLength);
        }
    }


    /// <summary>
    /// Alternative to System.Collections.Concurrent.ConcurrentQueue
    /// (It's only available in .NET 4.0 and greater)
    /// </summary>
    /// <remarks>
    /// It's a bit slow (as it uses locks), and only provides a small subset of the interface
    /// Overall, the implementation is intended to be simple & robust
    /// </remarks>
    public class ConcurrentQueue<T>
    {
        private readonly object _queueLock = new object();
        private readonly Queue<T> _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (_queueLock)
            {
                if (_queue.Count == 0)
                {
                    result = default(T);
                    return false;
                }

                result = _queue.Dequeue();
                return true;
            }
        }
    }
}