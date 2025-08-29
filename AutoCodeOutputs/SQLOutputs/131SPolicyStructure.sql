
IF OBJECT_ID('RawData.AMS_131SPolicy') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_131SPolicy
END
GO

CREATE TABLE RawData.AMS_131SPolicy(

	SPolicyChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CurRetroDate VARCHAR(255),
	EffDate DATETIME,
	EnteredDate DATETIME,
	ExpirPolNo VARCHAR(255),
	IsDollarYes VARCHAR(255),
	IsUmbrella VARCHAR(255),
	LOBId VARCHAR(255),
	PolId VARCHAR(255),
	PropRetroDate VARCHAR(255),
	SPolicyStatus VARCHAR(255),
	UPolId VARCHAR(255),
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


IF OBJECT_ID('Staging.AMS_131SPolicy') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_131SPolicy
END
GO

CREATE TABLE Staging.AMS_131SPolicy(

	SPolicyChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CurRetroDate VARCHAR(255),
	EffDate DATETIME,
	EnteredDate DATETIME,
	ExpirPolNo VARCHAR(255),
	IsDollarYes VARCHAR(255),
	IsUmbrella VARCHAR(255),
	LOBId VARCHAR(255),
	PolId VARCHAR(255),
	PropRetroDate VARCHAR(255),
	SPolicyStatus VARCHAR(255),
	UPolId VARCHAR(255),
	StagingDate DATETIME, 
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO



CREATE CLUSTERED INDEX cix_131SPolicy_Company_131SPolicyId ON Staging.AMS_131SPolicy(Company, 131SPolicyId)
CREATE CLUSTERED INDEX cix_131SPolicy_Company_131SPolicyId ON RawData.AMS_131SPolicy(Company, 131SPolicyId)

GO

IF OBJECT_ID('Staging.AMS_131SPolicy_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_131SPolicy_Load
END
GO

CREATE PROCEDURE Staging.AMS_131SPolicy_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_131SPolicy
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_131SPolicy AS RawData
                         WHERE   RawData.131SPolicyId = AMS_131SPolicy.131SPolicyId
                   );

    INSERT INTO Staging.AMS_131SPolicy (
        SPolicyChecksum
		, Datasource
		, Deleted
		, Sequencenumber
		, ChangedBy
		, ChangedDate
		, CurRetroDate
		, EffDate
		, EnteredDate
		, ExpirPolNo
		, IsDollarYes
		, IsUmbrella
		, LOBId
		, PolId
		, PropRetroDate
		, SPolicyStatus
		, UPolId
        , StagingDate
        , InsertDate
    )
    SELECT AMS_131SPolicy.SPolicyChecksum
		, AMS_131SPolicy.Datasource
		, AMS_131SPolicy.Deleted
		, AMS_131SPolicy.Sequencenumber
		, AMS_131SPolicy.ChangedBy
		, AMS_131SPolicy.ChangedDate
		, AMS_131SPolicy.CurRetroDate
		, AMS_131SPolicy.EffDate
		, AMS_131SPolicy.EnteredDate
		, AMS_131SPolicy.ExpirPolNo
		, AMS_131SPolicy.IsDollarYes
		, AMS_131SPolicy.IsUmbrella
		, AMS_131SPolicy.LOBId
		, AMS_131SPolicy.PolId
		, AMS_131SPolicy.PropRetroDate
		, AMS_131SPolicy.SPolicyStatus
		, AMS_131SPolicy.UPolId
        , AMS_131SPolicy.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.AMS_131SPolicy;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_131SPolicy.131SPolicyId
                                                  ORDER BY AMS_131SPolicy.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_131SPolicy
            ) AS AMS_131SPolicy
     WHERE  AMS_131SPolicy.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.AMS_131SPolicy TO [abeerconsulting];
GRANT CONTROL ON RawData.AMS_131SPolicy TO [abeerconsulting];
GO

GO

