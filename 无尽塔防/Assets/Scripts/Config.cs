using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public static string ENEMYDATA_PATH = Application.persistentDataPath + "/EnemyData.Json";
    public static string TOWERDATA_PATH = Application.persistentDataPath + "/TowerData.Json";
    public static string LEVELDATA_PATH = Application.persistentDataPath + "/LevelData.Json";
    public static string CURRENTLEVELDATA_PATH = Application.persistentDataPath + "/CurrentLevelData.Json";

    public static string ABDATA_PATH = Application.dataPath + "/ABPackageRes/Data";
    public static string STEARINGASSET_PATH = Application.dataPath + "/StreamingAssets";

    public static string UPLOADAB_PATH = Application.dataPath + "/ABPackageRes/Data";
    public static string DownLOADAB_PATH = Application.persistentDataPath + "/ABPackageRes/Data";

}
