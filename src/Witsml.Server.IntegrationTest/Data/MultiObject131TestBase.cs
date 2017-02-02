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

using System.Linq;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.Witsml.Server.Configuration;

namespace PDS.Witsml.Server.Data
{
    public abstract class MultiObject131TestBase : IntegrationTestBase
    {
        public Well Well { get; set; }
        public Wellbore Wellbore { get; set; }
        public Log Log { get; set; }
        public Trajectory Trajectory { get; set; }
        public DevKit131Aspect DevKit { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit131Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version131.Value)
                .ToArray();

            Well = new Well
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Well"),
                TimeZone = DevKit.TimeZone
            };
            Wellbore = new Wellbore
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Wellbore"),
                UidWell = Well.Uid,
                NameWell = Well.Name,
                MDCurrent = new MeasuredDepthCoord(0, MeasuredDepthUom.ft)
            };
            Log = new Log
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Log"),
                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name
            };
            Trajectory = new Trajectory
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Trajectory"),
                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name
            };

            BeforeEachTest();
            OnTestSetUp();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AfterEachTest();
            OnTestCleanUp();
            DevKit.Container.Dispose();
            DevKit = null;
        }

        private void BeforeEachTest()
        {
            //log
            Log.IndexType = LogIndexType.measureddepth;
            Log.IndexCurve = new IndexCurve
            {
                ColumnIndex = 0,
                Value = "MD"
            };

            //trajectory
            Trajectory.ServiceCompany = "Service Company T";
        }

        private void AfterEachTest()
        {
            //log
            WitsmlSettings.DepthRangeSize = DevKitAspect.DefaultDepthChunkRange;
            WitsmlSettings.TimeRangeSize = DevKitAspect.DefaultTimeChunkRange;
            WitsmlSettings.LogMaxDataPointsGet = DevKitAspect.DefaultLogMaxDataPointsGet;
            WitsmlSettings.LogMaxDataPointsUpdate = DevKitAspect.DefaultLogMaxDataPointsAdd;
            WitsmlSettings.LogMaxDataPointsAdd = DevKitAspect.DefaultLogMaxDataPointsUpdate;
            WitsmlSettings.LogMaxDataPointsDelete = DevKitAspect.DefaultLogMaxDataPointsDelete;
            WitsmlSettings.LogMaxDataNodesGet = DevKitAspect.DefaultLogMaxDataNodesGet;
            WitsmlSettings.LogMaxDataNodesAdd = DevKitAspect.DefaultLogMaxDataNodesAdd;
            WitsmlSettings.LogMaxDataNodesUpdate = DevKitAspect.DefaultLogMaxDataNodesUpdate;
            WitsmlSettings.LogMaxDataNodesDelete = DevKitAspect.DefaultLogMaxDataNodesDelete;
            WitsmlSettings.LogGrowingTimeoutPeriod = DevKitAspect.DefaultLogGrowingTimeoutPeriod;
            WitsmlOperationContext.Current = null;

            //trajectory
            WitsmlSettings.MaxStationCount = DevKitAspect.DefaultMaxStationCount;
            WitsmlSettings.TrajectoryMaxDataNodesGet = DevKitAspect.DefaultTrajectoryMaxDataNodesGet;
            WitsmlSettings.TrajectoryMaxDataNodesAdd = DevKitAspect.DefaultTrajectoryMaxDataNodesAdd;
            WitsmlSettings.TrajectoryMaxDataNodesUpdate = DevKitAspect.DefaultTrajectoryMaxDataNodesUpdate;
            WitsmlSettings.TrajectoryMaxDataNodesDelete = DevKitAspect.DefaultTrajectoryMaxDataNodesDelete;
            WitsmlSettings.TrajectoryGrowingTimeoutPeriod = DevKitAspect.DefaultTrajectoryGrowingTimeoutPeriod;
        }

        protected virtual void OnTestSetUp() { }

        protected virtual void OnTestCleanUp() { }

        protected virtual void AddParents()
        {
            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.AddAndAssert<WellboreList, Wellbore>(Wellbore);
        }
    }
}
