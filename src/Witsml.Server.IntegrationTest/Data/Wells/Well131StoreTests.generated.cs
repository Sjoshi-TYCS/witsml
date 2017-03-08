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

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------

using Energistics.DataAccess;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Wells
{
    [TestClass]
    public partial class Well131StoreTests : Well131TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            BeforeEachTest();
        }

        protected override void OnTestCleanUp()
        {
            AfterEachTest();
        }

        [TestMethod]
        public void Well131DataAdapter_GetFromStore_Can_Get_Well()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.GetAndAssert<WellList, Well>(Well);
       }

        [TestMethod]
        public void Well131DataAdapter_AddToStore_Can_Add_Well()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
        }

        [TestMethod]
        public void Well131DataAdapter_UpdateInStore_Can_Update_Well()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.UpdateAndAssert<WellList, Well>(Well);
            DevKit.GetAndAssert<WellList, Well>(Well);
        }

        [TestMethod]
        public void Well131DataAdapter_DeleteFromStore_Can_Delete_Well()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.DeleteAndAssert<WellList, Well>(Well);
            DevKit.GetAndAssert<WellList, Well>(Well, isNotNull: false);
        }

        [TestMethod]
        public void Well131WitsmlStore_GetFromStore_Can_Transform_Well()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);

            // Re-initialize all capServer providers
            DevKit.Store.CapServerProviders = null;
            DevKit.Container.BuildUp(DevKit.Store);

            string typeIn, queryIn;
            var query = DevKit.List(DevKit.CreateQuery(Well));
            DevKit.SetupParameters<WellList, Well>(query, ObjectTypes.Well, out typeIn, out queryIn);

            var options = OptionsIn.Join(OptionsIn.ReturnElements.All, OptionsIn.DataVersion.Version141);
            var request = new WMLS_GetFromStoreRequest(typeIn, queryIn, options, null);
            var response = DevKit.Store.WMLS_GetFromStore(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.XMLout));
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var result = WitsmlParser.Parse(response.XMLout);
            var version = ObjectTypes.GetVersion(result.Root);
            Assert.AreEqual(OptionsIn.DataVersion.Version141.Value, version);
        }
    }
}
