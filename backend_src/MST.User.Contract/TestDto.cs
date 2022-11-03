using System.ComponentModel.DataAnnotations;

namespace MST.User.Contract;

public class TestDto
{
    
    public string name { get; set; }
    [Range(1,10,ErrorMessage = "范围在1-10之间")]
    public int age { get; set; }
}