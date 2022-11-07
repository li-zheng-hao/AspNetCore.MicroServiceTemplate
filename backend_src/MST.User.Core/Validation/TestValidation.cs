using FluentValidation;
using Quickwire.Attributes;

namespace MST.User.Webapi.Validation;

public class Person 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

public class TestValidation : AbstractValidator<Person> 
{
    public TestValidation() 
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).Length(0, 10);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Age).InclusiveBetween(18, 60);
    }
}