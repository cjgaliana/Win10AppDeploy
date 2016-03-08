using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinAppDeploy.GUI.Extensions;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Utils;

namespace WinAppDeploy.GUI.Services
{
    public interface IDeployService
    {
        Task<bool> IsInstalledAsync();

        Task<IList<DeployTargetDevice>> GetDevicesAsync();

        Task InstallAppAsync(string filePath, DeployTargetDevice device);

        Task UnistallAppAsync(string filePath, DeployTargetDevice device);

        Task UpdateAppAsync(string filePath, DeployTargetDevice device);

        Task<IList<WinApp>> GetInstalledAppsAsync(DeployTargetDevice device);
    }

    public class Win10DeployService : IDeployService
    {
        public async Task<bool> IsInstalledAsync()
        {
            try
            {
                var output = await this.RunWinAppDeployCmdAsync();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IList<DeployTargetDevice>> GetDevicesAsync()
        {
            var devices = new List<DeployTargetDevice>();

            var result = await this.RunWinAppDeployCmdAsync("devices");
            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices
            var lines = Regex.Split(sanizited, "\\n", RegexOptions.CultureInvariant);

            foreach (var line in lines)
            {
                if (line.IsDeviceInfo())
                {
                    var ip = Regex.Match(line, RegexHelper.IpPattern).Value;
                    var guid = Regex.Match(line, RegexHelper.GuidPattern).Value;
                    var name = Regex.Match(line, "(?<=^((\\S)*\\s){2}).*").Value.Trim(); // Capture the device name, but not the IP, the GUID or the final \r

                    var device = new DeployTargetDevice
                    {
                        Guid = guid,
                        Ip = ip,
                        Name = name
                    };
                    devices.Add(device);
                }
            }

            return devices;
        }

        public async Task InstallAppAsync(string filePath, DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "install",
                $"-file {filePath}",
                $"-ip {device.Ip}",
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
                $"-package {device.Ip}"
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
              $"-ip {device.Ip}"
              );

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices
        }

        public async Task<IList<WinApp>> GetInstalledAppsAsync(DeployTargetDevice device)
        {
            var result = await this.RunApp(-1,
                "list",
                $"-ip {device.Ip}");

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter().CleanListingHeader().CleanListingFooter().Trim();
            // TODO: Parse apps
            var apps = new List<WinApp>();

            var lines = Regex.Split(sanizited, "\\n", RegexOptions.CultureInvariant);

            foreach (var line in lines)
            {
                var name = line.Trim();
                var app = new WinApp { PackageName = name };
                apps.Add(app);
            }

            return apps;
        }

        private Task<string> RunWinAppDeployCmdAsync(params string[] arguments)
        {
            return this.RunApp(3, arguments);
        }

        private Task<string> RunApp(int timeoutInSeconds, params string[] arguments)
        {
            return Task.Run(() =>
            {
                var defaultPath = @"C:\Program Files (x86)\Windows Kits\10\bin\x86";
                var execName = "WinAppDeployCmd.exe";

                var cmdArguments = string.Join(" ", arguments);
                if (timeoutInSeconds > 0)
                {
                    cmdArguments += " " + timeoutInSeconds;
                }

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