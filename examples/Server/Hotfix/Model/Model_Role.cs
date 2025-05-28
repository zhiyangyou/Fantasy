using Fantasy;
using Fantasy.Entitas;
using Fantasy.Entitas.Interface;
using Fantasy.Network;
using MongoDB.Bson.Serialization.Attributes;

public class Model_Role : Entity, ISupportedDataBase {
    public long account_id;
    public int role_id;
    public int level;
    public string role_name;

    [BsonIgnore] // 不写入数据库
    public Session session = null;

    public RoleData ToRoleData() {
        return new RoleData() {
            role_id = this.role_id,
            level = this.level,
            role_name = this.role_name,
            uid = this.Id,
            account_id = this.account_id,
        };
    }
}