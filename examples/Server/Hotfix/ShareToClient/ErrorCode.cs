namespace Hotfix.ShareToClient;

public static class ErrorCode {
    public const uint Success = 0; // 正常, 成功
    
    public const uint EnterMap_NotSelectRole = 1301; // 进入地图接口, 当前没有选择特定角色
    public const uint EnterMap_MapConfigNotExist = 1302; // 进入地图接口, 地图id错误
    public const uint EnterMap_DoorConfigNotExist = 1303; // 进入地图接口, 门id错误
    public const uint EnterMap_RoleNotFound = 1304; // 进入地图接口, 门id错误
    public const uint EnterMap_Failed = 1305; // 进入地图接口, 门id错误
}