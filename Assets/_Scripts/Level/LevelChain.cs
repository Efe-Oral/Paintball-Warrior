using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string displayName;
    public string sceneName;
}

[CreateAssetMenu(fileName = "LevelChain", menuName = "PaintGame/Level Chain")]
public class LevelChain : ScriptableObject
{
    public LevelData[] levels;
}
