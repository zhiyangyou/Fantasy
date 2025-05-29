using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection.Metadata;
using Fantasy;
using Hotfix.Model.Battle;

namespace Test;

public class Tests {
    private long uniqueID = 0;

    [OneTimeSetUp]
    public void SetUp() {
        Log.Initialize();
    }

    private long NextUniqueID {
        get {
            var ret = Interlocked.Increment(ref uniqueID);
            return ret;
        }
    }

    private ConcurrentDictionary<long, FrameOperateData> _testAllGenDatas = new();
    private ConcurrentDictionary<long, FrameOperateData> _testRecieveDatas = new();

    private List<FrameOperateData> GenFrameOpDatas(int min, int max) {
        var count = Random.Shared.Next(min, max);
        var retList = new List<FrameOperateData>();
        for (int i = 0; i < count; i++) {
            var genOne = new FrameOperateData() { };
            var uniqueID = NextUniqueID;
            genOne.account_id = uniqueID;
            _testAllGenDatas.TryAdd(uniqueID, genOne);
            retList.Add(genOne);
        }
        return retList;
    }

    private bool JudgeIsRight() {
        var list1 = _testAllGenDatas.Keys.ToList();
        list1.Sort();
        var list2 = _testRecieveDatas.Keys.ToList();
        list2.Sort();

        _testAllGenDatas.Clear();
        _testRecieveDatas.Clear();

        return list1.First() == list2.First()
               && list1.Last() == list2.Last()
               && list1.Count == list2.Count;
    }

    private bool IsAllComplete(List<Task> listTasks) {
        return listTasks.All(t => t.IsCompleted);
    }

    [Test]
    public void Test_DoubleBuffer() {
        Buffer_FrameOpDatas buffer = new();

        var forCount = 10;
        var sw = new Stopwatch();
        sw.Start();
        while ((forCount--) > 0) {
            var listWriteTask = new List<Task>();
            for (int i = 0; i < 20; i++) {
                listWriteTask.Add(Task.Run(() => {
                    var clientData = GenFrameOpDatas(1_0000, 2_0000);
                    buffer.AddClientDatas(clientData);
                }));
            }

            var listReadTask = new List<Task>();
            for (int i = 0; i < 20; i++) {
                listReadTask.Add(Task.Run(() => {
                    while (true) {
                        buffer.ReadSendDatasAndSwapBuffer((getDatas) => {
                            foreach (var data in getDatas) {
                                _testRecieveDatas.TryAdd(data.account_id, data);
                            }
                        });
                        if (IsAllComplete(listWriteTask) && !buffer.HasData) {
                            break;
                        }
                    }
                }));
            }


            Task.WaitAll(listWriteTask);
            Task.WaitAll(listReadTask);
            var isRight = JudgeIsRight();
            buffer.Clear();
            if (!isRight) {
                Assert.Fail("数据不正确");
            }
        }
        
        Console.WriteLine($"test CostTime {sw.ElapsedMilliseconds}ms");
    }
}