/*
 * SPDX-FileCopyrightText: 2022 Daniele Corsini <daniele.corsini@corsinvest.it>
 * SPDX-FileCopyrightText: Copyright Corsinvest Srl
 * SPDX-License-Identifier: GPL-3.0-only
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Common;
using Corsinvest.ProxmoxVE.Api.Shared.Utils;
using Newtonsoft.Json;

namespace Corsinvest.ProxmoxVE.Api.Shared.Models.Vm
{
    /// <summary>
    /// rrd data structure
    /// </summary>
    public class VmRrdData : IDisk, INetIO, IDiskIO, ICpu, IMemory
    {
        /// <summary>
        /// Time unix time
        /// </summary>
        /// <value></value>
        [JsonProperty("time")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatUnixTime)]
        public long Time { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public DateTime TimeDate => DateTimeOffset.FromUnixTimeSeconds(Time).DateTime;

        /// <summary>
        /// Disk usage
        /// </summary>
        /// <value></value>
        [JsonProperty("disk")]
        [Display(Name = "Disk usage")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long DiskUsage { get; set; }

        /// <summary>
        /// Disk size
        /// </summary>
        /// <value></value>
        [JsonProperty("maxdisk")]
        [Display(Name = "Disk size")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long DiskSize { get; set; }

        /// <summary>
        /// Disk usage percentage
        /// </summary>
        /// <value></value>
        [Display(Name = "Disk usage %")]
        [DisplayFormat(DataFormatString = "{0:P1}")]
        public double DiskUsagePercentage { get; set; }

        /// <summary>
        /// Net in
        /// </summary>
        /// <value></value>
        [JsonProperty("netin")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long NetIn { get; set; }

        /// <summary>
        /// Net out
        /// </summary>
        /// <value></value>
        [JsonProperty("netout")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long NetOut { get; set; }

        /// <summary>
        /// Disk read
        /// </summary>
        /// <value></value>
        [JsonProperty("diskread")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long DiskRead { get; set; }

        /// <summary>
        /// Disk write
        /// </summary>
        /// <value></value>
        [JsonProperty("diskwrite")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long DiskWrite { get; set; }

        /// <summary>
        /// Cpu usage
        /// </summary>
        /// <value></value>
        [Display(Name = "CPU Usage %")]
        [DisplayFormat(DataFormatString = "{0:P1}")]
        [JsonProperty("cpu")]
        public double CpuUsagePercentage { get; set; }

        /// <summary>
        /// Cpu size
        /// </summary>
        /// <value></value>
        [JsonProperty("maxcpu")]
        public long CpuSize { get; set; }

        /// <summary>
        /// Cpu info
        /// </summary>
        /// <value></value>
        [Display(Name = "Cpu")]
        public string CpuInfo { get; set; }

        /// <summary>
        /// Memory usage
        /// </summary>
        /// <value></value>
        [JsonProperty("mem")]
        [Display(Name = "Memory")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long MemoryUsage { get; set; }

        /// <summary>
        ///Memory size
        /// </summary>
        /// <value></value>
        [JsonProperty("maxmem")]
        [Display(Name = "Max Memory")]
        [DisplayFormat(DataFormatString = FormatHelper.FormatBytes)]
        public long MemorySize { get; set; }

        /// <summary>
        /// Memory info
        /// </summary>
        /// <value></value>
        [Display(Name = "Memory")]
        public string MemoryInfo { get; set; }

        /// <summary>
        /// Memory usage percentage
        /// </summary>
        /// <value></value>
        [Display(Name = "Memory Usage %")]
        [DisplayFormat(DataFormatString = "{0:P1}")]
        public double MemoryUsagePercentage { get; set; }

        [OnDeserialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            ((ICpu)this).ImproveData();
            ((IMemory)this).ImproveData();
            ((IDisk)this).ImproveData();
        }
    }
}