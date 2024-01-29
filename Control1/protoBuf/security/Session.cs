// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: session.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from session.proto</summary>
public static partial class SessionReflection {

  #region Descriptor
  /// <summary>File descriptor for session.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static SessionReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Cg1zZXNzaW9uLnByb3RvGgpzZWMwLnByb3RvGgpzZWMxLnByb3RvGgpzZWMy",
          "LnByb3RvIpQBCgtTZXNzaW9uRGF0YRIiCgdzZWNfdmVyGAIgASgOMhEuU2Vj",
          "U2NoZW1lVmVyc2lvbhIcCgRzZWMwGAogASgLMgwuU2VjMFBheWxvYWRIABIc",
          "CgRzZWMxGAsgASgLMgwuU2VjMVBheWxvYWRIABIcCgRzZWMyGAwgASgLMgwu",
          "U2VjMlBheWxvYWRIAEIHCgVwcm90bypCChBTZWNTY2hlbWVWZXJzaW9uEg4K",
          "ClNlY1NjaGVtZTAQABIOCgpTZWNTY2hlbWUxEAESDgoKU2VjU2NoZW1lMhAC",
          "YgZwcm90bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { global::Sec0Reflection.Descriptor, global::Sec1Reflection.Descriptor, global::Sec2Reflection.Descriptor, },
        new pbr::GeneratedClrTypeInfo(new[] {typeof(global::SecSchemeVersion), }, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::SessionData), global::SessionData.Parser, new[]{ "SecVer", "Sec0", "Sec1", "Sec2" }, new[]{ "Proto" }, null, null, null)
        }));
  }
  #endregion

}
#region Enums
/// <summary>
/// Allowed values for the type of security
/// being used in a protocomm session 
/// </summary>
public enum SecSchemeVersion {
  /// <summary>
  ///!&lt; Unsecured - plaintext communication 
  /// </summary>
  [pbr::OriginalName("SecScheme0")] SecScheme0 = 0,
  /// <summary>
  ///!&lt; Security scheme 1 - Curve25519 + AES-256-CTR
  /// </summary>
  [pbr::OriginalName("SecScheme1")] SecScheme1 = 1,
  /// <summary>
  ///!&lt; Security scheme 2 - SRP6a + AES-256-GCM
  /// </summary>
  [pbr::OriginalName("SecScheme2")] SecScheme2 = 2,
}

#endregion

#region Messages
/// <summary>
/// Data structure exchanged when establishing
/// secure session between Host and Client 
/// </summary>
public sealed partial class SessionData : pb::IMessage<SessionData>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<SessionData> _parser = new pb::MessageParser<SessionData>(() => new SessionData());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<SessionData> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::SessionReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SessionData() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SessionData(SessionData other) : this() {
    secVer_ = other.secVer_;
    switch (other.ProtoCase) {
      case ProtoOneofCase.Sec0:
        Sec0 = other.Sec0.Clone();
        break;
      case ProtoOneofCase.Sec1:
        Sec1 = other.Sec1.Clone();
        break;
      case ProtoOneofCase.Sec2:
        Sec2 = other.Sec2.Clone();
        break;
    }

    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SessionData Clone() {
    return new SessionData(this);
  }

  /// <summary>Field number for the "sec_ver" field.</summary>
  public const int SecVerFieldNumber = 2;
  private global::SecSchemeVersion secVer_ = global::SecSchemeVersion.SecScheme0;
  /// <summary>
  ///!&lt; Type of security 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::SecSchemeVersion SecVer {
    get { return secVer_; }
    set {
      secVer_ = value;
    }
  }

  /// <summary>Field number for the "sec0" field.</summary>
  public const int Sec0FieldNumber = 10;
  /// <summary>
  ///!&lt; Payload data in case of security 0 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Sec0Payload Sec0 {
    get { return protoCase_ == ProtoOneofCase.Sec0 ? (global::Sec0Payload) proto_ : null; }
    set {
      proto_ = value;
      protoCase_ = value == null ? ProtoOneofCase.None : ProtoOneofCase.Sec0;
    }
  }

  /// <summary>Field number for the "sec1" field.</summary>
  public const int Sec1FieldNumber = 11;
  /// <summary>
  ///!&lt; Payload data in case of security 1 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Sec1Payload Sec1 {
    get { return protoCase_ == ProtoOneofCase.Sec1 ? (global::Sec1Payload) proto_ : null; }
    set {
      proto_ = value;
      protoCase_ = value == null ? ProtoOneofCase.None : ProtoOneofCase.Sec1;
    }
  }

  /// <summary>Field number for the "sec2" field.</summary>
  public const int Sec2FieldNumber = 12;
  /// <summary>
  ///!&lt; Payload data in case of security 2 
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::Sec2Payload Sec2 {
    get { return protoCase_ == ProtoOneofCase.Sec2 ? (global::Sec2Payload) proto_ : null; }
    set {
      proto_ = value;
      protoCase_ = value == null ? ProtoOneofCase.None : ProtoOneofCase.Sec2;
    }
  }

  private object proto_;
  /// <summary>Enum of possible cases for the "proto" oneof.</summary>
  public enum ProtoOneofCase {
    None = 0,
    Sec0 = 10,
    Sec1 = 11,
    Sec2 = 12,
  }
  private ProtoOneofCase protoCase_ = ProtoOneofCase.None;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ProtoOneofCase ProtoCase {
    get { return protoCase_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearProto() {
    protoCase_ = ProtoOneofCase.None;
    proto_ = null;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as SessionData);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(SessionData other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (SecVer != other.SecVer) return false;
    if (!object.Equals(Sec0, other.Sec0)) return false;
    if (!object.Equals(Sec1, other.Sec1)) return false;
    if (!object.Equals(Sec2, other.Sec2)) return false;
    if (ProtoCase != other.ProtoCase) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (SecVer != global::SecSchemeVersion.SecScheme0) hash ^= SecVer.GetHashCode();
    if (protoCase_ == ProtoOneofCase.Sec0) hash ^= Sec0.GetHashCode();
    if (protoCase_ == ProtoOneofCase.Sec1) hash ^= Sec1.GetHashCode();
    if (protoCase_ == ProtoOneofCase.Sec2) hash ^= Sec2.GetHashCode();
    hash ^= (int) protoCase_;
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (SecVer != global::SecSchemeVersion.SecScheme0) {
      output.WriteRawTag(16);
      output.WriteEnum((int) SecVer);
    }
    if (protoCase_ == ProtoOneofCase.Sec0) {
      output.WriteRawTag(82);
      output.WriteMessage(Sec0);
    }
    if (protoCase_ == ProtoOneofCase.Sec1) {
      output.WriteRawTag(90);
      output.WriteMessage(Sec1);
    }
    if (protoCase_ == ProtoOneofCase.Sec2) {
      output.WriteRawTag(98);
      output.WriteMessage(Sec2);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (SecVer != global::SecSchemeVersion.SecScheme0) {
      output.WriteRawTag(16);
      output.WriteEnum((int) SecVer);
    }
    if (protoCase_ == ProtoOneofCase.Sec0) {
      output.WriteRawTag(82);
      output.WriteMessage(Sec0);
    }
    if (protoCase_ == ProtoOneofCase.Sec1) {
      output.WriteRawTag(90);
      output.WriteMessage(Sec1);
    }
    if (protoCase_ == ProtoOneofCase.Sec2) {
      output.WriteRawTag(98);
      output.WriteMessage(Sec2);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (SecVer != global::SecSchemeVersion.SecScheme0) {
      size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) SecVer);
    }
    if (protoCase_ == ProtoOneofCase.Sec0) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Sec0);
    }
    if (protoCase_ == ProtoOneofCase.Sec1) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Sec1);
    }
    if (protoCase_ == ProtoOneofCase.Sec2) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Sec2);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(SessionData other) {
    if (other == null) {
      return;
    }
    if (other.SecVer != global::SecSchemeVersion.SecScheme0) {
      SecVer = other.SecVer;
    }
    switch (other.ProtoCase) {
      case ProtoOneofCase.Sec0:
        if (Sec0 == null) {
          Sec0 = new global::Sec0Payload();
        }
        Sec0.MergeFrom(other.Sec0);
        break;
      case ProtoOneofCase.Sec1:
        if (Sec1 == null) {
          Sec1 = new global::Sec1Payload();
        }
        Sec1.MergeFrom(other.Sec1);
        break;
      case ProtoOneofCase.Sec2:
        if (Sec2 == null) {
          Sec2 = new global::Sec2Payload();
        }
        Sec2.MergeFrom(other.Sec2);
        break;
    }

    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 16: {
          SecVer = (global::SecSchemeVersion) input.ReadEnum();
          break;
        }
        case 82: {
          global::Sec0Payload subBuilder = new global::Sec0Payload();
          if (protoCase_ == ProtoOneofCase.Sec0) {
            subBuilder.MergeFrom(Sec0);
          }
          input.ReadMessage(subBuilder);
          Sec0 = subBuilder;
          break;
        }
        case 90: {
          global::Sec1Payload subBuilder = new global::Sec1Payload();
          if (protoCase_ == ProtoOneofCase.Sec1) {
            subBuilder.MergeFrom(Sec1);
          }
          input.ReadMessage(subBuilder);
          Sec1 = subBuilder;
          break;
        }
        case 98: {
          global::Sec2Payload subBuilder = new global::Sec2Payload();
          if (protoCase_ == ProtoOneofCase.Sec2) {
            subBuilder.MergeFrom(Sec2);
          }
          input.ReadMessage(subBuilder);
          Sec2 = subBuilder;
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 16: {
          SecVer = (global::SecSchemeVersion) input.ReadEnum();
          break;
        }
        case 82: {
          global::Sec0Payload subBuilder = new global::Sec0Payload();
          if (protoCase_ == ProtoOneofCase.Sec0) {
            subBuilder.MergeFrom(Sec0);
          }
          input.ReadMessage(subBuilder);
          Sec0 = subBuilder;
          break;
        }
        case 90: {
          global::Sec1Payload subBuilder = new global::Sec1Payload();
          if (protoCase_ == ProtoOneofCase.Sec1) {
            subBuilder.MergeFrom(Sec1);
          }
          input.ReadMessage(subBuilder);
          Sec1 = subBuilder;
          break;
        }
        case 98: {
          global::Sec2Payload subBuilder = new global::Sec2Payload();
          if (protoCase_ == ProtoOneofCase.Sec2) {
            subBuilder.MergeFrom(Sec2);
          }
          input.ReadMessage(subBuilder);
          Sec2 = subBuilder;
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
