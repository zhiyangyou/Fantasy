namespace ServerShareToClient {
    /// <summary>
    /// 和客户端保持同步的一些配置
    /// </summary>
    public static class GameConstConfig {
        public const float FixedDeltaTime = 0.02f;

        public const float HallPlayerMoveSpeed = 2f;

        /// <summary>
        /// FixedUpdate每隔这个数值, 向后端同步一下这个状态
        /// </summary>
        public static int MaxSyncStateCount => 5;

        /// <summary>
        /// 队伍最多人数
        /// </summary>
        public const int TeamMaxNum = 4;
    }
}