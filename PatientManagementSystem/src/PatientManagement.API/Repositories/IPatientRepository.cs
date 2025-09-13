using PatientManagement.API.DTOs;
using PatientManagement.API.Models;

namespace PatientManagement.API.Repositories
{
    public interface IPatientRepository
    {
        #region CRUD Methods

        Task<int> AddAsync(Patient patient);
        Task<int> UpdateAsync(Patient patient);
        Task<int> DeleteAsync(int id);
        Task<Patient?> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> SearchAsync(int? minAge, int? maxAge, char? gender, string? city, string? condition);

        #endregion

        #region Reports

        /// <summary>
        /// Fetch all patients diagnosed in the last 6 months.
        /// </summary>
        Task<IEnumerable<Patient>> GetRecentPatientsAsync();

        /// <summary>
        /// Get the top 3 cities with maximum patients.
        /// </summary>
        Task<IEnumerable<dynamic>> GetTopCitiesAsync();

        /// <summary>
        /// Find patients who have more than 2 conditions.
        /// </summary>
        Task<IEnumerable<Patient>> GetPatientsWithMultipleConditionsAsync();

        /// <summary>
        /// Search patients dynamically using a stored procedure.
        /// </summary>
        Task<IEnumerable<Patient>> SearchPatientsUsingSPAsync(int? minAge, int? maxAge, string? condition, string? city);

        /// <summary>
        /// Get the average age of patients grouped by condition.
        /// </summary>
        Task<IEnumerable<dynamic>> GetAverageAgeByConditionAsync();
        Task<int> AddAsync(PatientCreateDto dto);
        Task<int> UpdateAsync(PatientUpdateDto dto);

        #endregion
    }
}
