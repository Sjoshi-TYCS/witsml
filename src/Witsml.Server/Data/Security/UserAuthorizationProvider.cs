﻿//----------------------------------------------------------------------- 
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
using System.Linq;
using log4net;
using PDS.Framework;
using PDS.Witsml.Server.Configuration;

namespace PDS.Witsml.Server.Data.Security
{
    /// <summary>
    /// Provides a base implementation of <see cref="IUserAuthorizationProvider"/>.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.Security.IUserAuthorizationProvider" />
    [Export(typeof(IUserAuthorizationProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class UserAuthorizationProvider : IUserAuthorizationProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UserAuthorizationProvider));

        /// <summary>
        /// Gets or sets the user authorization adapters.
        /// </summary>
        /// <value>The user authorization adapters.</value>
        [ImportMany]
        public List<IUserAuthorizationAdapter> UserAuthorizationAdapters { get; set; }

        /// <summary>
        /// Verifies that the current user is authorized to execute the requested ETP action.
        /// </summary>
        public void CheckEtpAccess()
        {
            if (!IsAuthorized(WitsmlEndpointTypes.Etp))
            {
                throw new WitsmlException(ErrorCodes.InsufficientOperationRights);
            }
        }

        /// <summary>
        /// Verifies that the current user is authorized to execute the requested SOAP action.
        /// </summary>
        public void CheckSoapAccess()
        {
            if (!IsAuthorized(WitsmlEndpointTypes.Soap))
            {
                throw new WitsmlException(ErrorCodes.InsufficientOperationRights);
            }
        }

        /// <summary>
        /// Determines whether the current user is authorized to execute the requested action.
        /// </summary>
        /// <param name="endpointType">The type of endpoint.</param>
        /// <returns><c>true</c> if the current user is authorized; otherwise, <c>false</c>.</returns>
        private bool IsAuthorized(WitsmlEndpointTypes endpointType)
        {
            if (!WitsmlSettings.IsUserAuthorizationEnabled)
                return true;

            var context = WitsmlOperationContext.Current;
            var endpoint = endpointType.GetDescription();
            var username = context.User;
            var request = context.Request;

            _log.Debug($"Verifying authorization for user: {username}; endpoint: {endpoint}; function: {request.Function}");

            return UserAuthorizationAdapters
                .Select(x => x.IsAuthorized(username, request, endpointType))
                .FirstOrDefault();
        }
    }
}