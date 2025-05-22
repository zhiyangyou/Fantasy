using Fantasy.Entitas;
using Fantasy.Entitas.Interface;

namespace Hotfix.Model;

public class Model_Account : Entity, ISupportedDataBase {
    public string account;
    public string password;
    public long loginTime;
    public long createTime; // 账号创建时间


    #region Game属性

    public long level;
    public long diamonds;
    public long golds;

    #endregion

    public void InitGameAttrs() {
        level = 1;
        golds = 0;
        diamonds = 0;
    }
}