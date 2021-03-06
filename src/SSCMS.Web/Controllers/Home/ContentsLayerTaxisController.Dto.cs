﻿using System.Collections.Generic;
using SSCMS.Abstractions.Dto.Request;

namespace SSCMS.Controllers.Home
{
    public partial class ContentsLayerTaxisController
    {
        public class SubmitRequest : ChannelRequest
        {
            public List<int> ContentIds { get; set; }
            public bool IsUp { get; set; }
            public int Taxis { get; set; }
        }
    }
}
