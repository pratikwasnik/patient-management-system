#region Namespaces
using FluentValidation;
using PatientManagement.API.DTOs;
#endregion

namespace PatientManagement.API.Validators
{
    public class PatientCreateDtoValidator : AbstractValidator<PatientCreateDto>
    {
        #region Constructor

        public PatientCreateDtoValidator()
        {
            #region Name Rules
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);
            #endregion

            #region DOB Rules
            RuleFor(x => x.DOB)
                .LessThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("DOB cannot be in the future");
            #endregion

            #region Gender Rules
            RuleFor(x => x.Gender)
                .Must(g => g == 'M' || g == 'F' || g == 'O')
                .WithMessage("Gender must be M, F, or O");
            #endregion

            #region Email Rules
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);
            #endregion

            #region Phone Rules
            RuleFor(x => x.Phone)
                .MaximumLength(15);
            #endregion
        }

        #endregion
    }
}
