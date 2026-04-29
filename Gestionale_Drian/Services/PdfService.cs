using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Gestionale_Drian.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// ALIAS PER EVITARE CONFLITTI MAUI
using QColors = QuestPDF.Helpers.Colors;
using QContainer = QuestPDF.Infrastructure.IContainer;
using QUnit = QuestPDF.Infrastructure.Unit;

namespace Gestionale_Drian.Services;

public class PdfService
{
    public string GeneraPreventivoPdf(Progetto progetto)
    {
        // Licenza obbligatoria
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        string cartellaDocumenti = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string cartellaPreventivi = Path.Combine(cartellaDocumenti, "Preventivi");
        Directory.CreateDirectory(cartellaPreventivi);

        string titoloPulito = string.IsNullOrWhiteSpace(progetto.Titolo) ? "SenzaTitolo" : string.Join("_", progetto.Titolo.Split(Path.GetInvalidFileNameChars()));
        string filePath = Path.Combine(cartellaPreventivi, $"Preventivo_{progetto.Id}_{titoloPulito}.pdf");

        var richiedente = progetto.Membri?.FirstOrDefault();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, QUnit.Centimetre);
                page.PageColor(QColors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                // NON USIAMO L'HEADER PER IL LOGO GIGANTE, USIAMO IL CONTENUTO
                page.Content().Column(col =>
                {
                    // 1. INTESTAZIONE (Stampata solo a inizio documento)
                    col.Item().PaddingBottom(1, QUnit.Centimetre).Element(c => ComposeHeader(c, progetto));

                    // 2. CORPO DEL PREVENTIVO
                    col.Item().Element(c => ComposeContent(c, progetto, richiedente));
                });

                // Il piè di pagina leggero va bene su ogni foglio
                page.Footer().Element(ComposeFooter);
            });
        })
        .GeneratePdf(filePath);

        return filePath;
    }

    private static void ComposeHeader(QContainer container, Progetto progetto)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                // Titolo generico o Nome del Mittente caricato dal DB
                col.Item().Text(progetto.MittenteNome.ToUpper()).FontSize(20).ExtraBold().FontColor(QColors.Blue.Medium);

                // Campi compilabili dall'utente nel form
                col.Item().PaddingTop(2).Text(progetto.MittenteIndirizzo);
                col.Item().Text($"C.F./P.IVA: {progetto.MittenteCodiceFiscale}");
                col.Item().Text(progetto.MittenteContatti);
            });

            row.ConstantItem(150).AlignRight().Column(col =>
            {
                col.Item().Text("PREVENTIVO").FontSize(18).Bold().FontColor(QColors.Grey.Darken3);
                col.Item().PaddingTop(5).Text($"Data: {DateTime.Now:dd/MM/yyyy}");
                col.Item().Text($"Documento nr: {progetto.Id:D6}");
            });
        });
    }

    private static void ComposeContent(QContainer container, Progetto progetto, Associato? richiedente)
    {
        container.Column(col =>
        {
            col.Item().AlignCenter().PaddingBottom(15).Text($"Preventivo {progetto.Titolo ?? ""}").FontSize(14).Bold();

            // TABELLA DETTAGLIATA
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                });

                table.Header(header =>
                {
                    var headerStyle = TextStyle.Default.SemiBold();
                    header.Cell().BorderBottom(1).PaddingBottom(5).Text("Descrizione").Style(headerStyle);
                    header.Cell().BorderBottom(1).PaddingBottom(5).AlignCenter().Text("Qtà").Style(headerStyle);
                    header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Unitario").Style(headerStyle);
                    header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Importo").Style(headerStyle);
                });

                var voci = progetto.VociPreventivo ?? new List<VocePreventivo>();

                foreach (var voce in voci)
                {
                    table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten3).PaddingVertical(5).Text(voce.Descrizione ?? "Voce non specificata");
                    table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten3).PaddingVertical(5).AlignCenter().Text(voce.Quantita.ToString());
                    table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten3).PaddingVertical(5).AlignRight().Text($"{voce.PrezzoUnitario:C}");
                    table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten3).PaddingVertical(5).AlignRight().Text($"{voce.Importo:C}");
                }
            });

            // TOTALI E SCONTI
            col.Item().PaddingTop(10).AlignRight().Column(c =>
            {
                c.Item().Text(t =>
                {
                    t.Span("Totale parziale: ").FontSize(10);
                    t.Span($"{progetto.TotaleVoci:C}").SemiBold();
                });

                if (progetto.ScontoPercentuale > 0)
                {
                    decimal valoreSconto = progetto.TotaleVoci * progetto.ScontoPercentuale / 100;
                    c.Item().Text(t =>
                    {
                        t.Span($"Sconto associati ({progetto.ScontoPercentuale}%): ").FontSize(10).FontColor(QColors.Red.Medium);
                        t.Span($"- {valoreSconto:C}").FontColor(QColors.Red.Medium);
                    });
                }

                c.Item().PaddingTop(5).Text(t =>
                {
                    t.Span("TOTALE (eventuali sconti inclusi): ").Bold().FontSize(12);
                    t.Span($"{progetto.TotaleScontato:C}").Bold().FontSize(14).FontColor(QColors.Blue.Darken2);
                });
            });

            // CONDIZIONI LEGALI
            col.Item().PaddingTop(20).Column(c =>
            {
                c.Item().PaddingBottom(2).Text(t => {
                    t.Span("Metodo e modalità di pagamento:  ").Bold();
                    t.Span(progetto.MetodoPagamento ?? "Anticipo (Contanti/PayPal/Bonifico) del 50% e 50% alla consegna.");
                });

                c.Item().PaddingBottom(2).Text(t => {
                    t.Span("Tipologia di consegna:  ").Bold();
                    t.Span(progetto.TipologiaConsegna ?? "Consegna a mano.");
                });

                c.Item().PaddingBottom(2).Text(t => {
                    t.Span("Tempo stimato per la realizzazione (gg. lavorativi):  ").Bold();
                    t.Span(progetto.GiorniConsegna ?? "20 gg.");
                });

                c.Item().PaddingTop(2).Text("La validità di questo preventivo è di 30 giorni. Decorso tale termine sarà necessaria l’emissione di un ulteriore preventivo per la possibile variazione dei prezzi dei materiali necessari.");
            });

            // DATI RICHIEDENTE CON SALVAVITA PAGINAZIONE (EnsureSpace)
            col.Item().EnsureSpace(80).PaddingTop(20).Border(1).BorderColor(QColors.Grey.Lighten2).Padding(10).Column(c =>
            {
                c.Item().Text("RICHIEDENTE").Bold().FontSize(11).FontColor(QColors.Grey.Darken2);
                if (richiedente != null)
                {
                    c.Item().Text($"{richiedente.Nome} {richiedente.Cognome}").Bold();
                    c.Item().Text(richiedente.Indirizzo ?? "-");

                    string contatti = "";
                    if (!string.IsNullOrWhiteSpace(richiedente.Cellulare)) contatti += $"(+39) {richiedente.Cellulare}";
                    if (!string.IsNullOrWhiteSpace(richiedente.Email)) contatti += (contatti.Length > 0 ? ", email: " : "email: ") + richiedente.Email;

                    if (!string.IsNullOrWhiteSpace(contatti)) c.Item().Text(contatti);
                }
                else
                {
                    c.Item().Text("Dati richiedente non selezionati").Italic();
                }
            });

            // FIRME CON SALVAVITA PAGINAZIONE
            col.Item().EnsureSpace(80).PaddingTop(40).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Timbro e Firma Mittente").FontSize(9); 
                    c.Item().PaddingTop(20).LineHorizontal(0.5f);
                });
                row.ConstantItem(60);
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Firma per Accettazione Cliente").FontSize(9); 
                    c.Item().PaddingTop(20).LineHorizontal(0.5f);
                });
            });
        });
    }

    private static void ComposeFooter(QContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span("Pagina ");
            x.CurrentPageNumber();
            x.Span(" di ");
            x.TotalPages();
        });
    }
}