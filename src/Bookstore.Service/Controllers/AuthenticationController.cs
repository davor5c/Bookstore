using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Rhetos.Host.AspNet;
using Rhetos.Processing;
using Rhetos.Processing.DefaultCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookstore.Service.Controllers
{
    [Route("Authentication/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IProcessingEngine processingEngine;

        public AuthenticationController(IRhetosComponent<IProcessingEngine> rhetosProcessingEngine)
        {
            processingEngine = rhetosProcessingEngine.Value;
        }

        [HttpGet]
        public async Task Login(string username)
        {
            // Overly simplified authentication without a password, for demo purpose only.
            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties() { IsPersistent = true });
        }

        [HttpGet]
        public async Task Logout(string username)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties() { IsPersistent = true });
        }

        [HttpGet]
        public string DemoProcessingEngine()
        {
            var processingResult = processingEngine.Execute(new[] {
                new ReadCommandInfo
                {
                    DataSource = "AuthenticationDemo.UserInfoReport",
                    ReadRecords = true
                }});

            if (!processingResult.Success)
                return $"UserMessage:{processingResult.UserMessage}{Environment.NewLine}SystemMessage:{processingResult.SystemMessage}";

            var records = (IEnumerable<AuthenticationDemo.UserInfoReport>)((ReadCommandResult)processingResult.CommandResults.Single().Data.Value).Records;

            return "UserInfo:" + Environment.NewLine
                + string.Join(Environment.NewLine, records.Select(record => $"{record.Key}: {record.Value}"));
        }
    }
}
