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
}