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
		public int role_uid { get; set; }
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
}
