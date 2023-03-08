using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConsoleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int maxLineCount = 10;
    int lineCount = 0;
    string myLog;
    

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(lineCount);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.LogError(lineCount);
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error: logString = "<color=red>" + logString + "</color>"; break;
            case LogType.Assert: logString = "<color=white>" + logString + "</color>"; break;
            case LogType.Warning: logString = "<color=yellow>" + logString + "</color>"; break;
            case LogType.Log:  logString = "<color=white>" + logString + "</color>"; break;
            case LogType.Exception:logString = "<color=red>" + logString + "</color>";  break;
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        myLog += "\n" + logString;
        lineCount++;

        if (lineCount > maxLineCount)
        {
            lineCount--;
            myLog = DeleteLines(myLog, 1);
        }

        text.text = myLog;
    }

    private string DeleteLines(string message, int linesToRemove)
    {
        return message.Split(Environment.NewLine.ToCharArray(), linesToRemove + 1).Skip(linesToRemove).FirstOrDefault();
    }
}
