using Microsoft.AspNetCore.Authorization;

public class IsADMRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public IsADMRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}