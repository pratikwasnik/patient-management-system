using PatientManagement.API.DTOs;
using PatientManagement.API.Models;
using PatientManagement.API.Repositories;

namespace PatientManagement.API.Services
{
    public class PatientService : IPatientService
    {
        #region Fields & Constructor
        private readonly IPatientRepository _repo;

        public PatientService(IPatientRepository repo)
        {
            _repo = repo;
        }
        #endregion

        #region CRUD Methods
        public async Task<int> CreateAsync(PatientCreateDto dto)
        {
            if (dto.DOB > DateTime.UtcNow.Date) throw new ArgumentException("DOB cannot be in the future");
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Gender = dto.Gender,
                City = dto.City,
                Email = dto.Email,
                Phone = dto.Phone
            };            
            return await _repo.AddAsync(patient);
        }

        public async Task<int> UpdateAsync(PatientUpdateDto dto)
        {
            if (dto.DOB > DateTime.UtcNow.Date) throw new ArgumentException("DOB cannot be in the future");
            var patient = new Patient
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Gender = dto.Gender,
                City = dto.City,
                Email = dto.Email,
                Phone = dto.Phone
            };
            return await _repo.UpdateAsync(patient);
        }

        public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<Patient?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Patient>> SearchAsync(int? minAge, int? maxAge, char? gender, string? city, string? condition)
            => await _repo.SearchAsync(minAge, maxAge, gender, city, condition);
        #endregion
    }
}
