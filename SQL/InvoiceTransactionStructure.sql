IF OBJECT_ID ('RawData.AMS_InvoiceTransaction') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_InvoiceTransaction;
END;
GO

CREATE TABLE RawData.AMS_InvoiceTransaction
(
    InvoiceTransactionChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , BackInvTPId VARCHAR(255)
    , BHId VARCHAR(255)
    , BillMethod VARCHAR(255)
    , BillSeqId VARCHAR(255)
    , BillTranId VARCHAR(255)
    , BinderPolTEffDate DATETIME
    , BinderPostMethod VARCHAR(255)
    , BinderStatus VARCHAR(255)
    , BrokerCode VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , ChargeCat VARCHAR(255)
    , Chargecode VARCHAR(255)
    , CoCode VARCHAR(255)
    , CommPayType VARCHAR(255)
    , CoType VARCHAR(255)
    , CSHId VARCHAR(255)
    , CustId VARCHAR(255)
    , Description VARCHAR(255)
    , EnteredDate DATETIME
    , FullTermPremAmt MONEY
    , GLDate DATETIME
    , GrossAmt MONEY
    , InvEffDate DATETIME
    , InvId VARCHAR(255)
    , InvTPId VARCHAR(255)
    , IsCancelled VARCHAR(255)
    , IsInstallment VARCHAR(255)
    , IsPosted VARCHAR(255)
    , JournalTranId VARCHAR(255)
    , LineOfBus VARCHAR(255)
    , NewInvTPId VARCHAR(255)
    , NonPrRecipient VARCHAR(255)
    , OldInvTPId VARCHAR(255)
    , PlanType VARCHAR(255)
    , PolTEffDate DATETIME
    , Poltpid VARCHAR(255)
    , RBBkOutId VARCHAR(255)
    , ReplaceDate DATETIME
    , TranType VARCHAR(255)
    , WritingCoCode VARCHAR(255)
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

IF OBJECT_ID ('Staging.AMS_InvoiceTransaction') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_InvoiceTransaction;
END;
GO

CREATE TABLE Staging.AMS_InvoiceTransaction
(
    InvoiceTransactionChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , BackInvTPId VARCHAR(255)
    , BHId VARCHAR(255)
    , BillMethod VARCHAR(255)
    , BillSeqId VARCHAR(255)
    , BillTranId VARCHAR(255)
    , BinderPolTEffDate DATETIME
    , BinderPostMethod VARCHAR(255)
    , BinderStatus VARCHAR(255)
    , BrokerCode VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , ChargeCat VARCHAR(255)
    , Chargecode VARCHAR(255)
    , CoCode VARCHAR(255)
    , CommPayType VARCHAR(255)
    , CoType VARCHAR(255)
    , CSHId VARCHAR(255)
    , CustId VARCHAR(255)
    , Description VARCHAR(255)
    , EnteredDate DATETIME
    , FullTermPremAmt MONEY
    , GLDate DATETIME
    , GrossAmt MONEY
    , InvEffDate DATETIME
    , InvId VARCHAR(255)
    , InvTPId VARCHAR(255)
    , IsCancelled VARCHAR(255)
    , IsInstallment VARCHAR(255)
    , IsPosted VARCHAR(255)
    , JournalTranId VARCHAR(255)
    , LineOfBus VARCHAR(255)
    , NewInvTPId VARCHAR(255)
    , NonPrRecipient VARCHAR(255)
    , OldInvTPId VARCHAR(255)
    , PlanType VARCHAR(255)
    , PolTEffDate DATETIME
    , Poltpid VARCHAR(255)
    , RBBkOutId VARCHAR(255)
    , ReplaceDate DATETIME
    , TranType VARCHAR(255)
    , WritingCoCode VARCHAR(255)
    , StagingDate DATETIME
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

CREATE CLUSTERED INDEX cix_InvoiceTransaction_InvTPId
    ON Staging.AMS_InvoiceTransaction (InvTPId);

CREATE CLUSTERED INDEX cix_InvoiceTransaction_InvTPId
    ON RawData.AMS_InvoiceTransaction (InvTPId);
GO

IF OBJECT_ID ('Staging.AMS_InvoiceTransaction_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_InvoiceTransaction_Load;
END;
GO

CREATE PROCEDURE Staging.AMS_InvoiceTransaction_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_InvoiceTransaction
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_InvoiceTransaction AS RawData
                        WHERE   RawData.BHId = AMS_InvoiceTransaction.InvTPId
                   );

    INSERT INTO Staging.AMS_InvoiceTransaction (InvoiceTransactionChecksum
                                                , Datasource
                                                , Deleted
                                                , Sequencenumber
                                                , BackInvTPId
                                                , BHId
                                                , BillMethod
                                                , BillSeqId
                                                , BillTranId
                                                , BinderPolTEffDate
                                                , BinderPostMethod
                                                , BinderStatus
                                                , BrokerCode
                                                , ChangedBy
                                                , ChangedDate
                                                , ChargeCat
                                                , Chargecode
                                                , CoCode
                                                , CommPayType
                                                , CoType
                                                , CSHId
                                                , CustId
                                                , Description
                                                , EnteredDate
                                                , FullTermPremAmt
                                                , GLDate
                                                , GrossAmt
                                                , InvEffDate
                                                , InvId
                                                , InvTPId
                                                , IsCancelled
                                                , IsInstallment
                                                , IsPosted
                                                , JournalTranId
                                                , LineOfBus
                                                , NewInvTPId
                                                , NonPrRecipient
                                                , OldInvTPId
                                                , PlanType
                                                , PolTEffDate
                                                , Poltpid
                                                , RBBkOutId
                                                , ReplaceDate
                                                , TranType
                                                , WritingCoCode
                                                , StagingDate
                                                , InsertDate)
    SELECT  AMS_InvoiceTransaction.InvoiceTransactionChecksum
            , AMS_InvoiceTransaction.Datasource
            , AMS_InvoiceTransaction.Deleted
            , AMS_InvoiceTransaction.Sequencenumber
            , AMS_InvoiceTransaction.BackInvTPId
            , AMS_InvoiceTransaction.BHId
            , AMS_InvoiceTransaction.BillMethod
            , AMS_InvoiceTransaction.BillSeqId
            , AMS_InvoiceTransaction.BillTranId
            , AMS_InvoiceTransaction.BinderPolTEffDate
            , AMS_InvoiceTransaction.BinderPostMethod
            , AMS_InvoiceTransaction.BinderStatus
            , AMS_InvoiceTransaction.BrokerCode
            , AMS_InvoiceTransaction.ChangedBy
            , AMS_InvoiceTransaction.ChangedDate
            , AMS_InvoiceTransaction.ChargeCat
            , AMS_InvoiceTransaction.Chargecode
            , AMS_InvoiceTransaction.CoCode
            , AMS_InvoiceTransaction.CommPayType
            , AMS_InvoiceTransaction.CoType
            , AMS_InvoiceTransaction.CSHId
            , AMS_InvoiceTransaction.CustId
            , AMS_InvoiceTransaction.Description
            , AMS_InvoiceTransaction.EnteredDate
            , AMS_InvoiceTransaction.FullTermPremAmt
            , AMS_InvoiceTransaction.GLDate
            , AMS_InvoiceTransaction.GrossAmt
            , AMS_InvoiceTransaction.InvEffDate
            , AMS_InvoiceTransaction.InvId
            , AMS_InvoiceTransaction.InvTPId
            , AMS_InvoiceTransaction.IsCancelled
            , AMS_InvoiceTransaction.IsInstallment
            , AMS_InvoiceTransaction.IsPosted
            , AMS_InvoiceTransaction.JournalTranId
            , AMS_InvoiceTransaction.LineOfBus
            , AMS_InvoiceTransaction.NewInvTPId
            , AMS_InvoiceTransaction.NonPrRecipient
            , AMS_InvoiceTransaction.OldInvTPId
            , AMS_InvoiceTransaction.PlanType
            , AMS_InvoiceTransaction.PolTEffDate
            , AMS_InvoiceTransaction.Poltpid
            , AMS_InvoiceTransaction.RBBkOutId
            , AMS_InvoiceTransaction.ReplaceDate
            , AMS_InvoiceTransaction.TranType
            , AMS_InvoiceTransaction.WritingCoCode
            , AMS_InvoiceTransaction.InsertDate
            , GETDATE () AS InsertDate
      --FROM  RawData.AMS_InvoiceTransaction;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_InvoiceTransaction.InvTPId
                                                  ORDER BY AMS_InvoiceTransaction.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_InvoiceTransaction
            ) AS AMS_InvoiceTransaction
     WHERE  AMS_InvoiceTransaction.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.AMS_InvoiceTransaction TO [abeerconsulting];

GRANT CONTROL ON RawData.AMS_InvoiceTransaction TO [abeerconsulting];
GO