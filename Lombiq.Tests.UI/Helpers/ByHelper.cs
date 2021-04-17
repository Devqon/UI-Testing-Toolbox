﻿using Atata;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;

namespace Lombiq.Tests.UI.Helpers
{
    /// <summary>
    /// Provides helper functions for generating <see cref="By"/> selectors.
    /// </summary>
    public static class ByHelper
    {
        /// <summary>
        /// Returns an XPath selector for an email in the list whose headers contain the text <paramref name="text"/>.
        /// </summary>
        public static By SmtpInboxRow(string text) =>
            By
                .XPath($"//tr[contains(@class,'el-table__row')]//div[contains(@class,'cell')][contains(text(), {JsonConvert.SerializeObject(text)})]")
                .Within(TimeSpan.FromMinutes(2));

        public static By Text(string innerText, string element = "*") =>
            By.XPath($"//{element}[. = {JsonConvert.SerializeObject(innerText)}]");

        public static By TextContains(string innerText, string element = "*") =>
            By.XPath($"//{element}[contains(., {JsonConvert.SerializeObject(innerText)})]");
    }
}
