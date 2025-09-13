namespace PatientManagement.API.DTOs
{
    public class PatientCreateDto
    {
        #region Properties
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DOB { get; set; }
        public char Gender { get; set; }
        public string? City { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        #endregion
    }
}
