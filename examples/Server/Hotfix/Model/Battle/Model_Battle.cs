using System.Collections.Concurrent;
using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Model.Battle;

public class Buffer_FrameOpDatas {
    private List<FrameOperateData> _cacheBuffer = new();
    private List<FrameOperateData> _sendBuffer = new();

    private object lockObj = new(); // 禁用线程跟踪

    public bool HasData => _sendBuffer.Count > 0 || _cacheBuffer.Count > 0;

    public void AddClientDatas(List<FrameOperateData> data) {
        lock (lockObj) {
            _cacheBuffer.AddRange(data);
        }
    }


    public void Clear() {
        lock (lockObj) {
            _cacheBuffer.Clear();
            _sendBuffer.Clear();
        }
    }

    public void ReadSendDatasAndSwapBuffer(Action<IEnumerable<FrameOperateData>> sendDatas) {
        try {
            lock (lockObj) {
                (_sendBuffer, _cacheBuffer) = (_cacheBuffer, _sendBuffer);
                sendDatas.Invoke(_sendBuffer);
                _sendBuffer.Clear();
            }
        }
        catch (Exception e) {
            Log.Error($"ReadSendDatasAndSwapBuffer error: {e.Message}");
        }
    }
}

/// <summary>
/// 表达: 一场帧同步战斗
/// </summary>
public class Model_Battle : Entity {
    #region 属性和字段

    public long battleID { get; private set; }

    /// <summary>
    /// key: account_ID
    /// </summary>
    private Dictionary<long, Model_BattlePlayer> _dicAllPlayers = null;

    public BattleStateEnum BattleState { get; private set; } = BattleStateEnum.None;

    public long LogicFrameID { get; private set; }

    private long _logicFrameTimerID = -1;

    private Buffer_FrameOpDatas _frameOpDataBuffer = null;

    #endregion


    #region public

    public void Init(List<Model_Role> players) {
        BattleState = BattleStateEnum.None;
        this.battleID = this.Id;
        this.LogicFrameID = 0;
        _frameOpDataBuffer = new();
        _dicAllPlayers = new();
        foreach (var modelRole in players) {
            var modelBattlePlayer = Entity.Create<Model_BattlePlayer>(Scene, true, false);
            modelBattlePlayer.Init(modelRole);
            _dicAllPlayers.Add(modelRole.account_id, modelBattlePlayer);
        }
    }

    public void BattleStart() {
        this.BattleState = BattleStateEnum.Start;
        _logicFrameTimerID = FTask.RepeatedTimer(Scene, GameConstConfig.LogicFrameIntervalMS, OnLogicFrameUpdate);
    }

    public void OnPlayerOperateFrameInput(List<FrameOperateData> listFrameOpDatas) {
        _frameOpDataBuffer.AddClientDatas(listFrameOpDatas);
    }

    #endregion

    #region private

    private async void OnLogicFrameUpdate() {
        try {
            this.LogicFrameID++;
            Msg_S2C_FrameOpEvent msg = new Msg_S2C_FrameOpEvent();
            msg.battle_id = this.battleID;
            msg.logic_frame_id = LogicFrameID;
            msg.frame_operate_datas = new List<FrameOperateData>();
            _frameOpDataBuffer.ReadSendDatasAndSwapBuffer(datas => { msg.frame_operate_datas.AddRange(datas); });

            // 广播数据
            foreach (var modelBattlePlayer in _dicAllPlayers.Values) {
                if (!modelBattlePlayer.session.IsDisposed) {
                    modelBattlePlayer.session.Send(msg);
                }
            }
        }
        catch (Exception e) {
            Log.Error($"逻辑帧更新失败: {e.Message}");
        }
    }

    public override void Dispose() {
        if (_logicFrameTimerID > 0) {
            FTask.RemoveTimer(Scene, ref _logicFrameTimerID);
        }
        _logicFrameTimerID = -1;
        LogicFrameID = 0;
        this.BattleState = BattleStateEnum.None;

        if (_dicAllPlayers != null) {
            foreach (var kv in _dicAllPlayers) {
                kv.Value.Dispose();
            }
            _dicAllPlayers.Clear();
        }
        if (_frameOpDataBuffer != null) {
            _frameOpDataBuffer = null;
        }
        base.Dispose();
    }

    #endregion
}