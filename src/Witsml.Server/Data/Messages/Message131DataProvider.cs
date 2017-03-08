﻿//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2017.1
//
// Copyright 2017 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Energistics.Datatypes;

namespace PDS.Witsml.Server.Data.Messages
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="Message"/>.
    /// </summary>
    public partial class Message131DataProvider
    {
        /// <summary>
        /// Sets the additional default values.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="uri">The URI.</param>
        partial void SetAdditionalDefaultValues(Message dataObject, EtpUri uri)
        {
            dataObject.DateTime = dataObject.DateTime ?? DateTimeOffset.UtcNow;
            dataObject.TypeMessage = dataObject.TypeMessage ?? MessageType.unknown;
        }
    }
}
