using BuildingBlocks.Application.StaticDetails;

namespace AuthService.Api.StaticDetails;

public class StaticDetail : GlobalStaticDetail
{
    public static string CLAIM_TYPE_TOKEN_TYPE = "token_type";
    public static string CLAIM_TYPE_PERMISSIONS = "permission";
    public static string CLAIM_TYPE_SCOPES = "scope";

    public static string TOKEN_TYPE_ACCESS = "access";
    public static string TOKEN_TYPE_REFRESH = "refresh";
}
