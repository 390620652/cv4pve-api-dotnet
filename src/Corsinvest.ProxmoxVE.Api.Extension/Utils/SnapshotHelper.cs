﻿/*
 * SPDX-FileCopyrightText: 2019 Daniele Corsini <daniele.corsini@corsinvest.it>
 * SPDX-FileCopyrightText: Copyright Corsinvest Srl
 * SPDX-License-Identifier: GPL-3.0-only
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Vm;

namespace Corsinvest.ProxmoxVE.Api.Extension.Utils
{
    /// <summary>
    /// Snaopshot helper
    /// </summary>
    public static class SnapshotHelper
    {
        /// <summary>
        /// Get Snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<IEnumerable<VmSnapshot>> GetSnapshots(PveClient client, string node, VmType vmType, long vmId)
            => (vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot.Get(),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot.Get(),
                _ => throw new InvalidEnumArgumentException(),
            }).OrderBy(a => a.Date);

        /// <summary>
        /// Create snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="state"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<Result> CreateSnapshot(PveClient client,
                                                        string node,
                                                        VmType vmType,
                                                        long vmId,
                                                        string name,
                                                        string description,
                                                        bool state,
                                                        long timeout)
        {
            var result = vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot.Snapshot(name, description, state),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot.Snapshot(name, description),
                _ => throw new InvalidEnumArgumentException(),
            };
            await client.WaitForTaskToFinish(result, timeout: timeout);

            return result;
        }

        /// <summary>
        /// Remove snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<Result> RemoveSnapshot(PveClient client,
                                                        string node,
                                                        VmType vmType,
                                                        long vmId,
                                                        string name,
                                                        long timeout,
                                                        bool? force = null)
        {
            var result = vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot[name].Delsnapshot(force),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot[name].Delsnapshot(force),
                _ => throw new InvalidEnumArgumentException(),
            };
            await client.WaitForTaskToFinish(result, timeout: timeout);

            return result;
        }

        /// <summary>
        /// Get config snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<Result> GetConfigSnapshot(PveClient client,
                                                           string node,
                                                           VmType vmType,
                                                           long vmId,
                                                           string name)
            => vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot[name].Config.GetSnapshotConfig(),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot[name].Config.GetSnapshotConfig(),
                _ => throw new InvalidEnumArgumentException(),
            };

        /// <summary>
        /// Update snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<Result> UpdateSnapshot(PveClient client,
                                                        string node,
                                                        VmType vmType,
                                                        long vmId,
                                                        string name,
                                                        string description)
            => vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot[name].Config.UpdateSnapshotConfig(description),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot[name].Config.UpdateSnapshotConfig(description),
                _ => throw new InvalidEnumArgumentException(),
            };

        /// <summary>
        /// Roolback snapshot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="node"></param>
        /// <param name="vmType"></param>
        /// <param name="vmId"></param>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static async Task<Result> RollbackSnapshot(PveClient client,
                                                          string node,
                                                          VmType vmType,
                                                          long vmId,
                                                          string name,
                                                          long timeout)
        {
            var result = vmType switch
            {
                VmType.Qemu => await client.Nodes[node].Qemu[vmId].Snapshot[name].Rollback.Rollback(),
                VmType.Lxc => await client.Nodes[node].Lxc[vmId].Snapshot[name].Rollback.Rollback(),
                _ => throw new InvalidEnumArgumentException(),
            };

            await client.WaitForTaskToFinish(result, timeout: timeout);
            return result;
        }
    }
}