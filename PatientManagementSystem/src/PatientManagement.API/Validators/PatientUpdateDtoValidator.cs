#region Namespaces
using FluentValidation;
using PatientManagement.API.DTOs;
#endregion

namespace PatientManagement.API.Validators
{
    public class PatientUpdateDtoValidator : AbstractValidator<PatientUpdateDto>
    {
        public PatientUpdateDtoValidator()
        {
            Include(new PatientCreateDtoValidator());
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
