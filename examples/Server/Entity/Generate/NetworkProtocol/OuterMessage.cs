using ProtoBuf;

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantOverriddenMember
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618

namespace Fantasy
{	
	[ProtoContract]
	public partial class Send_RegisterAccount : AMessage, IRequest, IProto
	{
		public static Send_RegisterAccount Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_RegisterAccount>();
		}
		public override void Dispose()
		{
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_RegisterAccount>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_RegisterAccount ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_RegisterAccount; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_RegisterAccount : AMessage, IResponse, IProto
	{
		public static Rcv_RegisterAccount Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_RegisterAccount>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_RegisterAccount>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_RegisterAccount; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_GetLoginToken : AMessage, IRequest, IProto
	{
		public static Send_GetLoginToken Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_GetLoginToken>();
		}
		public override void Dispose()
		{
			account_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_GetLoginToken>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_GetLoginToken ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_GetLoginToken; }
		[ProtoMember(1)]
		public string account_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_GetLoginToken : AMessage, IResponse, IProto
	{
		public static Rcv_GetLoginToken Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_GetLoginToken>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			token = default;
			login_address = default;
			account_id = default;
			scene_config_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_GetLoginToken>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_GetLoginToken; }
		[ProtoMember(1)]
		public string token { get; set; }
		[ProtoMember(2)]
		public string login_address { get; set; }
		[ProtoMember(3)]
		public long account_id { get; set; }
		[ProtoMember(4)]
		public uint scene_config_id { get; set; }
		[ProtoMember(5)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_LoginGate : AMessage, IRequest, IProto
	{
		public static Send_LoginGate Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_LoginGate>();
		}
		public override void Dispose()
		{
			account_id = default;
			token = default;
			scene_config_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_LoginGate>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_LoginGate ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_LoginGate; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public string token { get; set; }
		[ProtoMember(3)]
		public uint scene_config_id { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_LoginGate : AMessage, IResponse, IProto
	{
		public static Rcv_LoginGate Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_LoginGate>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			account_id = default;
			level = default;
			gold = default;
			diamond = default;
			role_datas.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_LoginGate>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_LoginGate; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public long level { get; set; }
		[ProtoMember(3)]
		public long gold { get; set; }
		[ProtoMember(4)]
		public long diamond { get; set; }
		[ProtoMember(5)]
		public List<RoleData> role_datas = new List<RoleData>();
		[ProtoMember(6)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class RoleData : AMessage, IProto
	{
		public static RoleData Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<RoleData>();
		}
		public override void Dispose()
		{
			uid = default;
			role_name = default;
			level = default;
			role_id = default;
			account_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<RoleData>(this);
#endif
		}
		[ProtoMember(1)]
		public long uid { get; set; }
		[ProtoMember(2)]
		public string role_name { get; set; }
		[ProtoMember(3)]
		public int level { get; set; }
		[ProtoMember(4)]
		public int role_id { get; set; }
		[ProtoMember(5)]
		public long account_id { get; set; }
	}
	[ProtoContract]
	public partial class Send_CreateRole : AMessage, IRequest, IProto
	{
		public static Send_CreateRole Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_CreateRole>();
		}
		public override void Dispose()
		{
			role_id = default;
			account_id = default;
			role_name = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_CreateRole>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_CreateRole ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_CreateRole; }
		[ProtoMember(1)]
		public int role_id { get; set; }
		[ProtoMember(2)]
		public long account_id { get; set; }
		[ProtoMember(3)]
		public string role_name { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_CreateRole : AMessage, IResponse, IProto
	{
		public static Rcv_CreateRole Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_CreateRole>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			role_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_CreateRole>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_CreateRole; }
		[ProtoMember(1)]
		public RoleData role_data { get; set; }
		[ProtoMember(2)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_SelectRole : AMessage, IRequest, IProto
	{
		public static Send_SelectRole Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_SelectRole>();
		}
		public override void Dispose()
		{
			role_uid = default;
			account_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_SelectRole>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_SelectRole ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_SelectRole; }
		[ProtoMember(1)]
		public long role_uid { get; set; }
		[ProtoMember(2)]
		public long account_id { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_SelectRole : AMessage, IResponse, IProto
	{
		public static Rcv_SelectRole Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_SelectRole>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			role_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_SelectRole>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_SelectRole; }
		[ProtoMember(1)]
		public RoleData role_data { get; set; }
		[ProtoMember(2)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_EnterMap : AMessage, IRequest, IProto
	{
		public static Send_EnterMap Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_EnterMap>();
		}
		public override void Dispose()
		{
			player_id = default;
			cur_map = default;
			map_type = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_EnterMap>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_EnterMap ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_EnterMap; }
		[ProtoMember(1)]
		public long player_id { get; set; }
		[ProtoMember(2)]
		public int cur_map { get; set; }
		[ProtoMember(3)]
		public int map_type { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_EnterMap : AMessage, IResponse, IProto
	{
		public static Rcv_EnterMap Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_EnterMap>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			player_id = default;
			map_type = default;
			role_init_pos = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_EnterMap>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_EnterMap; }
		[ProtoMember(1)]
		public long player_id { get; set; }
		[ProtoMember(2)]
		public int map_type { get; set; }
		[ProtoMember(3)]
		public CSVector3 role_init_pos { get; set; }
		[ProtoMember(4)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_CreateTeam : AMessage, IRequest, IProto
	{
		public static Send_CreateTeam Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_CreateTeam>();
		}
		public override void Dispose()
		{
			account_id = default;
			map_type = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_CreateTeam>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_CreateTeam ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_CreateTeam; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public int map_type { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_CreateTeam : AMessage, IResponse, IProto
	{
		public static Rcv_CreateTeam Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_CreateTeam>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			team_id = default;
			role_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_CreateTeam>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_CreateTeam; }
		[ProtoMember(1)]
		public int team_id { get; set; }
		[ProtoMember(2)]
		public RoleData role_data { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_JoinTeam : AMessage, IRequest, IProto
	{
		public static Send_JoinTeam Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_JoinTeam>();
		}
		public override void Dispose()
		{
			account_id = default;
			map_type = default;
			team_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_JoinTeam>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_JoinTeam ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_JoinTeam; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public int map_type { get; set; }
		[ProtoMember(3)]
		public int team_id { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_JoinTeam : AMessage, IResponse, IProto
	{
		public static Rcv_JoinTeam Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_JoinTeam>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			team_id = default;
			team_role_list.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_JoinTeam>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_JoinTeam; }
		[ProtoMember(1)]
		public int team_id { get; set; }
		[ProtoMember(2)]
		public List<RoleData> team_role_list = new List<RoleData>();
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Msg_TeamStateChanged : AMessage, IMessage, IProto
	{
		public static Msg_TeamStateChanged Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_TeamStateChanged>();
		}
		public override void Dispose()
		{
			team_state = default;
			role_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_TeamStateChanged>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_TeamStateChanged; }
		[ProtoMember(1)]
		public int team_state { get; set; }
		[ProtoMember(2)]
		public RoleData role_data { get; set; }
	}
	[ProtoContract]
	public partial class Send_StateSync : AMessage, IRequest, IProto
	{
		public static Send_StateSync Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_StateSync>();
		}
		public override void Dispose()
		{
			state_pack_id = default;
			role_sync_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_StateSync>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_StateSync ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_StateSync; }
		[ProtoMember(1)]
		public long state_pack_id { get; set; }
		[ProtoMember(2)]
		public StateSyncData role_sync_data { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_StateSync : AMessage, IResponse, IProto
	{
		public static Rcv_StateSync Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_StateSync>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			state_pack_id = default;
			role_sync_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_StateSync>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_StateSync; }
		[ProtoMember(1)]
		public long state_pack_id { get; set; }
		[ProtoMember(2)]
		public StateSyncData role_sync_data { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Msg_EnterDungeon : AMessage, IMessage, IProto
	{
		public static Msg_EnterDungeon Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_EnterDungeon>();
		}
		public override void Dispose()
		{
			team_id = default;
			dungeonCfgID = default;
			teamMembers.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_EnterDungeon>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_EnterDungeon; }
		[ProtoMember(1)]
		public int team_id { get; set; }
		[ProtoMember(2)]
		public int dungeonCfgID { get; set; }
		[ProtoMember(3)]
		public List<RoleData> teamMembers = new List<RoleData>();
	}
	[ProtoContract]
	public partial class Msg_LoadDungeonProgress : AMessage, IMessage, IProto
	{
		public static Msg_LoadDungeonProgress Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_LoadDungeonProgress>();
		}
		public override void Dispose()
		{
			team_id = default;
			account_id = default;
			progress = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_LoadDungeonProgress>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_LoadDungeonProgress; }
		[ProtoMember(1)]
		public int team_id { get; set; }
		[ProtoMember(2)]
		public long account_id { get; set; }
		[ProtoMember(3)]
		public float progress { get; set; }
	}
	[ProtoContract]
	public partial class Msg_StartDungeonBattle : AMessage, IMessage, IProto
	{
		public static Msg_StartDungeonBattle Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_StartDungeonBattle>();
		}
		public override void Dispose()
		{
			battle_role_datas.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_StartDungeonBattle>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_StartDungeonBattle; }
		[ProtoMember(1)]
		public List<RoleData> battle_role_datas = new List<RoleData>();
	}
	[ProtoContract]
	public partial class StateSyncData : AMessage, IProto
	{
		public static StateSyncData Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<StateSyncData>();
		}
		public override void Dispose()
		{
			player_id = default;
			map_type = default;
			position = default;
			input_dir = default;
			state = default;
			role_id = default;
			player_map_status = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<StateSyncData>(this);
#endif
		}
		[ProtoMember(1)]
		public long player_id { get; set; }
		[ProtoMember(2)]
		public int map_type { get; set; }
		[ProtoMember(3)]
		public CSVector3 position { get; set; }
		[ProtoMember(4)]
		public CSVector3 input_dir { get; set; }
		[ProtoMember(5)]
		public int state { get; set; }
		[ProtoMember(6)]
		public int role_id { get; set; }
		[ProtoMember(7)]
		public int player_map_status { get; set; }
	}
	[ProtoContract]
	public partial class Msg_OtherPlayerStateSync : AMessage, IMessage, IProto
	{
		public static Msg_OtherPlayerStateSync Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_OtherPlayerStateSync>();
		}
		public override void Dispose()
		{
			role_data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_OtherPlayerStateSync>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_OtherPlayerStateSync; }
		[ProtoMember(1)]
		public StateSyncData role_data { get; set; }
	}
	[ProtoContract]
	public partial class CSVector3 : AMessage, IProto
	{
		public static CSVector3 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<CSVector3>();
		}
		public override void Dispose()
		{
			x = default;
			y = default;
			z = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<CSVector3>(this);
#endif
		}
		[ProtoMember(1)]
		public float x { get; set; }
		[ProtoMember(2)]
		public float y { get; set; }
		[ProtoMember(3)]
		public float z { get; set; }
	}
	[ProtoContract]
	public partial class CSFixIntVector3 : AMessage, IProto
	{
		public static CSFixIntVector3 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<CSFixIntVector3>();
		}
		public override void Dispose()
		{
			x = default;
			y = default;
			z = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<CSFixIntVector3>(this);
#endif
		}
		[ProtoMember(1)]
		public int x { get; set; }
		[ProtoMember(2)]
		public int y { get; set; }
		[ProtoMember(3)]
		public int z { get; set; }
	}
	[ProtoContract]
	public partial class Msg_C2S_FrameOpEvent : AMessage, IMessage, IProto
	{
		public static Msg_C2S_FrameOpEvent Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_C2S_FrameOpEvent>();
		}
		public override void Dispose()
		{
			battle_id = default;
			frame_operate_datas.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_C2S_FrameOpEvent>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_C2S_FrameOpEvent; }
		[ProtoMember(1)]
		public long battle_id { get; set; }
		[ProtoMember(2)]
		public List<FrameOperateData> frame_operate_datas = new List<FrameOperateData>();
	}
	[ProtoContract]
	public partial class Msg_S2C_FrameOpEvent : AMessage, IMessage, IProto
	{
		public static Msg_S2C_FrameOpEvent Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Msg_S2C_FrameOpEvent>();
		}
		public override void Dispose()
		{
			battle_id = default;
			frame_operate_datas.Clear();
			logic_frame_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Msg_S2C_FrameOpEvent>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Msg_S2C_FrameOpEvent; }
		[ProtoMember(1)]
		public long battle_id { get; set; }
		[ProtoMember(2)]
		public List<FrameOperateData> frame_operate_datas = new List<FrameOperateData>();
		[ProtoMember(3)]
		public long logic_frame_id { get; set; }
	}
	[ProtoContract]
	public partial class FrameOperateData : AMessage, IProto
	{
		public static FrameOperateData Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<FrameOperateData>();
		}
		public override void Dispose()
		{
			operate_type = default;
			input_dir = default;
			skillId = default;
			skillType = default;
			skillPos = default;
			account_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<FrameOperateData>(this);
#endif
		}
		[ProtoMember(1)]
		public int operate_type { get; set; }
		[ProtoMember(2)]
		public CSFixIntVector3 input_dir { get; set; }
		[ProtoMember(3)]
		public int skillId { get; set; }
		[ProtoMember(4)]
		public int skillType { get; set; }
		[ProtoMember(5)]
		public CSFixIntVector3 skillPos { get; set; }
		[ProtoMember(6)]
		public long account_id { get; set; }
	}
}
