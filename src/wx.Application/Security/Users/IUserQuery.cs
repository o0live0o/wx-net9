namespace wx.Application.Security.Users;

public interface IUserQuery
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}

public record UserDto(string Account);