﻿using System.Collections.Generic;
using SSCMS.Abstractions.Dto.Request;

namespace SSCMS.Controllers.Admin.Cms.Channels
{
    public partial class ChannelsLayerTaxisController
    {
        public class SubmitRequest : SiteRequest
        {
            public IEnumerable<int> ChannelIds { get; set; }
            public bool IsUp { get; set; }
            public int Taxis { get; set; }
        }
    }
}