//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML131;
using PDS.Framework;

using WbGeometry = Energistics.DataAccess.WITSML131.StandAloneWellboreGeometry;
using WbGeometryList = Energistics.DataAccess.WITSML131.WellboreGeometryList;

namespace PDS.Witsml.Server.Data.WbGeometries
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="WbGeometry"/>.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.WitsmlDataProvider{WbGeometryList, WbGeometry}" />
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<WbGeometry>))]
    [Export141(ObjectTypes.WbGeometry, typeof(IEtpDataProvider))]
    [Export141(ObjectTypes.WbGeometry, typeof(IWitsmlDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WbGeometry131DataProvider : WitsmlDataProvider<WbGeometryList, WbGeometry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WbGeometry131DataProvider"/> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="dataAdapter">The data adapter.</param>
        [ImportingConstructor]
        public WbGeometry131DataProvider(IContainer container, IWitsmlDataAdapter<WbGeometry> dataAdapter) : base(container, dataAdapter)
        {
        }

        /// <summary>
        /// Sets the default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        protected override void SetDefaultValues(WbGeometry dataObject)
        {
            dataObject.Uid = dataObject.NewUid();
            dataObject.CommonData = dataObject.CommonData.Create();

            SetAdditionalDefaultValues(dataObject);
        }

        /// <summary>
        /// Creates a new <see cref="WbGeometryList" /> instance containing the specified data objects.
        /// </summary>
        /// <param name="dataObjects">The data objects.</param>
        /// <returns>A new <see cref="WbGeometryList" /> instance.</returns>
        protected override WbGeometryList CreateCollection(List<WbGeometry> dataObjects)
        {
            return new WbGeometryList { WellboreGeometry = dataObjects };
        }

        /// <summary>
        /// Sets additional default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        partial void SetAdditionalDefaultValues(WbGeometry dataObject);
    }
}