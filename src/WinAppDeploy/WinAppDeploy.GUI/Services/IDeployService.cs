using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WinAppDeploy.GUI.Extensions;
using WinAppDeploy.GUI.Models;

namespace WinAppDeploy.GUI.Services
{
    public interface IDeployService
    {
        Task<bool> IsInstalledAsync();

        Task<IList<DeployTargetDevice>> GetDevicesAsync();

        Task InstallAppAsync(string filePath, DeployTargetDevice device);

        Task UnistallAppAsync(string filePath, DeployTargetDevice device);

        Task UpdateAppAsync(string filePath, DeployTargetDevice device);

        Task<IList<string>> GetInstalledAppsAsync(DeployTargetDevice device);
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
            var result = await this.RunWinAppDeployCmdAsync("devices 3");
            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices

            return new List<DeployTargetDevice>();
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

        public async Task<IList<string>> GetInstalledAppsAsync(DeployTargetDevice device)
        {
            var result = await this.RunWinAppDeployCmdAsync(
                "list",
                $"-ip {device.Ip}");

            // TODO: Process output
            var sanizited = result.CleanHeader().CleanFooter();
            // TODO: Parse devices

            return new List<string>();
        }

        private Task<string> RunWinAppDeployCmdAsync(params string[] arguments)
        {
            return Task.Run(() =>
            {
                var defaultPath = @"C:\Program Files (x86)\Windows Kits\10\bin\x86";
                var execName = "WinAppDeployCmd.exe";

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(defaultPath, execName),
                        Arguments = string.Join(" ", arguments),
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