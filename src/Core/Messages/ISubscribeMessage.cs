using System;

public interface ISubscribeMessage
{
    object Message { get; }
    Type MessageType { get; }
}