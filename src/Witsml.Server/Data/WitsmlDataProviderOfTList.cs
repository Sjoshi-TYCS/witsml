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
using System.Linq;
using Energistics.DataAccess;
using Energistics.Datatypes;
using PDS.Framework;
using PDS.Witsml.Server.Configuration;

namespace PDS.Witsml.Server.Data
{
    /// <summary>
    /// Data provider that encapsulates CRUD service calls for WITSML query.
    /// </summary>
    /// <typeparam name="TList">Type of the object list.</typeparam>
    /// <typeparam name="TObject">Type of the object.</typeparam>
    /// <seealso cref="PDS.Witsml.Server.Data.IWitsmlDataProvider" />
    public abstract class WitsmlDataProvider<TList, TObject> : WitsmlDataProvider<TObject>, IWitsmlDataProvider
        where TList : IEnergisticsCollection
        where TObject : class, IDataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WitsmlDataProvider{TList, TObject}" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="dataAdapter">The data adapter.</param>
        protected WitsmlDataProvider(IContainer container, IWitsmlDataAdapter<TObject> dataAdapter) : base(container, dataAdapter)
        {
        }

        /// <summary>
        /// Gets object(s) from store.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>Queried objects.</returns>
        public virtual WitsmlResult<IEnergisticsCollection> GetFromStore(RequestContext context)
        {
            var parser = new WitsmlQueryParser(context);
            Logger.DebugFormat("Getting {0}", typeof(TObject).Name);

            var childParsers = parser.ForkElements().ToArray();

            // Validate each query template separately
            foreach (var childParser in childParsers)
                Validate(Functions.GetFromStore, childParser, null);

            Logger.DebugFormat("Validated {0} for Query", typeof(TObject).Name);

            // Execute each query separately
            var queries = childParsers.SelectMany(DataAdapter.Query);

            return new WitsmlResult<IEnergisticsCollection>(
                ErrorCodes.Success,
                CreateCollection(queries));
        }

        /// <summary>
        /// Adds an object to the data store.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>
        /// A WITSML result that includes a positive value indicates a success or a negative value indicates an error.
        /// </returns>
        public virtual WitsmlResult AddToStore(RequestContext context)
        {
            var parser = new WitsmlQueryParser(context);
            return Add(parser);
        }

        /// <summary>
        /// Updates an object in the data store.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>
        /// A WITSML result that includes a positive value indicates a success or a negative value indicates an error.
        /// </returns>
        public virtual WitsmlResult UpdateInStore(RequestContext context)
        {
            var parser = new WitsmlQueryParser(context);
            return Update(parser);
        }

        /// <summary>
        /// Deletes or partially update object from store.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>
        /// A WITSML result that includes a positive value indicates a success or a negative value indicates an error.
        /// </returns>
        public virtual WitsmlResult DeleteFromStore(RequestContext context)
        {
            var parser = new WitsmlQueryParser(context);
            return Delete(parser);
        }

        /// <summary>
        /// Gets the URI for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        protected override EtpUri GetUri(TObject dataObject)
        {
            return dataObject.GetUri();
        }

        /// <summary>
        /// Parses the specified XML string.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The data object instance.</returns>
        protected override TObject Parse(string xml)
        {
            var list = WitsmlParser.Parse<TList>(xml);
            return list.Items.Cast<TObject>().FirstOrDefault();
        }

        /// <summary>
        /// Creates an <see cref="TList"/> instance containing the specified data objects.
        /// </summary>
        /// <param name="dataObjects">The data objects.</param>
        /// <returns>The <see cref="TList"/> instance.</returns>
        protected abstract TList CreateCollection(IEnumerable<TObject> dataObjects);
    }
}
