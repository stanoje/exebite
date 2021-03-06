﻿using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace Exebite.GoogleSheetAPI.GoogleSSFactory
{
    public class GoogleSheetServiceFactory : IGoogleSheetServiceFactory
    {
        public SheetsService GetService()
        {
            var jsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\client_secret.json");
            UserCredential credential;
            using (var stream = new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { "https://www.googleapis.com/auth/spreadsheets" },
                    "user",
                    CancellationToken.None).Result;
            }

            SheetsService sSservice = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Exebite"
            });
            return sSservice;
        }
    }
}