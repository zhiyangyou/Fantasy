using Fantasy.Entitas;
using Fantasy.Entitas.Interface;

public class Model_Role : Entity, ISupportedDataBase {
    public long account_id;
    public int role_id;
    public int level;
    public string role_name;
}