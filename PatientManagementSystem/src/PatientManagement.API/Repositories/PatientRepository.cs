
#region Namespaces
using Dapper;
using PatientManagement.API.DTOs;
using PatientManagement.API.Models;
using System.Data;
#endregion

namespace PatientManagement.API.Repositories
{
    public class PatientRepository : IPatientRepository
    {

        #region Fields & Constructor
        private readonly IDbConnection _db;

        public PatientRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }


        #endregion

        #region CRUD Methods

        public async Task<int> AddAsync(Patient patient)
        {
            var sql = @"
INSERT INTO Patients (FirstName, LastName, DOB, Gender, City, Email, Phone)
VALUES (@FirstName, @LastName, @DOB, @Gender, @City, @Email, @Phone);
SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await _db.ExecuteScalarAsync<int>(sql, patient);
            return id;
        }

        public async Task<int> UpdateAsync(Patient patient)
        {
            var sql = @"
UPDATE Patients 
SET FirstName=@FirstName, LastName=@LastName, DOB=@DOB, Gender=@Gender, City=@City, Email=@Email, Phone=@Phone
WHERE Id=@Id;";
            return await _db.ExecuteAsync(sql, patient);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Patients WHERE Id=@Id;";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Patients WHERE Id=@Id;";
            return await _db.QueryFirstOrDefaultAsync<Patient>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Patient>> SearchAsync(int? minAge, int? maxAge, char? gender, string? city, string? condition)
        {
            var sql = @"
SELECT p.* 
FROM Patients p
LEFT JOIN PatientConditions pc ON p.Id = pc.PatientId
LEFT JOIN Conditions c ON pc.ConditionId = c.Id
WHERE 1=1
";
            var parameters = new DynamicParameters();

            if (minAge.HasValue)
            {
                sql += " AND DATEDIFF(YEAR, p.DOB, GETDATE()) >= @MinAge";
                parameters.Add("MinAge", minAge.Value);
            }
            if (maxAge.HasValue)
            {
                sql += " AND DATEDIFF(YEAR, p.DOB, GETDATE()) <= @MaxAge";
                parameters.Add("MaxAge", maxAge.Value);
            }
            if (gender.HasValue)
            {
                sql += " AND p.Gender = @Gender";
                parameters.Add("Gender", gender.Value);
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                sql += " AND p.City = @City";
                parameters.Add("City", city);
            }
            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql += " AND c.Name = @Condition";
                parameters.Add("Condition", condition);
            }

            sql += " GROUP BY p.Id, p.FirstName, p.LastName, p.DOB, p.Gender, p.City, p.Email, p.Phone";

            return await _db.QueryAsync<Patient>(sql, parameters);
        }

        #endregion
        
        #region Report Methods

        public async Task<IEnumerable<Patient>> GetRecentPatientsAsync()
        {
            var sql = @"
SELECT 
    p.Id,
    p.FirstName,
    p.LastName,
    p.DOB,
    p.Gender,
    p.City,
    p.Email,
    p.Phone,
    pc.DiagnosedDate,
    c.Name AS ConditionName,
    c.Description AS ConditionDescription
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
JOIN Conditions c ON pc.ConditionId = c.Id
WHERE pc.DiagnosedDate >= DATEADD(MONTH, -6, GETDATE())
ORDER BY pc.DiagnosedDate DESC;;";
            return await _db.QueryAsync<Patient>(sql);
        }

        public async Task<IEnumerable<dynamic>> GetTopCitiesAsync()
        {
            var sql = @"
SELECT TOP 3 City, COUNT(*) AS PatientCount
FROM Patients
GROUP BY City
ORDER BY COUNT(*) DESC;";
            return await _db.QueryAsync<dynamic>(sql);
        }

        public async Task<IEnumerable<Patient>> GetPatientsWithMultipleConditionsAsync()
        {
            var sql = @"
SELECT p.*
FROM Patients p
JOIN (
    SELECT PatientId
    FROM PatientConditions
    GROUP BY PatientId
    HAVING COUNT(*) > 2
) c ON p.Id = c.PatientId;";
            return await _db.QueryAsync<Patient>(sql);
        }

        public async Task<IEnumerable<Patient>> SearchPatientsUsingSPAsync(int? minAge, int? maxAge, string? condition, string? city)
        {
            return await _db.QueryAsync<Patient>(
                "sp_SearchPatients",
                new { MinAge = minAge, MaxAge = maxAge, Condition = condition, City = city },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<dynamic>> GetAverageAgeByConditionAsync()
        {
            var sql = @"
SELECT c.Name AS ConditionName,
       AVG(DATEDIFF(YEAR, p.DOB, GETDATE())) AS AverageAge
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
JOIN Conditions c ON pc.ConditionId = c.Id
GROUP BY c.Name;";
            return await _db.QueryAsync<dynamic>(sql);
        }

        public async Task<int> AddAsync(PatientCreateDto dto)
        {
            var sql = @"
        INSERT INTO Patients (FirstName, LastName, Dob, Gender, City, Email, Phone)
        OUTPUT INSERTED.Id
        VALUES (@FirstName, @LastName, @Dob, @Gender, @City, @Email, @Phone)";
          
            var id = await _db.ExecuteScalarAsync<int>(sql, dto);
            return id;
        }

        public async Task<int> UpdateAsync(PatientUpdateDto dto)
        {
            var sql = @"
        UPDATE Patients
        SET FirstName = @FirstName,
            LastName = @LastName,
            DOB = @DOB,
            Gender = @Gender,
            City = @City,
            Email = @Email,
            Phone = @Phone
        WHERE Id = @Id";
            
            var rows = await _db.ExecuteAsync(sql, dto);
            return rows;
        }
        #endregion
    }
}
