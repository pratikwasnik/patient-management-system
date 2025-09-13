#region Namespaces
using Microsoft.AspNetCore.Mvc;
using PatientManagement.API.DTOs;
using PatientManagement.API.Repositories;
#endregion

namespace PatientManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        #region Fields & Constructor

        private readonly IPatientRepository _repo;

        public PatientsController(IPatientRepository repo)
        {
            _repo = repo;
        }

        #endregion

        #region CRUD Operations

        
        // POST: api/patients/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] PatientCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = 400,
                    error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var id = await _repo.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
        }

        
        // PUT: api/patients/UpdateById/{id}
        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] PatientUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = 400,
                    error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            dto.Id = id;
            var rows = await _repo.UpdateAsync(dto);
            return Ok(new { Updated = rows });
        }

        
        // DELETE: api/patients/DeleteById/{id}
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var rows = await _repo.DeleteAsync(id);
            return Ok(new { Deleted = rows });
        }

        
        // GET: api/patients/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _repo.GetByIdAsync(id);
            if (patient == null) return NotFound(new { status = 404, error = "Patient not found" });
            return Ok(patient);
        }

        
        // GET: api/patients/Search
        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? minAge, int? maxAge, char? gender, string? city, string? condition)
        {
            var result = await _repo.SearchAsync(minAge, maxAge, gender, city, condition);
            return Ok(result);
        }

        #endregion

        #region Reports

        // GET: api/patients/GetRecentPatients
        [HttpGet("GetRecentPatients")]
        public async Task<IActionResult> GetRecentPatients()
        {
            var result = await _repo.GetRecentPatientsAsync();
            return Ok(result);
        }

        // GET: api/patients/GetTopCities
        [HttpGet("GetTopCities")]
        public async Task<IActionResult> GetTopCities()
        {
            var result = await _repo.GetTopCitiesAsync();
            return Ok(result);
        }

        // GET: api/patients/GetPatientsWithMultipleConditions
        [HttpGet("GetPatientsWithMultipleConditions")]
        public async Task<IActionResult> GetPatientsWithMultipleConditions()
        {
            var result = await _repo.GetPatientsWithMultipleConditionsAsync();
            return Ok(result);
        }

        // GET: api/patients/SearchPatientsSP
        [HttpGet("SearchPatientsSP")]
        public async Task<IActionResult> SearchPatientsSP(int? minAge, int? maxAge, string? condition, string? city)
        {
            var result = await _repo.SearchPatientsUsingSPAsync(minAge, maxAge, condition, city);
            return Ok(result);
        }

        // GET: api/patients/GetAverageAgeByCondition
        [HttpGet("GetAverageAgeByCondition")]
        public async Task<IActionResult> GetAverageAgeByCondition()
        {
            var result = await _repo.GetAverageAgeByConditionAsync();
            return Ok(result);
        }

        #endregion
    }
}
