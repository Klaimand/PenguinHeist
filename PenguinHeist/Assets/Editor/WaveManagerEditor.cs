using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveManager))]
public class WaveManagerEditor : Editor
{
    WaveManager waveManager;
    
    private void OnEnable()
    {
        waveManager = (WaveManager)target;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < waveManager.spawnPoints.Length; i++)
        {
            Vector3 pos = Handles.PositionHandle(waveManager.spawnPoints[i], Quaternion.identity);
            Handles.Label(waveManager.spawnPoints[i], "Spawn Point " + i);
            waveManager.spawnPoints[i] = new Vector3(pos.x, 0.5f, pos.z);
        }
    }
}
