﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Energistics.Common;
using Energistics.Datatypes.Object;
using Energistics.Protocol.Discovery;

namespace PDS.WITSMLstudio.Store.Providers.Discovery
{
    /// <summary>
    /// Defines properties and methods that can be used to discover resources available in a WITSML store.
    /// </summary>
    public interface IDiscoveryStoreProvider
    {
        /// <summary>
        /// Gets the data schema version supported by the provider.
        /// </summary>
        /// <value>The data schema version.</value>
        string DataSchemaVersion { get; }

        /// <summary>
        /// Gets a collection of resources associated to the specified URI.
        /// </summary>
        /// <param name="args">The ProtocolEventArgs{GetResources, IList{Resource}} instance containing the event data.</param>
        void GetResources(ProtocolEventArgs<GetResources, IList<Resource>> args);
    }
}
