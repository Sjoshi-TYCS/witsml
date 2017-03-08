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

using log4net;

namespace PDS.Witsml.Server.Data.Trajectories
{
    /// <summary>
    /// Provides common helper methods for Trajectory data objects.
    /// </summary>
    public static class TrajectoryExtensions
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TrajectoryExtensions));

        /// <summary>
        /// Determines whether trajectory stations should be included in the query response.
        /// </summary>
        /// <param name="parser">The query parser.</param>
        /// <returns><c>true</c> if trajectory stations should be included; otherwise, <c>false</c>.</returns>
        public static bool IncludeTrajectoryStations(this WitsmlQueryParser parser)
        {
            var returnElements = parser.ReturnElements();

            _log.DebugFormat("Checking if trajectory stations should be included. Return Elements: {0};", returnElements);

            return OptionsIn.ReturnElements.All.Equals(returnElements) ||
                   OptionsIn.ReturnElements.DataOnly.Equals(returnElements) ||
                   OptionsIn.ReturnElements.StationLocationOnly.Equals(returnElements) ||
                   (OptionsIn.ReturnElements.Requested.Equals(returnElements) && parser.Contains("trajectoryStation"));
        }
    }
}
