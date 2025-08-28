IF OBJECT_ID ('RawData.AMS_Customer') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_Customer;
END;
GO

CREATE TABLE RawData.AMS_Customer
(
    CustomerChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , AcquisitionCode VARCHAR(255)
    , Active VARCHAR(255)
    , Addr1 VARCHAR(255)
    , Addr2 VARCHAR(255)
    , ANotId VARCHAR(255)
    , ARClosedDate DATETIME
    , ARClosedStatus VARCHAR(255)
    , AutoApplyCR VARCHAR(255)
    , AutoApplyPay VARCHAR(255)
    , BillAddr1 VARCHAR(255)
    , BillAddr2 VARCHAR(255)
    , BillCity VARCHAR(255)
    , BillCounty VARCHAR(255)
    , BillName VARCHAR(255)
    , BillState VARCHAR(255)
    , BillZip VARCHAR(255)
    , BrokerCode VARCHAR(255)
    , BusAreaCode VARCHAR(255)
    , BusExt VARCHAR(255)
    , BusFullPhone VARCHAR(255)
    , BusOriginCode VARCHAR(255)
    , BusPhone VARCHAR(255)
    , BusSince VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , City VARCHAR(255)
    , CollectLetter VARCHAR(255)
    , ContactMethod VARCHAR(255)
    , Country VARCHAR(255)
    , County VARCHAR(255)
    , CsrCode VARCHAR(255)
    , CustAddedDate DATETIME
    , CustId VARCHAR(255)
    , CustNo INT
    , DBA VARCHAR(255)
    , DOB DATETIME
    , DriversLicense VARCHAR(255)
    , DUNSNo VARCHAR(255)
    , ElectronicDelivery VARCHAR(255)
    , EMail VARCHAR(255)
    , EMail2 VARCHAR(255)
    , EnteredDate DATETIME
    , FaxAreaCode VARCHAR(255)
    , FaxExt VARCHAR(255)
    , FaxFullPhone VARCHAR(255)
    , FaxPhone VARCHAR(255)
    , FedIdNo VARCHAR(255)
    , FirmNameCust VARCHAR(255)
    , FirstName VARCHAR(255)
    , FormalSalutation VARCHAR(255)
    , GLBrnchCode VARCHAR(255)
    , GLCode VARCHAR(255)
    , GLDeptCode VARCHAR(255)
    , GLDivCode VARCHAR(255)
    , GLGrpCode VARCHAR(255)
    , GroupingOption VARCHAR(255)
    , InformalSalutation VARCHAR(255)
    , IsBillAddrSameAsCust VARCHAR(255)
    , IsBillNameSameAsCust VARCHAR(255)
    , IsBrokCust VARCHAR(255)
    , IsDeriveAttrFlagsCust VARCHAR(255)
    , IsExclDelete VARCHAR(255)
    , IsForeign BIT
    , IsPrintAgencyBill VARCHAR(255)
    , IsPrintDirectBill VARCHAR(255)
    , IsSecured VARCHAR(255)
    , JoinCriteria VARCHAR(255)
    , KnownSince VARCHAR(255)
    , LastName VARCHAR(255)
    , LateCharge VARCHAR(255)
    , LatitudeCust DECIMAL(18, 4)
    , LLId VARCHAR(255)
    , LongitudeCust DECIMAL(18, 4)
    , MarineAreaCode VARCHAR(255)
    , MarineExt VARCHAR(255)
    , MarineFullPhone VARCHAR(255)
    , MarinePhone VARCHAR(255)
    , MarketingSolicitation VARCHAR(255)
    , Married VARCHAR(255)
    , MasterCustId VARCHAR(255)
    , MasterSubTrack VARCHAR(255)
    , MasterSubType VARCHAR(255)
    , MethodOfDistribution VARCHAR(255)
    , MidName VARCHAR(255)
    , MktgFlag VARCHAR(255)
    , NAICS VARCHAR(255)
    , NonPC VARCHAR(255)
    , Notes VARCHAR(255)
    , Occupation VARCHAR(255)
    , OtherAreaCode VARCHAR(255)
    , OtherExt VARCHAR(255)
    , OtherFullPhone VARCHAR(255)
    , OtherPhone VARCHAR(255)
    , PagerAreaCode VARCHAR(255)
    , PagerExt VARCHAR(255)
    , PagerFullPhone VARCHAR(255)
    , PagerPhone VARCHAR(255)
    , PermAttrFlagsCust VARCHAR(255)
    , PremOption VARCHAR(255)
    , PrintCustStmt VARCHAR(255)
    , Prod1Code VARCHAR(255)
    , ReferralLocationCode VARCHAR(255)
    , ReferralNameCode VARCHAR(255)
    , ReportOption VARCHAR(255)
    , ResAreaCode VARCHAR(255)
    , ResExt VARCHAR(255)
    , ResFullPhone VARCHAR(255)
    , ResPhone VARCHAR(255)
    , Sex VARCHAR(255)
    , SIC VARCHAR(255)
    , SortName VARCHAR(255)
    , SSN VARCHAR(255)
    , CustomerState VARCHAR(255)
    , StatePrintGroup VARCHAR(255)
    , TypeCust VARCHAR(255)
    , TypeEntity VARCHAR(255)
    , TypeName VARCHAR(255)
    , TypeOfBusiness VARCHAR(255)
    , UserAttrFlagsCust VARCHAR(255)
    , WebAddr VARCHAR(255)
    , YearEmployed VARCHAR(255)
    , ZipCode VARCHAR(255)
    , EducationLevel VARCHAR(255)
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

IF OBJECT_ID ('Staging.AMS_Customer') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_Customer;
END;
GO

CREATE TABLE Staging.AMS_Customer
(
    CustomerChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , AcquisitionCode VARCHAR(255)
    , Active VARCHAR(255)
    , Addr1 VARCHAR(255)
    , Addr2 VARCHAR(255)
    , ANotId VARCHAR(255)
    , ARClosedDate DATETIME
    , ARClosedStatus VARCHAR(255)
    , AutoApplyCR VARCHAR(255)
    , AutoApplyPay VARCHAR(255)
    , BillAddr1 VARCHAR(255)
    , BillAddr2 VARCHAR(255)
    , BillCity VARCHAR(255)
    , BillCounty VARCHAR(255)
    , BillName VARCHAR(255)
    , BillState VARCHAR(255)
    , BillZip VARCHAR(255)
    , BrokerCode VARCHAR(255)
    , BusAreaCode VARCHAR(255)
    , BusExt VARCHAR(255)
    , BusFullPhone VARCHAR(255)
    , BusOriginCode VARCHAR(255)
    , BusPhone VARCHAR(255)
    , BusSince VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , City VARCHAR(255)
    , CollectLetter VARCHAR(255)
    , ContactMethod VARCHAR(255)
    , Country VARCHAR(255)
    , County VARCHAR(255)
    , CsrCode VARCHAR(255)
    , CustAddedDate DATETIME
    , CustId VARCHAR(255)
    , CustNo INT
    , DBA VARCHAR(255)
    , DOB DATETIME
    , DriversLicense VARCHAR(255)
    , DUNSNo VARCHAR(255)
    , ElectronicDelivery VARCHAR(255)
    , EMail VARCHAR(255)
    , EMail2 VARCHAR(255)
    , EnteredDate DATETIME
    , FaxAreaCode VARCHAR(255)
    , FaxExt VARCHAR(255)
    , FaxFullPhone VARCHAR(255)
    , FaxPhone VARCHAR(255)
    , FedIdNo VARCHAR(255)
    , FirmNameCust VARCHAR(255)
    , FirstName VARCHAR(255)
    , FormalSalutation VARCHAR(255)
    , GLBrnchCode VARCHAR(255)
    , GLCode VARCHAR(255)
    , GLDeptCode VARCHAR(255)
    , GLDivCode VARCHAR(255)
    , GLGrpCode VARCHAR(255)
    , GroupingOption VARCHAR(255)
    , InformalSalutation VARCHAR(255)
    , IsBillAddrSameAsCust VARCHAR(255)
    , IsBillNameSameAsCust VARCHAR(255)
    , IsBrokCust VARCHAR(255)
    , IsDeriveAttrFlagsCust VARCHAR(255)
    , IsExclDelete VARCHAR(255)
    , IsForeign BIT
    , IsPrintAgencyBill VARCHAR(255)
    , IsPrintDirectBill VARCHAR(255)
    , IsSecured VARCHAR(255)
    , JoinCriteria VARCHAR(255)
    , KnownSince VARCHAR(255)
    , LastName VARCHAR(255)
    , LateCharge VARCHAR(255)
    , LatitudeCust DECIMAL(18, 4)
    , LLId VARCHAR(255)
    , LongitudeCust DECIMAL(18, 4)
    , MarineAreaCode VARCHAR(255)
    , MarineExt VARCHAR(255)
    , MarineFullPhone VARCHAR(255)
    , MarinePhone VARCHAR(255)
    , MarketingSolicitation VARCHAR(255)
    , Married VARCHAR(255)
    , MasterCustId VARCHAR(255)
    , MasterSubTrack VARCHAR(255)
    , MasterSubType VARCHAR(255)
    , MethodOfDistribution VARCHAR(255)
    , MidName VARCHAR(255)
    , MktgFlag VARCHAR(255)
    , NAICS VARCHAR(255)
    , NonPC VARCHAR(255)
    , Notes VARCHAR(255)
    , Occupation VARCHAR(255)
    , OtherAreaCode VARCHAR(255)
    , OtherExt VARCHAR(255)
    , OtherFullPhone VARCHAR(255)
    , OtherPhone VARCHAR(255)
    , PagerAreaCode VARCHAR(255)
    , PagerExt VARCHAR(255)
    , PagerFullPhone VARCHAR(255)
    , PagerPhone VARCHAR(255)
    , PermAttrFlagsCust VARCHAR(255)
    , PremOption VARCHAR(255)
    , PrintCustStmt VARCHAR(255)
    , Prod1Code VARCHAR(255)
    , ReferralLocationCode VARCHAR(255)
    , ReferralNameCode VARCHAR(255)
    , ReportOption VARCHAR(255)
    , ResAreaCode VARCHAR(255)
    , ResExt VARCHAR(255)
    , ResFullPhone VARCHAR(255)
    , ResPhone VARCHAR(255)
    , Sex VARCHAR(255)
    , SIC VARCHAR(255)
    , SortName VARCHAR(255)
    , SSN VARCHAR(255)
    , CustomerState VARCHAR(255)
    , StatePrintGroup VARCHAR(255)
    , TypeCust VARCHAR(255)
    , TypeEntity VARCHAR(255)
    , TypeName VARCHAR(255)
    , TypeOfBusiness VARCHAR(255)
    , UserAttrFlagsCust VARCHAR(255)
    , WebAddr VARCHAR(255)
    , YearEmployed VARCHAR(255)
    , ZipCode VARCHAR(255)
    , EducationLevel VARCHAR(255)
    , StagingDate DATETIME
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

CREATE CLUSTERED INDEX cix_Customer_CustId
    ON Staging.AMS_Customer (CustId);

CREATE CLUSTERED INDEX cix_Customer_CustId
    ON RawData.AMS_Customer (CustId);
GO

IF OBJECT_ID ('Staging.AMS_Customer_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_Customer_Load;
END;
GO

CREATE PROCEDURE Staging.AMS_Customer_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_Customer
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_Customer AS RawData
                        WHERE   RawData.CustId = AMS_Customer.CustId
                   );

    INSERT INTO Staging.AMS_Customer (CustomerChecksum
                                      , Datasource
                                      , Deleted
                                      , Sequencenumber
                                      , AcquisitionCode
                                      , Active
                                      , Addr1
                                      , Addr2
                                      , ANotId
                                      , ARClosedDate
                                      , ARClosedStatus
                                      , AutoApplyCR
                                      , AutoApplyPay
                                      , BillAddr1
                                      , BillAddr2
                                      , BillCity
                                      , BillCounty
                                      , BillName
                                      , BillState
                                      , BillZip
                                      , BrokerCode
                                      , BusAreaCode
                                      , BusExt
                                      , BusFullPhone
                                      , BusOriginCode
                                      , BusPhone
                                      , BusSince
                                      , ChangedBy
                                      , ChangedDate
                                      , City
                                      , CollectLetter
                                      , ContactMethod
                                      , Country
                                      , County
                                      , CsrCode
                                      , CustAddedDate
                                      , CustId
                                      , CustNo
                                      , DBA
                                      , DOB
                                      , DriversLicense
                                      , DUNSNo
                                      , ElectronicDelivery
                                      , EMail
                                      , EMail2
                                      , EnteredDate
                                      , FaxAreaCode
                                      , FaxExt
                                      , FaxFullPhone
                                      , FaxPhone
                                      , FedIdNo
                                      , FirmNameCust
                                      , FirstName
                                      , FormalSalutation
                                      , GLBrnchCode
                                      , GLCode
                                      , GLDeptCode
                                      , GLDivCode
                                      , GLGrpCode
                                      , GroupingOption
                                      , InformalSalutation
                                      , IsBillAddrSameAsCust
                                      , IsBillNameSameAsCust
                                      , IsBrokCust
                                      , IsDeriveAttrFlagsCust
                                      , IsExclDelete
                                      , IsForeign
                                      , IsPrintAgencyBill
                                      , IsPrintDirectBill
                                      , IsSecured
                                      , JoinCriteria
                                      , KnownSince
                                      , LastName
                                      , LateCharge
                                      , LatitudeCust
                                      , LLId
                                      , LongitudeCust
                                      , MarineAreaCode
                                      , MarineExt
                                      , MarineFullPhone
                                      , MarinePhone
                                      , MarketingSolicitation
                                      , Married
                                      , MasterCustId
                                      , MasterSubTrack
                                      , MasterSubType
                                      , MethodOfDistribution
                                      , MidName
                                      , MktgFlag
                                      , NAICS
                                      , NonPC
                                      , Notes
                                      , Occupation
                                      , OtherAreaCode
                                      , OtherExt
                                      , OtherFullPhone
                                      , OtherPhone
                                      , PagerAreaCode
                                      , PagerExt
                                      , PagerFullPhone
                                      , PagerPhone
                                      , PermAttrFlagsCust
                                      , PremOption
                                      , PrintCustStmt
                                      , Prod1Code
                                      , ReferralLocationCode
                                      , ReferralNameCode
                                      , ReportOption
                                      , ResAreaCode
                                      , ResExt
                                      , ResFullPhone
                                      , ResPhone
                                      , Sex
                                      , SIC
                                      , SortName
                                      , SSN
                                      , CustomerState
                                      , StatePrintGroup
                                      , TypeCust
                                      , TypeEntity
                                      , TypeName
                                      , TypeOfBusiness
                                      , UserAttrFlagsCust
                                      , WebAddr
                                      , YearEmployed
                                      , ZipCode
                                      , EducationLevel
                                      , StagingDate
                                      , InsertDate)
    SELECT  AMS_Customer.CustomerChecksum
            , AMS_Customer.Datasource
            , AMS_Customer.Deleted
            , AMS_Customer.Sequencenumber
            , AMS_Customer.AcquisitionCode
            , AMS_Customer.Active
            , AMS_Customer.Addr1
            , AMS_Customer.Addr2
            , AMS_Customer.ANotId
            , AMS_Customer.ARClosedDate
            , AMS_Customer.ARClosedStatus
            , AMS_Customer.AutoApplyCR
            , AMS_Customer.AutoApplyPay
            , AMS_Customer.BillAddr1
            , AMS_Customer.BillAddr2
            , AMS_Customer.BillCity
            , AMS_Customer.BillCounty
            , AMS_Customer.BillName
            , AMS_Customer.BillState
            , AMS_Customer.BillZip
            , AMS_Customer.BrokerCode
            , AMS_Customer.BusAreaCode
            , AMS_Customer.BusExt
            , AMS_Customer.BusFullPhone
            , AMS_Customer.BusOriginCode
            , AMS_Customer.BusPhone
            , AMS_Customer.BusSince
            , AMS_Customer.ChangedBy
            , AMS_Customer.ChangedDate
            , AMS_Customer.City
            , AMS_Customer.CollectLetter
            , AMS_Customer.ContactMethod
            , AMS_Customer.Country
            , AMS_Customer.County
            , AMS_Customer.CsrCode
            , AMS_Customer.CustAddedDate
            , AMS_Customer.CustId
            , AMS_Customer.CustNo
            , AMS_Customer.DBA
            , AMS_Customer.DOB
            , AMS_Customer.DriversLicense
            , AMS_Customer.DUNSNo
            , AMS_Customer.ElectronicDelivery
            , AMS_Customer.EMail
            , AMS_Customer.EMail2
            , AMS_Customer.EnteredDate
            , AMS_Customer.FaxAreaCode
            , AMS_Customer.FaxExt
            , AMS_Customer.FaxFullPhone
            , AMS_Customer.FaxPhone
            , AMS_Customer.FedIdNo
            , AMS_Customer.FirmNameCust
            , AMS_Customer.FirstName
            , AMS_Customer.FormalSalutation
            , AMS_Customer.GLBrnchCode
            , AMS_Customer.GLCode
            , AMS_Customer.GLDeptCode
            , AMS_Customer.GLDivCode
            , AMS_Customer.GLGrpCode
            , AMS_Customer.GroupingOption
            , AMS_Customer.InformalSalutation
            , AMS_Customer.IsBillAddrSameAsCust
            , AMS_Customer.IsBillNameSameAsCust
            , AMS_Customer.IsBrokCust
            , AMS_Customer.IsDeriveAttrFlagsCust
            , AMS_Customer.IsExclDelete
            , AMS_Customer.IsForeign
            , AMS_Customer.IsPrintAgencyBill
            , AMS_Customer.IsPrintDirectBill
            , AMS_Customer.IsSecured
            , AMS_Customer.JoinCriteria
            , AMS_Customer.KnownSince
            , AMS_Customer.LastName
            , AMS_Customer.LateCharge
            , AMS_Customer.LatitudeCust
            , AMS_Customer.LLId
            , AMS_Customer.LongitudeCust
            , AMS_Customer.MarineAreaCode
            , AMS_Customer.MarineExt
            , AMS_Customer.MarineFullPhone
            , AMS_Customer.MarinePhone
            , AMS_Customer.MarketingSolicitation
            , AMS_Customer.Married
            , AMS_Customer.MasterCustId
            , AMS_Customer.MasterSubTrack
            , AMS_Customer.MasterSubType
            , AMS_Customer.MethodOfDistribution
            , AMS_Customer.MidName
            , AMS_Customer.MktgFlag
            , AMS_Customer.NAICS
            , AMS_Customer.NonPC
            , AMS_Customer.Notes
            , AMS_Customer.Occupation
            , AMS_Customer.OtherAreaCode
            , AMS_Customer.OtherExt
            , AMS_Customer.OtherFullPhone
            , AMS_Customer.OtherPhone
            , AMS_Customer.PagerAreaCode
            , AMS_Customer.PagerExt
            , AMS_Customer.PagerFullPhone
            , AMS_Customer.PagerPhone
            , AMS_Customer.PermAttrFlagsCust
            , AMS_Customer.PremOption
            , AMS_Customer.PrintCustStmt
            , AMS_Customer.Prod1Code
            , AMS_Customer.ReferralLocationCode
            , AMS_Customer.ReferralNameCode
            , AMS_Customer.ReportOption
            , AMS_Customer.ResAreaCode
            , AMS_Customer.ResExt
            , AMS_Customer.ResFullPhone
            , AMS_Customer.ResPhone
            , AMS_Customer.Sex
            , AMS_Customer.SIC
            , AMS_Customer.SortName
            , AMS_Customer.SSN
            , AMS_Customer.CustomerState
            , AMS_Customer.StatePrintGroup
            , AMS_Customer.TypeCust
            , AMS_Customer.TypeEntity
            , AMS_Customer.TypeName
            , AMS_Customer.TypeOfBusiness
            , AMS_Customer.UserAttrFlagsCust
            , AMS_Customer.WebAddr
            , AMS_Customer.YearEmployed
            , AMS_Customer.ZipCode
            , AMS_Customer.EducationLevel
            , AMS_Customer.InsertDate
            , GETDATE () AS InsertDate
      --FROM  RawData.AMS_Customer;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_Customer.CustId
                                                  ORDER BY AMS_Customer.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_Customer
            ) AS AMS_Customer
     WHERE  AMS_Customer.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.AMS_Customer TO [abeerconsulting];

GRANT CONTROL ON RawData.AMS_Customer TO [abeerconsulting];
GO

GO