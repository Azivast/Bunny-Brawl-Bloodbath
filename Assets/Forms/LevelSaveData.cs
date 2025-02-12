using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSaveData: MonoBehaviour
{
    public enum GeneratorType
    {
        Perlin,
        Agent
    }

    [SerializeField] private GeneratorType _generatorType;
    
    private class Data
    {
        public string sceneName;
        public string generatorType;
        public int enemiesKilled;
        public int damageTaken;
        public float timeStart;
        public float timeEnd;
        public float timeTaken;
        public bool playerWon;
    }

    private LevelSaveData.Data _data;

    private void Awake()
    {
        _data = new Data();
        _data.generatorType = _generatorType.ToString();
        _data.sceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        _data.timeStart = Time.time;
        Debug.Log(Application.persistentDataPath);
    }

    private void OnDestroy()
    {
        _data.timeEnd = Time.time;
        _data.timeTaken = _data.timeEnd - _data.timeStart;
        SaveData();
    }

    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(_data);
        FileIO.SaveData(jsonData, global::SaveData.Filename);
    }
}
