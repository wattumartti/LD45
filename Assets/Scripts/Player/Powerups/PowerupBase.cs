using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase
{
    public enum PowerupType
    {
        NONE = 0,
        DOUBLE_JUMP = 1,
        DASH = 2
    }

    public PowerupBase()
    {

    }

    public PowerupType type = PowerupType.NONE;

    protected double timeUsed = 0;
    protected int cooldownSeconds = 0;
    protected bool isEnabled = true;

    public virtual bool ActivatePowerup()
    {
        if (!isEnabled)
        {
            return false;
        }

        double timeStampNow = ConvertToUnixTimestamp(DateTime.Now);

        if (timeUsed + cooldownSeconds > timeStampNow)
        {
            return false;
        }

        timeUsed = timeStampNow;

        return true;
    }

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp);
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    public virtual void SetPowerupEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    public virtual void OnPurchased()
    {

    }
}
