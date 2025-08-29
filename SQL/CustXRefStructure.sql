
IF OBJECT_ID('RawData.AMS_CustXRef') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_CustXRef
END
GO

CREATE TABLE RawData.AMS_CustXRef(

	CustXRefChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AXRefId VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CustId VARCHAR(255),
	CXRefId VARCHAR(255),
	EnteredDate DATETIME,
	XReference VARCHAR(255),
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


IF OBJECT_ID('Staging.AMS_CustXRef') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_CustXRef
END
GO

CREATE TABLE Staging.AMS_CustXRef(

	CustXRefChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AXRefId VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CustId VARCHAR(255),
	CXRefId VARCHAR(255),
	EnteredDate DATETIME,
	XReference VARCHAR(255),
	StagingDate DATETIME, 
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO



CREATE CLUSTERED INDEX cix_CustXRef_CustXRefId ON Staging.AMS_CustXRef(CXRefId)
CREATE CLUSTERED INDEX cix_CustXRef_CustXRefId ON RawData.AMS_CustXRef(CXRefId)

GO

IF OBJECT_ID('Staging.AMS_CustXRef_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_CustXRef_Load
END
GO

CREATE PROCEDURE Staging.AMS_CustXRef_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_CustXRef
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_CustXRef AS RawData
                         WHERE   RawData.CXRefId = AMS_CustXRef.CXRefId
                   );

    INSERT INTO Staging.AMS_CustXRef (
        CustXRefChecksum
		, Datasource
		, Deleted
		, Sequencenumber
		, AXRefId
		, ChangedBy
		, ChangedDate
		, CustId
		, CXRefId
		, EnteredDate
		, XReference
        , StagingDate
        , InsertDate
    )
    SELECT AMS_CustXRef.CustXRefChecksum
		, AMS_CustXRef.Datasource
		, AMS_CustXRef.Deleted
		, AMS_CustXRef.Sequencenumber
		, AMS_CustXRef.AXRefId
		, AMS_CustXRef.ChangedBy
		, AMS_CustXRef.ChangedDate
		, AMS_CustXRef.CustId
		, AMS_CustXRef.CXRefId
		, AMS_CustXRef.EnteredDate
		, AMS_CustXRef.XReference
        , AMS_CustXRef.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.AMS_CustXRef;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_CustXRef.CXRefId
                                                  ORDER BY AMS_CustXRef.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_CustXRef
            ) AS AMS_CustXRef
     WHERE  AMS_CustXRef.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

-- GRANT CONTROL ON Staging.AMS_CustXRef TO [abeerconsulting];
-- GRANT CONTROL ON RawData.AMS_CustXRef TO [abeerconsulting];
GO

GO