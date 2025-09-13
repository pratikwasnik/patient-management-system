/*•	Find patients who have more than 2 conditions.*/

SELECT p.Id, p.FirstName, p.LastName, p.Email, COUNT(pc.ConditionId) AS ConditionCount
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
GROUP BY p.Id, p.FirstName, p.LastName, p.Email
HAVING COUNT(pc.ConditionId) > 2;