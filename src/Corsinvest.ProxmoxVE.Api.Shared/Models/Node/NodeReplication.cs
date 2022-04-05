﻿/*
 * SPDX-FileCopyrightText: 2022 Daniele Corsini <daniele.corsini@corsinvest.it>
 * SPDX-FileCopyrightText: Copyright Corsinvest Srl
 * SPDX-License-Identifier: GPL-3.0-only
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Corsinvest.ProxmoxVE.Api.Shared.Models.Node
{
    /// <summary>
    /// Node replication
    /// </summary>
    public class NodeReplication
    {
        /// <summary>
        /// Source
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Vm Type
        /// </summary>
        [JsonProperty("vmtype")]
        public string Vmtype { get; set; }

        /// <summary>
        /// Fail Count
        /// </summary>
        [JsonProperty("fail_count")]
        public int FailCount { get; set; }

        /// <summary>
        /// Last Sync
        /// </summary>
        [JsonProperty("last_sync")]
        public int LastSync { get; set; }

        /// <summary>
        /// Job Num
        /// </summary>
        [JsonProperty("jobnum")]
        public string JobNum { get; set; }

        /// <summary>
        /// Next Sync
        /// </summary>
        [JsonProperty("next_sync")]
        public int NextSync { get; set; }

        /// <summary>
        /// Guest
        /// </summary>
        [JsonProperty("guest")]
        public string Guest { get; set; }

        /// <summary>
        /// Schedule
        /// </summary>
        [JsonProperty("schedule")]
        public string Schedule { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        [JsonProperty("duration")]
        public double Duration { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Last Try
        /// </summary>
        [JsonProperty("last_try")]
        public int LastTry { get; set; }

        /// <summary>
        /// Target
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }

         /// <summary>
        /// Extension Data
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; set; }
   }
}