/* •	Write a stored procedure to search patients dynamically (filters: age range, condition, city).*/


CREATE OR ALTER PROCEDURE sp_SearchPatients
    @MinAge INT = NULL,
    @MaxAge INT = NULL,
    @Condition VARCHAR(100) = NULL,
    @City VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT DISTINCT p.Id, p.FirstName, p.LastName, p.DOB, p.Gender, 
               p.City, p.Email, p.Phone
        FROM Patients p
        LEFT JOIN PatientConditions pc ON p.Id = pc.PatientId
        LEFT JOIN Conditions c ON pc.ConditionId = c.Id
        WHERE ( @MinAge IS NULL OR DATEDIFF(YEAR, p.DOB, GETDATE()) >= @MinAge )
          AND ( @MaxAge IS NULL OR DATEDIFF(YEAR, p.DOB, GETDATE()) <= @MaxAge )
          AND ( @City IS NULL OR p.City = @City )
          AND ( @Condition IS NULL OR c.Name = @Condition )
        ORDER BY p.LastName, p.FirstName;
    END TRY
    BEGIN CATCH
        -- Return error details
        SELECT  
            ERROR_NUMBER()   AS ErrorNumber,
            ERROR_MESSAGE()  AS ErrorMessage,
            ERROR_LINE()     AS ErrorLine,
            ERROR_PROCEDURE() AS ErrorProcedure;
    END CATCH
END;
GO

/*example below*/
EXEC sp_SearchPatients @MinAge = 20, @MaxAge = 40 , @City = 'Nagpur';