using Fantasy.Entitas;
using Fantasy.Entitas.Interface;

namespace Hotfix.Model;

public class Model_Account : Entity, ISupportedDataBase {
    public string account;
    public string password;
    public long loginTime;
    public long createTime; // 账号创建时间
}