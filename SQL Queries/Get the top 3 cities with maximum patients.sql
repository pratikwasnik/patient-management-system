/*•	Get the top 3 cities with maximum patients.*/


SELECT TOP 3 
    City, 
    COUNT(*) AS PatientCount
FROM Patients
WHERE City IS NOT NULL
GROUP BY City
ORDER BY PatientCount DESC;
