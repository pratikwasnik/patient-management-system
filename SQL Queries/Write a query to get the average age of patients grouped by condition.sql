/* •	Write a query to get the average age of patients grouped by condition. */


SELECT 
    c.Name AS ConditionName,
    AVG(DATEDIFF(YEAR, p.DOB, GETDATE())) AS AverageAge
FROM Patients p
JOIN PatientConditions pc ON p.Id = pc.PatientId
JOIN Conditions c ON pc.ConditionId = c.Id
GROUP BY c.Name;
