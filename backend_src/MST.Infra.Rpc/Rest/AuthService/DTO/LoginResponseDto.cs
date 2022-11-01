namespace MST.Infra.Rpc.Rest;

public class LoginResponseDto
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string token_type { get; set; }
    public string refresh_token { get; set; }
    public string sope { get; set; }
}