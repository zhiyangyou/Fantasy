#if UNITY_EDITOR
using DG.Tweening;
using Fantasy;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportMapConfigToJson {
    static string ClientConfigDataDir {
        get {
            var clientDir = Path.Combine(Application.dataPath, "Editor/MapConfig/");
            return clientDir;
        }
    }
    
    static string ClientSourceCodeDir {
        get {
            var clientDir = Path.Combine(Application.dataPath, "GameScripts/MonoScript/Map/MapTools");
            return clientDir;
        }
    }

    static string ServerConfigDir {
        get {
            var serverDir = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.Parent.Parent.FullName, "Server/Fantasy/examples/Server/Hotfix/MapConfig/ConfigData");
            return serverDir;
        }
    }

    [MenuItem("Tools/导出地图配置")]
    public static void ExportMapConfig() {
        string mapCfgPath = Path.Combine(Application.dataPath, "GameData/Hall/Prefabs/Map");
        string[] filePathArr = Directory.GetFiles(mapCfgPath);
        List<MapConfig> mapConfigs = new List<MapConfig>();
        foreach (var path in filePathArr) {
            if (!path.EndsWith(".meta")) {
                Map map = AssetDatabase.LoadAssetAtPath<Map>(path.Replace(Application.dataPath, "Assets"));
                map.transform.position = map.mapInitPos;
                MapConfig mapConfig = new MapConfig();
                mapConfig.mapType = (MapType)map.MapType;
                mapConfig.mapName = map.MapType.ToString();
                mapConfig.maxPosition = ToCSVector3(map.roleMoveMaxPos);
                mapConfig.minPosition = ToCSVector3(map.roleMoveMinPos);
                mapConfig.mapDoorDataList = new List<MapDoorDataCfg>();
                foreach (MapEntry mapEntry in map._ListAllMapEntry) {
                    MapDoorDataCfg doorCfg = new MapDoorDataCfg();
                    doorCfg.doorType = (DoorType)mapEntry.GotoDoorType;
                    doorCfg.roleInitPos = ToCSVector3(mapEntry.DoorPos);
                    mapConfig.mapDoorDataList.Add(doorCfg);
                }
                mapConfigs.Add(mapConfig);
            }
        }
        string json = JsonConvert.SerializeObject(mapConfigs, Formatting.Indented);
        Debug.Log(json);
        var clientDir = ClientConfigDataDir;

        if (!Directory.Exists(clientDir)) {
            Directory.CreateDirectory(clientDir);
        }
        var saveClientPath = Path.Combine(clientDir, "MapConfig.json");
        File.WriteAllText(saveClientPath, json);
        Debug.Log("MapConfig Generate Success Path:" + saveClientPath);
        AssetDatabase.Refresh();
        CopyCodeAndConfigToServer();
    }

    private static void CopyCodeAndConfigToServer() {

        CopyDir(ClientConfigDataDir);
        CopyDir(ClientSourceCodeDir);
        void CopyDir(string sourceDir) {
            var dirInfo = new DirectoryInfo(sourceDir);
            foreach (var fileInfo in dirInfo.GetFiles()) {
                if (fileInfo.FullName.EndsWith(".cs")
                    || fileInfo.FullName.EndsWith(".json")
                   ) {
                    var copyFileFullPath = Path.Combine(ServerConfigDir, fileInfo.Name);
                    File.Copy(fileInfo.FullName, copyFileFullPath, true);
                    Debug.LogError($"拷贝文件:{copyFileFullPath}");
                }
            }
        }

    }

    public static CSVector3 ToCSVector3(Vector3 ve3) {
        CSVector3 csVector3 = new CSVector3();
        csVector3.x = ve3.x;
        csVector3.y = ve3.y;
        csVector3.z = ve3.z;
        return csVector3;
    }
}
#endif