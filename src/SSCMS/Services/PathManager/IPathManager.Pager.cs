﻿using System.Threading.Tasks;

namespace SSCMS.Abstractions
{
    public partial interface IPathManager
    {
        Task<string> GetUrlInChannelPageAsync(string type, Site site, Channel node, int index, int currentPageIndex,
            int pageCount, bool isLocal);

        Task<string> GetUrlInContentPageAsync(string type, Site site, int channelId, int contentId, int index, int currentPageIndex, int pageCount, bool isLocal);

        string GetClickStringInSearchPage(string type, string ajaxDivId, int index, int currentPageIndex, int pageCount);

        Task<string> GetJsMethodInDynamicPageAsync(string type, Site site, int channelId, int contentId, int index, int currentPageIndex, int pageCount, bool isPageRefresh, string ajaxDivId, bool isLocal);
    }
}
