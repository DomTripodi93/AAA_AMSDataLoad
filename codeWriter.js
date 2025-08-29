
let cSharpTypeMapping = {
    "bigint": "long?",
    "binary*": "byte[]?",
    "bit": "bool?",
    "byte": "byte?",
    "char": "string?",
    "date": "DateTime?",
    "datetime": "DateTime?",
    "datetime2": "DateTime?",
    "datetimeoffset": "DateTimeOffset?",
    "decimal": "decimal?",
    "image*": "byte[]?",
    "int": "int?",
    "integer": "int?",
    "money": "decimal?",
    "nchar": "string?",
    "ntext": "string?",
    "numeric": "decimal?",
    "nvarchar": "string?",
    "real": "Single?",
    "rowversion": "byte[]?",
    "smalldatetime": "DateTime?",
    "smallint": "short?",
    "smallmoney": "decimal?",
    "sysname": "string?",
    "text": "string?",
    "time": "TimeSpan?",
    "tinyint": "byte?",
    "uniqueidentifier": "string?",
    "varbinary*": "byte[]?",
    "varchar": "string?",
    "xml": "string?",

    "float": "decimal?",
    "number": "decimal?",
    "string": "string?",
    "datetime": "string?",
    "Date": "string?",
    // "Date": "DateTime?",
    "boolean": "bool?",
    "object": "List<string>?"
}

let cSharpParameterTypeMapping = {
    "bigint": "DbType.Int64",
    "binary*": "DbType.Binary",
    "bit": "DbType.Boolean",
    "byte": "DbType.Byte",
    "char": "DbType.String",
    "date": "DbType.Date",
    "datetime": "DbType.DateTime",
    "datetime2": "DbType.DateTime2",
    "datetimeoffset": "DbType.DateTimeOffset",
    "decimal": "DbType.Decimal",
    "geography": "DbType.Object",
    "geometry": "DbType.Object",
    "hierarchyid": "DbType.Object",
    "image*": "DbType.Binary",
    "int": "DbType.Int32",
    "integer": "DbType.Int32",
    "money": "DbType.Decimal",
    "nchar": "DbType.StringFixedLength",
    "ntext": "DbType.String",
    "numeric": "DbType.Decimal",
    "nvarchar": "DbType.String",
    "real": "DbType.Single",
    "rowversion": "DbType.Binary",
    "smalldatetime": "DbType.DateTime",
    "smallint": "DbType.Int16",
    "smallmoney": "DbType.Decimal",
    "sysname": "DbType.String",
    "text": "DbType.String",
    "time": "DbType.Time",
    "tinyint": "DbType.Byte",
    "uniqueidentifier": "DbType.String",
    "varbinary*": "DbType.Binary",
    "varchar": "DbType.String",
    "xml": "DbType.String",

    "float": "DbType.Decimal",
    "number": "DbType.Decimal",
    "uniqueidentifier": "DbType.String",
    "varchar": "DbType.String",
    "char": "DbType.String",
    "string": "DbType.String",
    "datetime": "DbType.DateTime",
    "Date": "DbType.DateTime",
    "boolean": "DbType.Boolean",
    "object": "DbType.String"
}

let sqlTypeMapping = {
    "bigint": "bigint",
    "binary*": "binary*",
    "bit": "bit",
    "byte": "byte",
    "char": "VARCHAR(255)",
    "date": "date",
    "datetime": "datetime",
    "datetime2": "datetime2",
    "datetimeoffset": "datetimeoffset",
    "decimal": "decimal",
    "float": "float",
    "geography": "geography",
    "geometry": "geometry",
    "hierarchyid": "hierarchyid",
    "image*": "image*",
    "int": "int",
    "integer": "integer",
    "money": "money",
    "nchar": "VARCHAR(255)",
    "ntext": "VARCHAR(255)",
    "numeric": "numeric",
    "nvarchar": "VARCHAR(255)",
    "real": "real",
    "rowversion": "rowversion",
    "smalldatetime": "smalldatetime",
    "smallint": "smallint",
    "smallmoney": "smallmoney",
    "sysname": "sysname",
    "text": "VARCHAR(255)",
    "time": "time",
    "tinyint": "tinyint",
    "uniqueidentifier": "VARCHAR(255)",
    "varbinary*": "varbinary*",
    "varchar": "VARCHAR(255)",
    "xml": "xml",


    "float": "DECIMAL(18,4)",
    "number": "DECIMAL(18,4)",
    "string": "VARCHAR(255)",
    "Date": "DATETIME",
    "boolean": "BIT",
    "object": "VARCHAR(MAX)"
}


const fs = require("fs");

let dataLoadFor = "AMS";
let dataModelPrefix = "AMS";
let resultKey = "value";

let finalize = true;

let examplesFolder = "AutoCodeOutputs/DataDefinitionFiles/";
let sqlFolder = "AutoCodeOutputs/SQLOutputs/";
let modelsFolder = "AutoCodeOutputs/Models/";
let handlersFolder = "AutoCodeOutputs/Handlers/";

if (finalize) {
    sqlFolder = "AutoCodeOutputs/SQLOutputs/";
    modelsFolder = "Models/";
    handlersFolder = "Handlers/";
}

let protectedWords = ["id", "name", "status", "subject", "state", "checksum"]

fs.readFile(examplesFolder + "fileList.json", { encoding: 'utf-8' }, (err, fileResult) => {
    // console.log(fileResult);
    let fileList = JSON.parse(fileResult);
    fileList.forEach((filePath, i) => {
        fs.readFile(examplesFolder + filePath.split(".json").join("Structure.txt"), { encoding: 'utf-8' }, (errOuter, strucFile) => {
            let fieldInfoStruc = []
            let strucTypes = { "__$deleted": "bit" };
            let pascalNames = {}

            let structRowsProcessed = 0;
            let structRows = strucFile.split("\n")

            structRows.forEach(structRow => {
                let structRowParts = structRow.split("\t");
                if (structRowParts.length > 2) {
                    // strucTypes[pascalToCamelCase(structRowParts[0])] = structRowParts[2].replace("\r","")
                    strucTypes[structRowParts[0].toLowerCase()] = structRowParts[2].replace("\r", "")
                    pascalNames[structRowParts[0].toLowerCase()] = structRowParts[0]
                } else if (structRow.length > 0) {
                    console.log(filePath)
                    console.log(structRow)
                }

                structRowsProcessed++
                if (structRowsProcessed === structRows.length) {
                    // console.log(strucTypes)

                    fs.readFile(examplesFolder + filePath, { encoding: 'utf-8' }, (err, dataResult) => {
                        // console.log(dataResult);
                        let fieldInfo = [];
                        let dataRowsProcessed = 0;
                        let modelName = filePath.split(".")[0];
                        // if (modelName[modelName.length - 1] === "s") {
                        //     modelName = modelName.substring(0, modelName.length - 1)
                        // }

                        // console.log(dataResult)
                        let jsonData = JSON.parse(dataResult);
                        let dataValues = jsonData["content"];
                        // console.log(dataValues)
                        // let dataRows = dataValues[pascalToCamelCase(modelName)];
                        // let rowForValues = dataRows[0];

                        let objKeys = Object.keys(strucTypes)
                        let rowForValues = {}

                        if (dataValues.length > 0) {

                            rowForValues = dataValues[0];
                            // console.log(filePath)
                            objKeys = Object.keys(rowForValues)
                        }

                        // console.log(rowForValues)

                        objKeys.forEach((key, i) => {
                            // console.log(key)
                            // console.log(!key.includes("_$"))
                            // console.log(row)
                            let fieldType = strucTypes[key]
                            if (dataValues.length > 0) {

                                fieldType = strucTypes[key] ?? typeof (rowForValues[key]);
                                if (
                                    Date.parse(rowForValues[key]) + "" !== "NaN" &&
                                    (+rowForValues[key]) + "" === "NaN" &&
                                    typeof (rowForValues[key]) !== "number"
                                ) {
                                    fieldType = "Date";
                                }
                                if (rowForValues[key] === null && !strucTypes[key]) {
                                    fieldType = "string";
                                }
                            }

                            let fieldNameOriginal = key
                            let fieldName = fieldNameOriginal.replace(modelName + "_", "").replace("__$", "")

                            fieldName = pascalNames[fieldName] ?? camelToPasal(fieldName)
                            // fieldName = camelToPasal(fieldName)

                            if (protectedWords.includes(fieldName.toLowerCase())) {
                                fieldName = removeNumbers(modelName) + fieldName;
                            }
                            let sqlType = sqlTypeMapping[fieldType];
                            let cSharpType = cSharpTypeMapping[fieldType];
                            let cSharpParameterType = cSharpParameterTypeMapping[fieldType];

                            fieldInfo.push({
                                fieldNameOriginal, fieldName, sqlType, cSharpType, cSharpParameterType
                            })
                            dataRowsProcessed++
                            if (dataRowsProcessed === objKeys.length) {
                                waitTen().then(() => {
                                    writeFiles(fieldInfo, modelName, dataModelPrefix.toLocaleLowerCase() + modelName, i);
                                })
                            }
                        })


                    })
                }
            })
        })
    })
})

function waitTen() {
    return new Promise(resolve => {
        setTimeout(() => {
            resolve()
        }, 2)
    })
}


function writeFiles(fieldInfo, modelName, camelModelName, fileIndex) {

    let modelOutput = `using System;
    using System.Text.Json.Serialization;
    
    namespace ${dataLoadFor}DataLoad.Models
    {
        public partial class ${dataModelPrefix + modelName}
    	{ 
    `;
    let constructor = `
            public ${dataModelPrefix + modelName}()
            { 
    `
    let tableOutput = `
IF OBJECT_ID('RawData.${dataLoadFor}_${modelName}') IS NOT NULL
BEGIN
    DROP TABLE RawData.${dataLoadFor}_${modelName}
END
GO

CREATE TABLE RawData.${dataLoadFor}_${modelName}(
`;

    // let modelFields = [];

    let addParams = ""
    let addForSqlString = ""
    let sqlLoadProc = "";
    let tableIndex = "";


    let processedFields = 0

    fieldInfo.forEach(async (singleField, i) => {
        let additionalConstraint = "";

        if (i === 0) {
            addForSqlString += "\"(@" + singleField.fieldName + "\" + countForCurrent.ToString() +"
        } else {
            addForSqlString += "\n\t\t\t\t\t\",@" + singleField.fieldName + "\" + countForCurrent.ToString() +"
        }

        addParams += "\n\t\t\t\tsqlParameters.Add(\"@" + singleField.fieldName + "\" + countForCurrent.ToString(), "
            + (singleField.cSharpParameterType === "DbType.DateTime" ? "_helper.FormatDate(" : "")
            + camelModelName + "." + singleField.fieldName
            + (singleField.cSharpParameterType === "DbType.DateTime" ? " + \"\")" : "")
            + ", " + singleField.cSharpParameterType + ");";

        tableOutput += "\n\t" + singleField.fieldName + " " + singleField.sqlType + ","

        modelOutput += "\t\t[JsonPropertyName(\"" + singleField.fieldNameOriginal + "\")]\n"
        modelOutput += "\t\tpublic " + singleField.cSharpType + " " + singleField.fieldName + " { get; set; }"
            // + (singleField.cSharpType === "string" ? " = \"\";" : "") 
            + "\n"

        processedFields++;
        if (processedFields === fieldInfo.length) {
            // console.log("hit")
            modelOutput += "\n\t}\n}";
            tableOutput = tableOutput + "\n\tInsertDate DATETIME NOT NULL DEFAULT(GETDATE())\n)";
            addForSqlString = addForSqlString + "\n\t\t\t\t\t\",GETDATE()),\";";
            // console.log(modelOutput)
            writeResult(modelOutput, modelsFolder + dataModelPrefix + modelName + ".cs", "Models")
            writeSqlOutput(modelName, fieldInfo, tableOutput, fileIndex, tableIndex);

            writeHandlerClass(modelName, camelModelName, fieldInfo, addForSqlString, addParams);
        }
    })
}

function writeSqlOutput(pascalModelName, modelFields, tableOutput, fileIndex, tableIndex) {
    let stagingProc = `

CREATE CLUSTERED INDEX cix_${pascalModelName}_Company_${pascalModelName}Id ON Staging.${dataLoadFor}_${pascalModelName}(Company, ${pascalModelName}Id)
CREATE CLUSTERED INDEX cix_${pascalModelName}_Company_${pascalModelName}Id ON RawData.${dataLoadFor}_${pascalModelName}(Company, ${pascalModelName}Id)

GO

IF OBJECT_ID('Staging.${dataLoadFor}_${pascalModelName}_Load') IS NOT NULL
BEGIN
    DROP PROCEDURE Staging.${dataLoadFor}_${pascalModelName}_Load
END
GO

CREATE PROCEDURE Staging.${dataLoadFor}_${pascalModelName}_Load
AS
BEGIN
    BEGIN TRANSACTION;

    DELETE  FROM Staging.${dataLoadFor}_${pascalModelName}
     WHERE  EXISTS (
                       SELECT   *
                         FROM   RawData.${dataLoadFor}_${pascalModelName} AS RawData
                         WHERE   RawData.${pascalModelName}Id = ${dataLoadFor}_${pascalModelName}.${pascalModelName}Id
                   );

    INSERT INTO Staging.${dataLoadFor}_${pascalModelName} (
        ${modelFields.map(row => row.fieldName).join("\n\t\t, ")}
        , StagingDate
        , InsertDate
    )
    SELECT ${dataLoadFor}_${pascalModelName}.${modelFields.map(row => row.fieldName).join("\n\t\t, " + dataLoadFor + "_" + pascalModelName + ".")}
        , ${dataLoadFor}_${pascalModelName}.InsertDate
        , GETDATE () AS InsertDate
    --FROM  RawData.${dataLoadFor}_${pascalModelName};
      FROM  (
                SELECT  *
                        , ROW_NUMBER () OVER (PARTITION BY ${dataLoadFor}_${pascalModelName}.${pascalModelName}Id
                                                  ORDER BY ${dataLoadFor}_${pascalModelName}.InsertDate DESC
                                             ) AS RelatedRow
                  FROM  RawData.${dataLoadFor}_${pascalModelName}
            ) AS ${dataLoadFor}_${pascalModelName}
     WHERE  ${dataLoadFor}_${pascalModelName}.RelatedRow = 1;

    COMMIT TRANSACTION;
END;
GO

GRANT CONTROL ON Staging.${dataLoadFor}_${pascalModelName} TO [abeerconsulting];
GRANT CONTROL ON RawData.${dataLoadFor}_${pascalModelName} TO [abeerconsulting];
GO
`

    let stagingTable = tableOutput.split("RawData").join("Staging").split("InsertDate DATETIME NOT NULL DEFAULT(GETDATE())").join("StagingDate DATETIME, \n\tInsertDate DATETIME NOT NULL DEFAULT(GETDATE())")

    let sqlToWrite = tableOutput + "\nGO\n\n" + tableIndex + "\nGO\n\n" +
        stagingTable + "\nGO\n\n" + tableIndex.split("RawData").join("Staging") + "\nGO\n\n" + stagingProc + "\nGO\n\n";
    writeResult(sqlToWrite, sqlFolder + pascalModelName + "Structure.sql");
}




function writeHandlerClass(pascalModelName, camelModelName, modelFields, addForSqlString, addParams) {
    let classString = `
using System.Data;
using Microsoft.Extensions.Configuration;
using ${dataLoadFor}DataLoad.Models;
using ${dataLoadFor}DataLoad.Handlers;
using System.Net;
using System.Security;
using Dapper;
using System.Text;
using ${dataLoadFor}DataLoad.Data;
using System.Text.Json;
using ${dataLoadFor}DataLoad.Helpers;

namespace ${dataLoadFor}DataLoad.Handlers
{
    public class ${dataModelPrefix + pascalModelName}Handler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "${pascalModelName}";
		private string _lastResponse = "";

        public ${dataModelPrefix + pascalModelName}Handler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsert${pascalModelName}(string rootRequestUrl, string filterDateString)
        {
            string emailBody = "";
            await Task.Run(() =>
            {
                string errorPage = "";
                string activeStep = "";
                try
                {
                    string nextPage = "";
                    int topValue = 1000;
                    string tableName = "AFW_${pascalModelName}?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.${dataLoadFor}_${pascalModelName}_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.${dataLoadFor}_${pascalModelName}");

                    while (moreRecordsLeft)
                    {
                        string dataUrl = rootRequestUrl + tableName
                        + "limit=" + topValue
                        + "&schema=public&select=*"
                        + (nextPage != "" ? "&starting_token=" + nextPage : "");
                        // + "&"
                        // + "searchStartDate=" + filterDateString" + topValue 
                        // + "&"
                        // + "$skip=" + skipValue
                        // + "&"
                        // + "$count=true";

                        errorPage = nextPage;

                        activeStep = "Getting " + _dataset;
                        _logging.WriteLogForModel(activeStep, _dataset);
                        _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                        _logging.WriteLogForModel(dataUrl, _dataset);
                        string dataString = _rootReqHandler.GetStringResult(dataUrl);
						_lastResponse = dataString;


                        Console.WriteLine(dataString.Substring(0, 200 < dataString.Length ? 200 : dataString.Length));
                        APIResponse<${dataModelPrefix + pascalModelName}> apiResponse = JsonSerializer.Deserialize<APIResponse<${dataModelPrefix + pascalModelName}>>(dataString) ?? new APIResponse<${dataModelPrefix + pascalModelName}>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsert${pascalModelName}s(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.${dataLoadFor}_${pascalModelName}_Load");

                    _logging.WriteLogForModel("${pascalModelName} Staging Load Completed", _dataset);
                    _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                }
                catch (Exception exception)
                {
                    _logging.WriteLogForModel(exception.Message, _dataset);
                    _logging.WriteLogForModel(exception.StackTrace != null ? exception.StackTrace : "", _dataset);
                    if (_lastResponse != "")
                    {
                        _logging.WriteLogForModel("-----Begin Previous Response:-----", _dataset);
                        _logging.WriteLogForModel(_lastResponse, _dataset);
                        _logging.WriteLogForModel("-----End Previous Response-----", _dataset);
                    }
                    _logging.WriteLogForModel("On Step " + activeStep + ": \\n" + exception.Message, _dataset, true);
                    emailBody += "On Step " + activeStep + ", On Skip Value " + errorPage + ": \\n" + exception.Message + "\\n";
                }
            });
            return emailBody;
        }

        public void CheckAndInsert${pascalModelName}s(IEnumerable<${dataModelPrefix + pascalModelName}> ${camelModelName}s, string activeStep)
        {
            if (${camelModelName}s != null && ${camelModelName}s.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                Insert${pascalModelName}s(${camelModelName}s);
            }
            else
            {
                _logging.WriteLogForModel("No ${pascalModelName}s Found in Date Range", _dataset);
            }
        }

        public void Insert${pascalModelName}s(IEnumerable<${dataModelPrefix + pascalModelName}> ${camelModelName}s)
        {

            // string insertRoot = "INSERT INTO RawData.${dataLoadFor}_${pascalModelName}(${modelFields.map(row => row.fieldName).join(",")},InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.${dataLoadFor}_${pascalModelName} VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (${dataModelPrefix + pascalModelName} ${camelModelName} in ${camelModelName}s)
            {
                countForCurrent += 1;
                string sqlToAdd = ${addForSqlString}


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }

${addParams}

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("${pascalModelName}s Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}`;

    // console.log(classString)
    writeResult(classString, handlersFolder + dataModelPrefix + pascalModelName + "Handler.cs")

    // console.log(tableOutput)
    // console.log(modelOutput)
    // console.log(addForSqlString)
    // console.log(addParams)
    // console.log("INSERT INTO RawData.${dataLoadFor}_" + pascalModelName + "(" + modelFields.join(",") + ",InsertDate) VALUES ")
}

function camelToPasal(str) {
    return str[0].toUpperCase() + str.substring(1);
}


function pascalToCamelCase(str) {
    let acronymReducedString = str.replace(/^([A-Z]+)(?=[A-Z][a-z]|$)/, (match) => match.toLowerCase());
    return acronymReducedString.charAt(0).toLowerCase() + acronymReducedString.slice(1);
}

function removeNumbers(str) {
    return str.replace(/[0-9]/g, '');
}

function writeResult(content, fileName) {
    fs.writeFile(fileName, content, err => {
        if (err) {
            console.error(err);
        }
    });
}

function writeOrAppendResult(content, fileName, fileIndex) {
    if (fileIndex === 0) {
        fs.writeFile(fileName, content, err => {
            if (err) {
                console.error(err);
            }
        });
    } else {
        fs.appendFile(fileName, content, err => {
            if (err) {
                console.error(err);
            }
        });
    }
}