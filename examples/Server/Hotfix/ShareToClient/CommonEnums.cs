namespace ServerShareToClient {
    public enum PlayerMapStatus {
        None = 0,
        InMap = 1,
        OutMap = 2,
    }

    public enum TeamOpStatus {
        None = 0,
        MemberJoined = 1, // 1.玩家加入
        MemberLeave = 2, // 2.玩家离开
        TeamDispose = 3, // 3.队伍解散
    }

    public enum BattleStateEnum {
        None,
        Start,
        End,
    }

    public enum EBattlePlayerOpType {
        None, // 0:无操作
        InputMove, // 1:方向输入
        ReleaseSkill, // 2:释放技能 // EBattlePlayerOpType
    }

    public enum EBattleOperateSkillType {
        None,
        ClickSkill, // 点击触发技能
        GuideSkill, // 位置引导技能
        StockPileTriggerSkill, // 蓄力技能
    }
}