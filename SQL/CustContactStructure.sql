IF OBJECT_ID ('RawData.AMS_CustContact') IS NOT NULL
BEGIN
    DROP TABLE RawData.AMS_CustContact;
END;
GO

CREATE TABLE RawData.AMS_CustContact
(
    CustContactChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , Address1 VARCHAR(255)
    , Address2 VARCHAR(255)
    , BusAreaCode VARCHAR(255)
    , BusExt VARCHAR(255)
    , BusPhone VARCHAR(255)
    , CCntId VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , City VARCHAR(255)
    , ContactMethod VARCHAR(255)
    , ContactName VARCHAR(255)
    , Country VARCHAR(255)
    , County VARCHAR(255)
    , CustCenterDisplay VARCHAR(255)
    , CustId VARCHAR(255)
    , EMail VARCHAR(255)
    , EnteredDate DATETIME
    , FaxAreaCode VARCHAR(255)
    , FaxExt VARCHAR(255)
    , FaxPhone VARCHAR(255)
    , FormalSalutation VARCHAR(255)
    , InformalSalutation VARCHAR(255)
    , IsDirector VARCHAR(255)
    , IsForeign BIT
    , IsOfficer VARCHAR(255)
    , MobileAreaCode VARCHAR(255)
    , MobileExt VARCHAR(255)
    , MobilePhone VARCHAR(255)
    , Notes VARCHAR(MAX)
    , PagerAreaCode VARCHAR(255)
    , PagerExt VARCHAR(255)
    , PagerPhone VARCHAR(255)
    , ResAreaCode VARCHAR(255)
    , ResExt VARCHAR(255)
    , ResPhone VARCHAR(255)
    , SortOrder SMALLINT
    , CustContactState VARCHAR(255)
    , Title VARCHAR(255)
    , Zip VARCHAR(255)
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

IF OBJECT_ID ('Staging.AMS_CustContact') IS NOT NULL
BEGIN
    DROP TABLE Staging.AMS_CustContact;
END;
GO

CREATE TABLE Staging.AMS_CustContact
(
    CustContactChecksum VARCHAR(255)
    , Datasource VARCHAR(255)
    , Deleted BIT
    , Sequencenumber DECIMAL(18, 4)
    , Address1 VARCHAR(255)
    , Address2 VARCHAR(255)
    , BusAreaCode VARCHAR(255)
    , BusExt VARCHAR(255)
    , BusPhone VARCHAR(255)
    , CCntId VARCHAR(255)
    , ChangedBy VARCHAR(255)
    , ChangedDate DATETIME
    , City VARCHAR(255)
    , ContactMethod VARCHAR(255)
    , ContactName VARCHAR(255)
    , Country VARCHAR(255)
    , County VARCHAR(255)
    , CustCenterDisplay VARCHAR(255)
    , CustId VARCHAR(255)
    , EMail VARCHAR(255)
    , EnteredDate DATETIME
    , FaxAreaCode VARCHAR(255)
    , FaxExt VARCHAR(255)
    , FaxPhone VARCHAR(255)
    , FormalSalutation VARCHAR(255)
    , InformalSalutation VARCHAR(255)
    , IsDirector VARCHAR(255)
    , IsForeign BIT
    , IsOfficer VARCHAR(255)
    , MobileAreaCode VARCHAR(255)
    , MobileExt VARCHAR(255)
    , MobilePhone VARCHAR(255)
    , Notes VARCHAR(MAX)
    , PagerAreaCode VARCHAR(255)
    , PagerExt VARCHAR(255)
    , PagerPhone VARCHAR(255)
    , ResAreaCode VARCHAR(255)
    , ResExt VARCHAR(255)
    , ResPhone VARCHAR(255)
    , SortOrder SMALLINT
    , CustContactState VARCHAR(255)
    , Title VARCHAR(255)
    , Zip VARCHAR(255)
    , StagingDate DATETIME
    , InsertDate DATETIME NOT NULL
          DEFAULT (GETDATE ())
);
GO

GO

CREATE CLUSTERED INDEX cix_CustContact_CCntId
    ON Staging.AMS_CustContact (CCntId);

CREATE CLUSTERED INDEX cix_CustContact_CCntId
    ON RawData.AMS_CustContact (CCntId);
GO

IF OBJECT_ID ('Staging.AMS_CustContact_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.AMS_CustContact_Load;
END;
GO

CREATE PROCEDURE Staging.AMS_CustContact_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.AMS_CustContact
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.AMS_CustContact AS RawData
                        WHERE   RawData.CCntId = AMS_CustContact.CCntId
                   );

    INSERT INTO Staging.AMS_CustContact (CustContactChecksum
                                         , Datasource
                                         , Deleted
                                         , Sequencenumber
                                         , Address1
                                         , Address2
                                         , BusAreaCode
                                         , BusExt
                                         , BusPhone
                                         , CCntId
                                         , ChangedBy
                                         , ChangedDate
                                         , City
                                         , ContactMethod
                                         , ContactName
                                         , Country
                                         , County
                                         , CustCenterDisplay
                                         , CustId
                                         , EMail
                                         , EnteredDate
                                         , FaxAreaCode
                                         , FaxExt
                                         , FaxPhone
                                         , FormalSalutation
                                         , InformalSalutation
                                         , IsDirector
                                         , IsForeign
                                         , IsOfficer
                                         , MobileAreaCode
                                         , MobileExt
                                         , MobilePhone
                                         , Notes
                                         , PagerAreaCode
                                         , PagerExt
                                         , PagerPhone
                                         , ResAreaCode
                                         , ResExt
                                         , ResPhone
                                         , SortOrder
                                         , CustContactState
                                         , Title
                                         , Zip
                                         , StagingDate
                                         , InsertDate)
    SELECT  AMS_CustContact.CustContactChecksum
            , AMS_CustContact.Datasource
            , AMS_CustContact.Deleted
            , AMS_CustContact.Sequencenumber
            , AMS_CustContact.Address1
            , AMS_CustContact.Address2
            , AMS_CustContact.BusAreaCode
            , AMS_CustContact.BusExt
            , AMS_CustContact.BusPhone
            , AMS_CustContact.CCntId
            , AMS_CustContact.ChangedBy
            , AMS_CustContact.ChangedDate
            , AMS_CustContact.City
            , AMS_CustContact.ContactMethod
            , AMS_CustContact.ContactName
            , AMS_CustContact.Country
            , AMS_CustContact.County
            , AMS_CustContact.CustCenterDisplay
            , AMS_CustContact.CustId
            , AMS_CustContact.EMail
            , AMS_CustContact.EnteredDate
            , AMS_CustContact.FaxAreaCode
            , AMS_CustContact.FaxExt
            , AMS_CustContact.FaxPhone
            , AMS_CustContact.FormalSalutation
            , AMS_CustContact.InformalSalutation
            , AMS_CustContact.IsDirector
            , AMS_CustContact.IsForeign
            , AMS_CustContact.IsOfficer
            , AMS_CustContact.MobileAreaCode
            , AMS_CustContact.MobileExt
            , AMS_CustContact.MobilePhone
            , AMS_CustContact.Notes
            , AMS_CustContact.PagerAreaCode
            , AMS_CustContact.PagerExt
            , AMS_CustContact.PagerPhone
            , AMS_CustContact.ResAreaCode
            , AMS_CustContact.ResExt
            , AMS_CustContact.ResPhone
            , AMS_CustContact.SortOrder
            , AMS_CustContact.CustContactState
            , AMS_CustContact.Title
            , AMS_CustContact.Zip
            , AMS_CustContact.InsertDate
            , GETDATE () AS InsertDate
      --FROM  RawData.AMS_CustContact;
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY AMS_CustContact.CCntId
                                                  ORDER BY AMS_CustContact.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.AMS_CustContact
            ) AS AMS_CustContact
     WHERE  AMS_CustContact.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.AMS_CustContact TO [abeerconsulting];

GRANT CONTROL ON RawData.AMS_CustContact TO [abeerconsulting];
GO

GO