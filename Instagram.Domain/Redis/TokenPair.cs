﻿namespace Instagram.Domain.Redis;

public class TokenPair
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}