using API.Entities;

namespace API.Interfaces;

public interface IJwtService
{
    string CreateJwt(User user);
}