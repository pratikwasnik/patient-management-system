#region Namespaces
using PatientManagement.API.DTOs;
using PatientManagement.API.Models;
#endregion

namespace PatientManagement.API.Services
{
    public interface IPatientService
    {
        #region CRUD Methods
        Task<int> CreateAsync(PatientCreateDto dto);
        Task<int> UpdateAsync(PatientUpdateDto dto);
        Task<int> DeleteAsync(int id);
        Task<Patient?> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> SearchAsync(int? minAge, int? maxAge, char? gender, string? city, string? condition);
        #endregion
    }
}
