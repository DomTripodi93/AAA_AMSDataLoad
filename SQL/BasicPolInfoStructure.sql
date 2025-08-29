
IF OBJECT_ID('RawData.AMS_BasicPolInfo') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_BasicPolInfo
END
GO

CREATE TABLE RawData.AMS_BasicPolInfo(

	BasicPolInfoChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AgcyBusClass VARCHAR(255),
	ANotId VARCHAR(255),
	AuditFlag VARCHAR(255),
	AuditPeriod VARCHAR(255),
	BillAcctNo VARCHAR(255),
	BilledStmtPrem money,
	BillMethod VARCHAR(255),
	BrokerCode VARCHAR(255),
	BusOriginCode VARCHAR(255),
	CarrierStatus VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CoCode VARCHAR(255),
	CompCustNo VARCHAR(255),
	CoType VARCHAR(255),
	CsrCode VARCHAR(255),
	CustId VARCHAR(255),
	DescriptionBPol VARCHAR(255),
	EnteredDate DATETIME,
	ExcludeFrmDownload VARCHAR(255),
	ExecCode VARCHAR(255),
	FirstWrittenDate DATETIME,
	FlatAmount money,
	FullTermPremium money,
	GLBrnchCode VARCHAR(255),
	GLDeptCode VARCHAR(255),
	GLDivCode VARCHAR(255),
	GLGrpCode VARCHAR(255),
	InstDay smallint,
	IsContinuous VARCHAR(255),
	IsExclDelete VARCHAR(255),
	IsFiltered VARCHAR(255),
	IsFinanced VARCHAR(255),
	IsMultiEntity VARCHAR(255),
	IsNewBPol VARCHAR(255),
	IsPosted VARCHAR(255),
	IsProdCredRequire100 VARCHAR(255),
	IsProductionCreditEnabled VARCHAR(255),
	IsReinsuranceEnabled VARCHAR(255),
	IssuedState VARCHAR(255),
	IsSuspended VARCHAR(255),
	Istid VARCHAR(255),
	MasterAgent VARCHAR(255),
	Method VARCHAR(255),
	MethodOfPayments VARCHAR(255),
	MultiEntityARFlag VARCHAR(255),
	NatlProdCode bigint,
	NegCommValidDate datetime,
	NotRenewable VARCHAR(255),
	NumOfPayments smallint,
	PayPId VARCHAR(255),
	Percentage DECIMAL(18,4),
	PolChangedDate datetime,
	PolEffDate DATETIME,
	PolExpDate DATETIME,
	PolId VARCHAR(255),
	PolNo VARCHAR(255),
	PolSubType VARCHAR(255),
	PolType VARCHAR(255),
	PolTypeLOB VARCHAR(255),
	PremAdj VARCHAR(255),
	PriorPolicy VARCHAR(255),
	Priorpolid VARCHAR(255),
	RenewalList VARCHAR(255),
	RenewalRptFlag VARCHAR(255),
	ShortPolNo VARCHAR(255),
	SourcePolId VARCHAR(255),
	BasicPolInfoStatus VARCHAR(255),
	SubAgent VARCHAR(255),
	TiComId VARCHAR(255),
	TypeOfBus smallint,
	Underwriter VARCHAR(255),
	WritingCoCode VARCHAR(255),
	RenewalTermCount smallint,
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO


IF OBJECT_ID('Staging.AMS_BasicPolInfo') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_BasicPolInfo
END
GO

CREATE TABLE Staging.AMS_BasicPolInfo(

	BasicPolInfoChecksum VARCHAR(255),
	Datasource VARCHAR(255),
	Deleted bit,
	Sequencenumber DECIMAL(18,4),
	AgcyBusClass VARCHAR(255),
	ANotId VARCHAR(255),
	AuditFlag VARCHAR(255),
	AuditPeriod VARCHAR(255),
	BillAcctNo VARCHAR(255),
	BilledStmtPrem money,
	BillMethod VARCHAR(255),
	BrokerCode VARCHAR(255),
	BusOriginCode VARCHAR(255),
	CarrierStatus VARCHAR(255),
	ChangedBy VARCHAR(255),
	ChangedDate DATETIME,
	CoCode VARCHAR(255),
	CompCustNo VARCHAR(255),
	CoType VARCHAR(255),
	CsrCode VARCHAR(255),
	CustId VARCHAR(255),
	DescriptionBPol VARCHAR(255),
	EnteredDate DATETIME,
	ExcludeFrmDownload VARCHAR(255),
	ExecCode VARCHAR(255),
	FirstWrittenDate DATETIME,
	FlatAmount money,
	FullTermPremium money,
	GLBrnchCode VARCHAR(255),
	GLDeptCode VARCHAR(255),
	GLDivCode VARCHAR(255),
	GLGrpCode VARCHAR(255),
	InstDay smallint,
	IsContinuous VARCHAR(255),
	IsExclDelete VARCHAR(255),
	IsFiltered VARCHAR(255),
	IsFinanced VARCHAR(255),
	IsMultiEntity VARCHAR(255),
	IsNewBPol VARCHAR(255),
	IsPosted VARCHAR(255),
	IsProdCredRequire100 VARCHAR(255),
	IsProductionCreditEnabled VARCHAR(255),
	IsReinsuranceEnabled VARCHAR(255),
	IssuedState VARCHAR(255),
	IsSuspended VARCHAR(255),
	Istid VARCHAR(255),
	MasterAgent VARCHAR(255),
	Method VARCHAR(255),
	MethodOfPayments VARCHAR(255),
	MultiEntityARFlag VARCHAR(255),
	NatlProdCode bigint,
	NegCommValidDate datetime,
	NotRenewable VARCHAR(255),
	NumOfPayments smallint,
	PayPId VARCHAR(255),
	Percentage DECIMAL(18,4),
	PolChangedDate datetime,
	PolEffDate DATETIME,
	PolExpDate DATETIME,
	PolId VARCHAR(255),
	PolNo VARCHAR(255),
	PolSubType VARCHAR(255),
	PolType VARCHAR(255),
	PolTypeLOB VARCHAR(255),
	PremAdj VARCHAR(255),
	PriorPolicy VARCHAR(255),
	Priorpolid VARCHAR(255),
	RenewalList VARCHAR(255),
	RenewalRptFlag VARCHAR(255),
	ShortPolNo VARCHAR(255),
	SourcePolId VARCHAR(255),
	BasicPolInfoStatus VARCHAR(255),
	SubAgent VARCHAR(255),
	TiComId VARCHAR(255),
	TypeOfBus smallint,
	Underwriter VARCHAR(255),
	WritingCoCode VARCHAR(255),
	RenewalTermCount smallint,
	StagingDate DATETIME, 
	InsertDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO


GO



CREATE CLUSTERED INDEX cix_BasicPolInfo_PolId ON Staging.AMS_BasicPolInfo(PolId)
CREATE CLUSTERED INDEX cix_BasicPolInfo_PolId ON RawData.AMS_BasicPolInfo(PolId)

GO

IF OBJECT_ID('Staging.AMS_BasicPolInfo_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_BasicPolInfo_Load
END
GO

CREATE PROCEDURE Staging.AMS_BasicPolInfo_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_BasicPolInfo
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_BasicPolInfo AS RawData
                         WHERE   RawData.PolId = AMS_BasicPolInfo.PolId
                   );

    INSERT INTO Staging.AMS_BasicPolInfo (
        BasicPolInfoChecksum
		, Datasource
		, Deleted
		, Sequencenumber
		, AgcyBusClass
		, ANotId
		, AuditFlag
		, AuditPeriod
		, BillAcctNo
		, BilledStmtPrem
		, BillMethod
		, BrokerCode
		, BusOriginCode
		, CarrierStatus
		, ChangedBy
		, ChangedDate
		, CoCode
		, CompCustNo
		, CoType
		, CsrCode
		, CustId
		, DescriptionBPol
		, EnteredDate
		, ExcludeFrmDownload
		, ExecCode
		, FirstWrittenDate
		, FlatAmount
		, FullTermPremium
		, GLBrnchCode
		, GLDeptCode
		, GLDivCode
		, GLGrpCode
		, InstDay
		, IsContinuous
		, IsExclDelete
		, IsFiltered
		, IsFinanced
		, IsMultiEntity
		, IsNewBPol
		, IsPosted
		, IsProdCredRequire100
		, IsProductionCreditEnabled
		, IsReinsuranceEnabled
		, IssuedState
		, IsSuspended
		, Istid
		, MasterAgent
		, Method
		, MethodOfPayments
		, MultiEntityARFlag
		, NatlProdCode
		, NegCommValidDate
		, NotRenewable
		, NumOfPayments
		, PayPId
		, Percentage
		, PolChangedDate
		, PolEffDate
		, PolExpDate
		, PolId
		, PolNo
		, PolSubType
		, PolType
		, PolTypeLOB
		, PremAdj
		, PriorPolicy
		, Priorpolid
		, RenewalList
		, RenewalRptFlag
		, ShortPolNo
		, SourcePolId
		, BasicPolInfoStatus
		, SubAgent
		, TiComId
		, TypeOfBus
		, Underwriter
		, WritingCoCode
		, RenewalTermCount
        , StagingDate
        , InsertDate
    )
    SELECT AMS_BasicPolInfo.BasicPolInfoChecksum
		, AMS_BasicPolInfo.Datasource
		, AMS_BasicPolInfo.Deleted
		, AMS_BasicPolInfo.Sequencenumber
		, AMS_BasicPolInfo.AgcyBusClass
		, AMS_BasicPolInfo.ANotId
		, AMS_BasicPolInfo.AuditFlag
		, AMS_BasicPolInfo.AuditPeriod
		, AMS_BasicPolInfo.BillAcctNo
		, AMS_BasicPolInfo.BilledStmtPrem
		, AMS_BasicPolInfo.BillMethod
		, AMS_BasicPolInfo.BrokerCode
		, AMS_BasicPolInfo.BusOriginCode
		, AMS_BasicPolInfo.CarrierStatus
		, AMS_BasicPolInfo.ChangedBy
		, AMS_BasicPolInfo.ChangedDate
		, AMS_BasicPolInfo.CoCode
		, AMS_BasicPolInfo.CompCustNo
		, AMS_BasicPolInfo.CoType
		, AMS_BasicPolInfo.CsrCode
		, AMS_BasicPolInfo.CustId
		, AMS_BasicPolInfo.DescriptionBPol
		, AMS_BasicPolInfo.EnteredDate
		, AMS_BasicPolInfo.ExcludeFrmDownload
		, AMS_BasicPolInfo.ExecCode
		, AMS_BasicPolInfo.FirstWrittenDate
		, AMS_BasicPolInfo.FlatAmount
		, AMS_BasicPolInfo.FullTermPremium
		, AMS_BasicPolInfo.GLBrnchCode
		, AMS_BasicPolInfo.GLDeptCode
		, AMS_BasicPolInfo.GLDivCode
		, AMS_BasicPolInfo.GLGrpCode
		, AMS_BasicPolInfo.InstDay
		, AMS_BasicPolInfo.IsContinuous
		, AMS_BasicPolInfo.IsExclDelete
		, AMS_BasicPolInfo.IsFiltered
		, AMS_BasicPolInfo.IsFinanced
		, AMS_BasicPolInfo.IsMultiEntity
		, AMS_BasicPolInfo.IsNewBPol
		, AMS_BasicPolInfo.IsPosted
		, AMS_BasicPolInfo.IsProdCredRequire100
		, AMS_BasicPolInfo.IsProductionCreditEnabled
		, AMS_BasicPolInfo.IsReinsuranceEnabled
		, AMS_BasicPolInfo.IssuedState
		, AMS_BasicPolInfo.IsSuspended
		, AMS_BasicPolInfo.Istid
		, AMS_BasicPolInfo.MasterAgent
		, AMS_BasicPolInfo.Method
		, AMS_BasicPolInfo.MethodOfPayments
		, AMS_BasicPolInfo.MultiEntityARFlag
		, AMS_BasicPolInfo.NatlProdCode
		, AMS_BasicPolInfo.NegCommValidDate
		, AMS_BasicPolInfo.NotRenewable
		, AMS_BasicPolInfo.NumOfPayments
		, AMS_BasicPolInfo.PayPId
		, AMS_BasicPolInfo.Percentage
		, AMS_BasicPolInfo.PolChangedDate
		, AMS_BasicPolInfo.PolEffDate
		, AMS_BasicPolInfo.PolExpDate
		, AMS_BasicPolInfo.PolId
		, AMS_BasicPolInfo.PolNo
		, AMS_BasicPolInfo.PolSubType
		, AMS_BasicPolInfo.PolType
		, AMS_BasicPolInfo.PolTypeLOB
		, AMS_BasicPolInfo.PremAdj
		, AMS_BasicPolInfo.PriorPolicy
		, AMS_BasicPolInfo.Priorpolid
		, AMS_BasicPolInfo.RenewalList
		, AMS_BasicPolInfo.RenewalRptFlag
		, AMS_BasicPolInfo.ShortPolNo
		, AMS_BasicPolInfo.SourcePolId
		, AMS_BasicPolInfo.BasicPolInfoStatus
		, AMS_BasicPolInfo.SubAgent
		, AMS_BasicPolInfo.TiComId
		, AMS_BasicPolInfo.TypeOfBus
		, AMS_BasicPolInfo.Underwriter
		, AMS_BasicPolInfo.WritingCoCode
		, AMS_BasicPolInfo.RenewalTermCount
        , AMS_BasicPolInfo.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.AMS_BasicPolInfo;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_BasicPolInfo.PolId
                                                  ORDER BY AMS_BasicPolInfo.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_BasicPolInfo
            ) AS AMS_BasicPolInfo
     WHERE  AMS_BasicPolInfo.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

-- GRANT CONTROL ON Staging.AMS_BasicPolInfo TO [abeerconsulting];
-- GRANT CONTROL ON RawData.AMS_BasicPolInfo TO [abeerconsulting];
GO