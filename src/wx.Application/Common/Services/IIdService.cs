namespace wx.Application.Common.Interfaces;

public interface IIdService
{
    int GenerateId(IdType type);
}

public enum IdType
{
    User,
}