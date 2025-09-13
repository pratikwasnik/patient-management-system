/**  	Fetch all patients diagnosed in the last 6 months.*/

SELECT DISTINCT p.*
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
WHERE pc.DiagnosedDate >= DATEADD(MONTH, -6, GETDATE());