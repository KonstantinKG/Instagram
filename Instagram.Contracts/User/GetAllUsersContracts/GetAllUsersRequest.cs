using System.ComponentModel.DataAnnotations;

namespace Instagram.Contracts.User.GetAllUsersContracts;

public record GetAllUsersRequest(
    int page
    );