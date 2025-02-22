namespace AuthLab.Api.Data.Models;

public class Role : BaseEntity
{
    public string Name { get; set; } = CommonConstants.Roles.User;
}
