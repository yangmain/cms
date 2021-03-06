﻿using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Abstractions;
using SSCMS.Core.Extensions;

namespace SSCMS.Controllers.Admin
{
    public partial class InstallController
    {
        [HttpPost, Route(RouteDatabaseConnect)]
        public async Task<ActionResult<DatabaseConnectResult>> DatabaseConnect([FromBody]DatabaseConnectRequest request)
        {
            if (!await _configRepository.IsNeedInstallAsync()) return Unauthorized();
            
            var connectionStringWithoutDatabaseName = GetDatabaseConnectionString(request.DatabaseType == DatabaseType.Oracle, request.DatabaseType, request.DatabaseHost, request.IsDatabaseDefaultPort, TranslateUtils.ToInt(request.DatabasePort), request.DatabaseUserName, request.DatabasePassword, string.Empty, request.OracleDatabase, request.OracleIsSid, request.OraclePrivilege);

            var db = new Database(request.DatabaseType, connectionStringWithoutDatabaseName);

            var (isConnectionWorks, message) = await db.IsConnectionWorksAsync();
            if (!isConnectionWorks)
            {
                return this.Error(message);
            }

            var databaseNames = await db.GetDatabaseNamesAsync();

            return new DatabaseConnectResult
            {
                DatabaseNames = databaseNames
            };
        }
    }
}
