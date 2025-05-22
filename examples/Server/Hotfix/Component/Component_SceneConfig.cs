using Fantasy;
using Fantasy.Entitas;
using Fantasy.Platform.Net;

namespace Hotfix.Share;

public class Component_SceneConfig : Entity {
    private List<SceneConfig> _listGateConfigs = null;

    private List<SceneConfig> listGateConfigs {
        get {
            if (_listGateConfigs == null) {
                _listGateConfigs = new();
                foreach (var config in SceneConfigData.Instance.List) {
                    if (config.SceneType == (int)SceneType.Gate) {
                        listGateConfigs.Add(config);
                    }
                }
            }
            return _listGateConfigs;
        }
    }

    /// <summary>
    ///
    /// 返回值如: ("127.0.0.1", 1234)
    /// </summary>
    /// <param name="account_id"></param>
    /// <returns>(ip,port)</returns>
    /// <exception cref="Exception"></exception>
    public (string retAddress, uint sceneConfigID) GetGate(long account_id) {
        var gateIndex = (int)(account_id % listGateConfigs.Count); // 负载均衡
        var gateConfig = listGateConfigs[gateIndex];
        var outerPort = gateConfig.OuterPort;
        var processID = gateConfig.ProcessConfigId;
        var processConfig = ProcessConfigData.Instance.Get(processID);
        if (processConfig == null) {
            throw new Exception($"配置表有误,找不到processID = {processID} 的进程配置");
        }
        var machineID = processConfig.MachineId;
        var machineConfig = MachineConfigData.Instance.Get(machineID);
        if (machineConfig == null) {
            throw new Exception($"配置表有误,找不到 machineID = {machineID} 的机器s配置");
        }
        var strIP = machineConfig.OuterBindIP;
        var address = ($"{strIP}:{outerPort}");
        return (address, gateConfig.Id);
    }
}