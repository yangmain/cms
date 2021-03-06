﻿using System.Collections.Generic;
using SSCMS.Abstractions.Dto.Request;

namespace SSCMS.Controllers.Admin.Cms.Contents
{
    public partial class ContentsLayerTagController
    {
        public class SubmitRequest : ChannelRequest
        {
            public string ChannelContentIds { get; set; }
            public bool IsCancel { get; set; }
            public IEnumerable<string> TagNames { get; set; }
        }
    }
}