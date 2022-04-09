using System;
namespace ClashDaemon.ClashLog;
public interface IClashLogService
{
    public void HandleLog(string? origin);
}
