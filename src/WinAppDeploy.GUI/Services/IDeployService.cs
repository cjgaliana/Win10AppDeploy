using System.Collections.Generic;
using System.Threading.Tasks;
using WinAppDeploy.GUI.Models;

namespace WinAppDeploy.GUI.Services
{
    public interface IDeployService
    {
        CmdResult LastCmdRunResukt { get; }

        Task<bool> IsSDKInstalledAsync();

        Task<IList<DeployTargetDevice>> GetDevicesAsync();

        Task InstallAppAsync(string filePath, DeployTargetDevice device);

        Task UnistallAppAsync(WinApp winApp, DeployTargetDevice device);

        Task UpdateAppAsync(string filePath, DeployTargetDevice device);

        Task<IList<WinApp>> GetInstalledAppsAsync(DeployTargetDevice device);
    }
}