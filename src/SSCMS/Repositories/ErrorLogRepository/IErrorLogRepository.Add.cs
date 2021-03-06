﻿using System;
using System.Threading.Tasks;

namespace SSCMS.Abstractions
{
    public partial interface IErrorLogRepository
    {
        Task<int> AddErrorLogAsync(ErrorLog log);
        Task<int> AddErrorLogAsync(Exception ex, string summary = "");
        Task<int> AddErrorLogAsync(string pluginId, Exception ex, string summary = "");
    }
}
