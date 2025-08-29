
IF OBJECT_ID('RawData.AMS_130Policy') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_130Policy
END
GO

CREATE TABLE RawData.AMS_130Policy(

	PolicyChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AddInfo VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	DividendPlan VARCHAR(255),
	EffDate DATETIME,
	EmployerNo VARCHAR(255),
	EnteredDate DATETIME,
	IncludeExclude VARCHAR(255),
	IsPart1 VARCHAR(255),
	IsParticipating VARCHAR(255),
	IsSafetyGroup VARCHAR(255),
	LOBId VARCHAR(255),
	NCCINo VARCHAR(255),
	OtherNo VARCHAR(255),
	PolId VARCHAR(255),
	RatingDate datetime,
	RetroPlan VARCHAR(255),
	RetroYrs VARCHAR(255),
	PolicyState VARCHAR(255),
	PolicyStatus VARCHAR(255),
	WPolId VARCHAR(255),
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


IF OBJECT_ID('Staging.AMS_130Policy') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_130Policy
END
GO

CREATE TABLE Staging.AMS_130Policy(

	PolicyChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AddInfo VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	DividendPlan VARCHAR(255),
	EffDate DATETIME,
	EmployerNo VARCHAR(255),
	EnteredDate DATETIME,
	IncludeExclude VARCHAR(255),
	IsPart1 VARCHAR(255),
	IsParticipating VARCHAR(255),
	IsSafetyGroup VARCHAR(255),
	LOBId VARCHAR(255),
	NCCINo VARCHAR(255),
	OtherNo VARCHAR(255),
	PolId VARCHAR(255),
	RatingDate datetime,
	RetroPlan VARCHAR(255),
	RetroYrs VARCHAR(255),
	PolicyState VARCHAR(255),
	PolicyStatus VARCHAR(255),
	WPolId VARCHAR(255),
	StagingDate DATETIME, 
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO



CREATE CLUSTERED INDEX cix_130Policy_WPolId ON Staging.AMS_130Policy(WPolId)
CREATE CLUSTERED INDEX cix_130Policy_WPolId ON RawData.AMS_130Policy(WPolId)

GO

IF OBJECT_ID('Staging.AMS_130Policy_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_130Policy_Load
END
GO

CREATE PROCEDURE Staging.AMS_130Policy_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_130Policy
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_130Policy AS RawData
                         WHERE   RawData.WPolId = AMS_130Policy.WPolId
                   );

    INSERT INTO Staging.AMS_130Policy (
        PolicyChecksum
		, Datasource
		, Deleted
		, Sequencenumber
		, AddInfo
		, ChangedBy
		, ChangedDate
		, DividendPlan
		, EffDate
		, EmployerNo
		, EnteredDate
		, IncludeExclude
		, IsPart1
		, IsParticipating
		, IsSafetyGroup
		, LOBId
		, NCCINo
		, OtherNo
		, PolId
		, RatingDate
		, RetroPlan
		, RetroYrs
		, PolicyState
		, PolicyStatus
		, WPolId
        , StagingDate
        , InsertDate
    )
    SELECT AMS_130Policy.PolicyChecksum
		, AMS_130Policy.Datasource
		, AMS_130Policy.Deleted
		, AMS_130Policy.Sequencenumber
		, AMS_130Policy.AddInfo
		, AMS_130Policy.ChangedBy
		, AMS_130Policy.ChangedDate
		, AMS_130Policy.DividendPlan
		, AMS_130Policy.EffDate
		, AMS_130Policy.EmployerNo
		, AMS_130Policy.EnteredDate
		, AMS_130Policy.IncludeExclude
		, AMS_130Policy.IsPart1
		, AMS_130Policy.IsParticipating
		, AMS_130Policy.IsSafetyGroup
		, AMS_130Policy.LOBId
		, AMS_130Policy.NCCINo
		, AMS_130Policy.OtherNo
		, AMS_130Policy.PolId
		, AMS_130Policy.RatingDate
		, AMS_130Policy.RetroPlan
		, AMS_130Policy.RetroYrs
		, AMS_130Policy.PolicyState
		, AMS_130Policy.PolicyStatus
		, AMS_130Policy.WPolId
        , AMS_130Policy.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.AMS_130Policy;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_130Policy.WPolId
                                                  ORDER BY AMS_130Policy.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_130Policy
            ) AS AMS_130Policy
     WHERE  AMS_130Policy.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

-- GRANT CONTROL ON Staging.AMS_130Policy TO [abeerconsulting];
-- GRANT CONTROL ON RawData.AMS_130Policy TO [abeerconsulting];

GO