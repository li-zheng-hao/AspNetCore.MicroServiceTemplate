namespace MST.Infra.Rpc.Rest;

public class LoginRequestDto
{
    public string grant_type { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}