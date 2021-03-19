using Rhetos.Utilities;
using System;

namespace AuthenticationDemo
{
    public static class AuthenticationReporting
    {
        public static UserInfoReport[] GetReport(IUserInfo userInfo)
        {
            return new UserInfoReport[]
            {
                new UserInfoReport { Key = "Report", Value = GetValueOrException(() => userInfo.Report()) },
                new UserInfoReport { Key = "Type", Value = userInfo.GetType().ToString() },
                new UserInfoReport { Key = "IsUserRecognized", Value = GetValueOrException(() => userInfo.IsUserRecognized) },
                new UserInfoReport { Key = "UserName", Value = GetValueOrException(() => userInfo.UserName) },
                new UserInfoReport { Key = "Workstation", Value = GetValueOrException(() => userInfo.Workstation) },
            };
        }

        private static string GetValueOrException(Func<object> getter)
        {
            try
            {
                return getter()?.ToString();
            }
            catch (Exception e)
            {
                return $"{e.GetType().Name}: {e.Message}";
            }
        }
    }
}
