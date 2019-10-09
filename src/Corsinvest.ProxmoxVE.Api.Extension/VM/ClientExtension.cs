﻿/*
 * This file is part of the cv4pve-api-dotnet https://github.com/Corsinvest/cv4pve-api-dotnet,
 * Copyright (C) 2016 Corsinvest Srl
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using Corsinvest.ProxmoxVE.Api.Extension.Helpers;

namespace Corsinvest.ProxmoxVE.Api.Extension.VM
{
    /// <summary>
    /// Client extension for vm
    /// </summary>
    public static class ClientExtension
    {
        /// <summary>
        /// Get all vms info.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static VMInfo[] GetVMs(this PveClient client)
        {
            var vms = new List<VMInfo>();
            foreach (var vm in client.Cluster.Resources.GetRest("vm").Response.data)
            {
                vms.Add(new VMInfo(client, vm));
            }
            return vms.OrderBy(a => a.Node).ThenBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get vm info from id or name.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        public static VMInfo GetVM(this PveClient client, string idOrName)
        {
            var vm = GetVMs(client).Where(a => CheckIdOrName(a, idOrName)).FirstOrDefault();
            if (vm == null) { throw new ArgumentException($"VM/CT {idOrName} not found!"); }
            return vm;
        }

        private static bool CheckIdOrName(VMInfo vm, string id)
        {
            if (StringHelper.IsNumeric(id))
            {
                //numeric
                return vm.Id == id;
            }
            else if (id.StartsWith("%") && id.EndsWith("%"))
            {
                //contain
                return vm.Name.Contains(id.Replace("%", ""));
            }
            else if (id.StartsWith("%"))
            {
                //startwith
                return vm.Name.StartsWith(id.Replace("%", ""));
            }
            else if (id.EndsWith("%"))
            {
                //endwith
                return vm.Name.EndsWith(id.Replace("%", ""));
            }
            else
            {
                return vm.Name == id;
            }
        }

        /// <summary>
        /// Get vms from jolly.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jolly">all for all vm, 
        /// <para>all-nodename all vm in host,</para>
        /// <para>vmid id vm</para>
        /// <para>start with '-' exclude vm</para>
        /// <para>comma reparated</para></param>
        /// <returns></returns>
        public static VMInfo[] GetVMs(this PveClient client, string jolly)
        {
            var vms = new List<VMInfo>();
            var allVms = GetVMs(client);

            foreach (var id in jolly.Split(','))
            {
                if (id == "all")
                {
                    //all nodes
                    vms.AddRange(allVms);
                }
                else if (id.StartsWith("all-"))
                {
                    //all in specific node
                    vms.AddRange(allVms.Where(a => a.Node == id.Substring(4)));
                }
                else
                {
                    //specific id
                    var vm = allVms.Where(a => CheckIdOrName(a, id)).FirstOrDefault();
                    if (vm != null) { vms.Add(vm); }
                }
            }

            //exclude data 
            foreach (var id in jolly.Split(',').Where(a => a.StartsWith("-")).Select(a => a.Substring(1)))
            {
                var vm = allVms.Where(a => CheckIdOrName(a, id)).FirstOrDefault();
                if (vm != null) { vms.Remove(vm); }
            }

            return vms.Distinct().ToArray();
        }
    }
}