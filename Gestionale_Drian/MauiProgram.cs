using Microsoft.Extensions.Logging;
using Gestionale_Drian.Data;
using System.IO;
using System;
using System.Runtime.Versioning;

namespace Gestionale_Drian
{
    public static class MauiProgram
    {
        [SupportedOSPlatform("windows")]
        public static MauiApp CreateMauiApp()
        {
            string documenti = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string logFile = Path.Combine(documenti, "Gestionale_Drian_CrashLog.txt");

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                File.AppendAllText(logFile, $"\n[{DateTime.Now}] ERRORE GLOBALE NON GESTITO: {args.ExceptionObject}\n");
            };

            try
            {
                File.AppendAllText(logFile, $"\n[{DateTime.Now}] --- AVVIO GESTIONALE DRIAN ---");
                File.AppendAllText(logFile, $"\n[{DateTime.Now}] Configurazione app in corso...");

                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                var builder = MauiApp.CreateBuilder();
                builder
                    .UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    });

                builder.Services.AddMauiBlazorWebView();

                builder.Services.AddSingleton<Services.ArticoliService>();
                builder.Services.AddSingleton<Services.AssociatiService>();
                builder.Services.AddSingleton<Services.ProgettiService>();
                builder.Services.AddSingleton<Services.MovimentiService>();
                builder.Services.AddSingleton<Services.EventiService>();
                builder.Services.AddSingleton<Services.TransazioniService>();
                builder.Services.AddSingleton<Services.PdfService>();
                builder.Services.AddBlazorBootstrap();

                File.AppendAllText(logFile, $"\n[{DateTime.Now}] Servizi caricati. Tento la creazione del Database...");

                using (var db = new Gestionale_Drian.Data.AppDbContext())
                {
                    db.Database.EnsureCreated();
                }

                File.AppendAllText(logFile, $"\n[{DateTime.Now}] Database pronto! Avvio UI...\n");

                return builder.Build();
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, $"\n[{DateTime.Now}] CRASH FATALE DURANTE L'AVVIO: {ex.Message}\nStackTrace: {ex.StackTrace}\n");
                throw;
            }
        }
    }
}