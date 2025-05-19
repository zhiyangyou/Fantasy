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
	public partial class Send_Test1 : AMessage, IRequest, IProto
	{
		public static Send_Test1 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_Test1>();
		}
		public override void Dispose()
		{
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_Test1>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_Test1 ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_Test1; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_Test1 : AMessage, IResponse, IProto
	{
		public static Rcv_Test1 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_Test1>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			success = default;
			error_msg = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_Test1>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_Test1; }
		[ProtoMember(1)]
		public bool success { get; set; }
		[ProtoMember(2)]
		public string error_msg { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class C2G_Test2 : AMessage, IMessage, IProto
	{
		public static C2G_Test2 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<C2G_Test2>();
		}
		public override void Dispose()
		{
			msg_content = default;
			frameOpCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<C2G_Test2>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.C2G_Test2; }
		[ProtoMember(1)]
		public string msg_content { get; set; }
		[ProtoMember(2)]
		public long frameOpCode { get; set; }
	}
	[ProtoContract]
	public partial class G2C_Test2 : AMessage, IMessage, IProto
	{
		public static G2C_Test2 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2C_Test2>();
		}
		public override void Dispose()
		{
			msg_content = default;
			frameOpCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2C_Test2>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.G2C_Test2; }
		[ProtoMember(1)]
		public string msg_content { get; set; }
		[ProtoMember(2)]
		public long frameOpCode { get; set; }
	}
}
