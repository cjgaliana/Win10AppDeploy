using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinAppDeploy.GUI.Extensions;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Utils;

namespace WinAppDeploy.GUI.Services
{
    public class Win10DeployService : IDeployService
    {
        public async Task<bool> IsSDKInstalledAsync()
        {
            try
            {
                var output = await this.RunWinAppDeployCmdAsync();
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public async Task<IList<DeployTargetDevice>> GetDevicesAsync()
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "devices", //cmd
                "5" // Timeout
                );

            // Sanitize string
            var sanizited = result.CleanHeader().CleanFooter();

            // Parse lines
            var lines = Regex.Split(sanizited, "\\n", RegexOptions.CultureInvariant);
            var devices = lines
                .Where(x => x.IsDeviceInfo())
                .Select(item => new DeployTargetDevice
                {
                    Guid = Regex.Match(item, RegexHelper.GuidPattern).Value,
                    IP = Regex.Match(item, RegexHelper.IpPattern).Value,
                    Name = Regex.Match(item, "(?<=^((\\S)*\\s){2}).*").Value.Trim()
                })
                .ToList();

            return devices;
        }

        public async Task<IList<WinApp>> GetInstalledAppsAsync(DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "list",
                $"-ip {device.IP}");

            var sanizited = result.CleanHeader().CleanFooter().CleanListingHeader().CleanListingFooter().Trim();

            var lines = Regex.Split(sanizited, "\\n", RegexOptions.CultureInvariant);

            var apps = lines
                .Select(name => new WinApp
                {
                    PackageName = name.Trim(),
                    Architecture = this.GetAppArchitecture(name)
                })
                .ToList();
            return apps;
        }

        public async Task InstallAppAsync(string filePath, DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "install",
                $"-file {filePath}",
                $"-ip {device.IP}",
                $"-guid {device.Guid}" //,
                //$"-pin {pin}"
                );

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices
        }

        public async Task UnistallAppAsync(string filePath, DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "uninstall",
                $"-file {filePath}",
                $"-package {device.IP}"
                );

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices
        }

        public async Task UpdateAppAsync(string filePath, DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "update",
                $"-file {filePath}",
                $"-ip {device.IP}"
                );

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices
        }

        private string GetAppArchitecture(string packageName)
        {
            if (packageName.ToLowerInvariant().Contains("_arm_".ToLowerInvariant()))
            {
                return "ARM";
            }

            if (packageName.ToLowerInvariant().Contains("_x64_".ToLowerInvariant()))
            {
                return "x64";
            }

            if (packageName.ToLowerInvariant().Contains("_neutral_".ToLowerInvariant()))
            {
                return "Any CPU";
            }

            return "x86";
        }

        private Task<string> RunWinAppDeployCmdAsync(params string[] arguments)
        {
            return Task.Run(() =>
            {
                var defaultPath = @"C:\Program Files (x86)\Windows Kits\10\bin\x86";
                var execName = "WinAppDeployCmd.exe";

                var cmdArguments = string.Join(" ", arguments);

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(defaultPath, execName),
                        Arguments = cmdArguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                var output = proc.StandardOutput.ReadToEnd();
                return output;
            });
        }
    }
}