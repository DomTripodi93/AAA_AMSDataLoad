IF OBJECT_ID ('RawData.AMS_LineOfBusiness') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_LineOfBusiness;
END;
GO

CREATE TABLE RawData.AMS_LineOfBusiness
(
    LineOfBusinessChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , AppCreatedDate DATETIME
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , Description VARCHAR(255)
    , EffDate DATETIME
    , ElfFormVerId VARCHAR(255)
    , EnteredDate DATETIME
    , ExpDate DATETIME
    , InsertSeqNo INT
    , LineOfBus VARCHAR(255)
    , LOBChangedDate DATETIME
    , LOBId VARCHAR(255)
    , PlanType VARCHAR(255)
    , PolId VARCHAR(255)
    , SortNo SMALLINT
    , StatePlanType VARCHAR(255)
    , UICodeLOBS VARCHAR(255)
    , WritingCoCode VARCHAR(255)
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

IF OBJECT_ID ('Staging.AMS_LineOfBusiness') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_LineOfBusiness;
END;
GO

CREATE TABLE Staging.AMS_LineOfBusiness
(
    LineOfBusinessChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , AppCreatedDate DATETIME
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , Description VARCHAR(255)
    , EffDate DATETIME
    , ElfFormVerId VARCHAR(255)
    , EnteredDate DATETIME
    , ExpDate DATETIME
    , InsertSeqNo INT
    , LineOfBus VARCHAR(255)
    , LOBChangedDate DATETIME
    , LOBId VARCHAR(255)
    , PlanType VARCHAR(255)
    , PolId VARCHAR(255)
    , SortNo SMALLINT
    , StatePlanType VARCHAR(255)
    , UICodeLOBS VARCHAR(255)
    , WritingCoCode VARCHAR(255)
    , StagingDate DATETIME
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

CREATE CLUSTERED INDEX cix_LineOfBusiness_LOBId_PolId
    ON Staging.AMS_LineOfBusiness (LOBId, PolId);

CREATE CLUSTERED INDEX cix_LineOfBusiness_LOBId_PolId
    ON RawData.AMS_LineOfBusiness (LOBId, PolId);
GO

IF OBJECT_ID ('Staging.AMS_LineOfBusiness_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_LineOfBusiness_Load;
END;
GO

CREATE PROCEDURE Staging.AMS_LineOfBusiness_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_LineOfBusiness
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_LineOfBusiness AS RawData
                        WHERE   RawData.LOBId = AMS_LineOfBusiness.LOBId
                                AND RawData.PolId = AMS_LineOfBusiness.PolId
                   );

    INSERT INTO Staging.AMS_LineOfBusiness (LineOfBusinessChecksum
                                            , Datasource
                                            , Deleted
                                            , Sequencenumber
                                            , AppCreatedDate
                                            , ChangedBy
                                            , ChangedDate
                                            , Description
                                            , EffDate
                                            , ElfFormVerId
                                            , EnteredDate
                                            , ExpDate
                                            , InsertSeqNo
                                            , LineOfBus
                                            , LOBChangedDate
                                            , LOBId
                                            , PlanType
                                            , PolId
                                            , SortNo
                                            , StatePlanType
                                            , UICodeLOBS
                                            , WritingCoCode
                                            , StagingDate
                                            , InsertDate)
    SELECT  AMS_LineOfBusiness.LineOfBusinessChecksum
            , AMS_LineOfBusiness.Datasource
            , AMS_LineOfBusiness.Deleted
            , AMS_LineOfBusiness.Sequencenumber
            , AMS_LineOfBusiness.AppCreatedDate
            , AMS_LineOfBusiness.ChangedBy
            , AMS_LineOfBusiness.ChangedDate
            , AMS_LineOfBusiness.Description
            , AMS_LineOfBusiness.EffDate
            , AMS_LineOfBusiness.ElfFormVerId
            , AMS_LineOfBusiness.EnteredDate
            , AMS_LineOfBusiness.ExpDate
            , AMS_LineOfBusiness.InsertSeqNo
            , AMS_LineOfBusiness.LineOfBus
            , AMS_LineOfBusiness.LOBChangedDate
            , AMS_LineOfBusiness.LOBId
            , AMS_LineOfBusiness.PlanType
            , AMS_LineOfBusiness.PolId
            , AMS_LineOfBusiness.SortNo
            , AMS_LineOfBusiness.StatePlanType
            , AMS_LineOfBusiness.UICodeLOBS
            , AMS_LineOfBusiness.WritingCoCode
            , AMS_LineOfBusiness.InsertDate
            , GETDATE () AS InsertDate
      --FROM  RawData.AMS_LineOfBusiness;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_LineOfBusiness.LOBId
                                                           , AMS_LineOfBusiness.PolId
                                                  ORDER BY AMS_LineOfBusiness.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_LineOfBusiness
            ) AS AMS_LineOfBusiness
     WHERE  AMS_LineOfBusiness.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.AMS_LineOfBusiness TO [abeerconsulting];

GRANT CONTROL ON RawData.AMS_LineOfBusiness TO [abeerconsulting];
GO