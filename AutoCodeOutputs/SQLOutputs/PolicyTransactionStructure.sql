
IF OBJECT_ID('RawData.AMS_PolicyTransaction') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_PolicyTransaction
END
GO

CREATE TABLE RawData.AMS_PolicyTransaction(

	PolicyTransactionChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	BilledNonPrem money,
	BillMethodPolT VARCHAR(255),
	BinderReplacePolTEffDate datetime,
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	Description VARCHAR(255),
	EffDate DATETIME,
	EnteredDate DATETIME,
	EstRevenuePercent DECIMAL(18,4),
	InstDayPolT smallint,
	IsPosted VARCHAR(255),
	IsUploaded VARCHAR(255),
	PayPId VARCHAR(255),
	PolId VARCHAR(255),
	PremOnEffDate money,
	ReasonPolT VARCHAR(255),
	ReplaceDatePolT datetime,
	Source VARCHAR(255),
	TranType VARCHAR(255),
	Annualizedestrevenue VARCHAR(255),
	Annualizedpremium VARCHAR(255),
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


IF OBJECT_ID('Staging.AMS_PolicyTransaction') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_PolicyTransaction
END
GO

CREATE TABLE Staging.AMS_PolicyTransaction(

	PolicyTransactionChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	BilledNonPrem money,
	BillMethodPolT VARCHAR(255),
	BinderReplacePolTEffDate datetime,
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	Description VARCHAR(255),
	EffDate DATETIME,
	EnteredDate DATETIME,
	EstRevenuePercent DECIMAL(18,4),
	InstDayPolT smallint,
	IsPosted VARCHAR(255),
	IsUploaded VARCHAR(255),
	PayPId VARCHAR(255),
	PolId VARCHAR(255),
	PremOnEffDate money,
	ReasonPolT VARCHAR(255),
	ReplaceDatePolT datetime,
	Source VARCHAR(255),
	TranType VARCHAR(255),
	Annualizedestrevenue VARCHAR(255),
	Annualizedpremium VARCHAR(255),
	StagingDate DATETIME, 
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


CREATE CLUSTERED INDEX cix_PolicyTransaction_PolId_EffDate ON Staging.AMS_PolicyTransaction(PolId, EffDate)
CREATE CLUSTERED INDEX cix_PolicyTransaction_PolId_EffDate ON RawData.AMS_PolicyTransaction(PolId, EffDate)

GO

IF OBJECT_ID('Staging.AMS_PolicyTransaction_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_PolicyTransaction_Load
END
GO

CREATE PROCEDURE Staging.AMS_PolicyTransaction_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_PolicyTransaction
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_PolicyTransaction AS RawData
                         WHERE   RawData.PolId = AMS_PolicyTransaction.PolId
						 AND RawData.EffDate = AMS_PolicyTransaction.EffDate
                   );

    INSERT INTO Staging.AMS_PolicyTransaction (
        PolicyTransactionChecksum
		, Datasource
		, Deleted
		, Sequencenumber
		, BilledNonPrem
		, BillMethodPolT
		, BinderReplacePolTEffDate
		, ChangedBy
		, ChangedDate
		, Description
		, EffDate
		, EnteredDate
		, EstRevenuePercent
		, InstDayPolT
		, IsPosted
		, IsUploaded
		, PayPId
		, PolId
		, PremOnEffDate
		, ReasonPolT
		, ReplaceDatePolT
		, Source
		, TranType
		, Annualizedestrevenue
		, Annualizedpremium
        , StagingDate
        , InsertDate
    )
    SELECT AMS_PolicyTransaction.PolicyTransactionChecksum
		, AMS_PolicyTransaction.Datasource
		, AMS_PolicyTransaction.Deleted
		, AMS_PolicyTransaction.Sequencenumber
		, AMS_PolicyTransaction.BilledNonPrem
		, AMS_PolicyTransaction.BillMethodPolT
		, AMS_PolicyTransaction.BinderReplacePolTEffDate
		, AMS_PolicyTransaction.ChangedBy
		, AMS_PolicyTransaction.ChangedDate
		, AMS_PolicyTransaction.Description
		, AMS_PolicyTransaction.EffDate
		, AMS_PolicyTransaction.EnteredDate
		, AMS_PolicyTransaction.EstRevenuePercent
		, AMS_PolicyTransaction.InstDayPolT
		, AMS_PolicyTransaction.IsPosted
		, AMS_PolicyTransaction.IsUploaded
		, AMS_PolicyTransaction.PayPId
		, AMS_PolicyTransaction.PolId
		, AMS_PolicyTransaction.PremOnEffDate
		, AMS_PolicyTransaction.ReasonPolT
		, AMS_PolicyTransaction.ReplaceDatePolT
		, AMS_PolicyTransaction.Source
		, AMS_PolicyTransaction.TranType
		, AMS_PolicyTransaction.Annualizedestrevenue
		, AMS_PolicyTransaction.Annualizedpremium
        , AMS_PolicyTransaction.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.AMS_PolicyTransaction;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_PolicyTransaction.PolId, AMS_PolicyTransaction.EffDate
                                                  ORDER BY AMS_PolicyTransaction.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_PolicyTransaction
            ) AS AMS_PolicyTransaction
     WHERE  AMS_PolicyTransaction.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

-- GRANT CONTROL ON Staging.AMS_PolicyTransaction TO [abeerconsulting];
-- GRANT CONTROL ON RawData.AMS_PolicyTransaction TO [abeerconsulting];
GO

GO