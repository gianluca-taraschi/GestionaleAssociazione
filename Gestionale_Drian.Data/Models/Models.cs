using Microsoft.EntityFrameworkCore.Migrations;

public class Articolo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }

    // DUE CONTATORI SEPARATI
    public int QuantitaInMagazzino { get; set; }
    public int QuantitaInUso { get; set; } = 0; // <--- NUOVO CAMPO

    public decimal Prezzo { get; set; }
    public string Categoria { get; set; } = "Generico";

    public List<Movimento> Movimenti { get; set; } = new();
}

public class Movimento
{
    public int Id { get; set; }
    public int ArticoloId { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;

    // NUOVO CAMPO PER IDENTIFICARE L'AZIONE ESATTA
    // Può essere: "Carico", "Scarico", "MessaInUso", "ScaricoUso"
    public string TipoOperazione { get; set; } = "Scarico";

    public int Quantita { get; set; } // Ora sarà sempre un numero positivo (il segno lo decide il TipoOperazione)
    public string Causale { get; set; } = string.Empty;

    public Articolo? Articolo { get; set; }
}

public class Associato
{
    public int Id { get; set; }
    public string NumeroTessera { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;

    public DateTime? DataDiNascita { get; set; }
    public string? Indirizzo { get; set; }
    public string? Cellulare { get; set; }

    public string? Email { get; set; }

    public List<Progetto> Progetti { get; set; } = new();
}

public class Progetto
{
    public int Id { get; set; }
    public string Titolo { get; set; } = string.Empty;
    public string? Descrizione { get; set; }

    // --- NUOVI CAMPI PER INTESTAZIONE NEUTRALE ---
    public string MittenteNome { get; set; } = "Nome Azienda/Associazione";
    public string MittenteIndirizzo { get; set; } = "Via Roma 1, Città (Prov)";
    public string MittenteCodiceFiscale { get; set; } = "P.IVA o C.F.";
    public string MittenteContatti { get; set; } = "Email: info@esempio.it | Tel: 000 000000";
    // ----------------------------------------------

    public decimal PrezzoTotale { get; set; }
    public string? PreventivoPdfPath { get; set; }

    public string MetodoPagamento { get; set; } = "Bonifico Bancario / Contanti"; // reso più generico
    public string TipologiaConsegna { get; set; } = "Ritiro in sede.";
    public string GiorniConsegna { get; set; } = "30 gg.";

    public List<VocePreventivo> VociPreventivo { get; set; } = new();

    public decimal TotaleVoci => VociPreventivo.Sum(v => v.Importo);
    public decimal ScontoPercentuale { get; set; } = 0;

    public decimal TotaleScontato => TotaleVoci - (TotaleVoci * ScontoPercentuale / 100);

    public bool IsCompletato { get; set; }
    public bool IsConsegnato { get; set; }
    public DateTime? DataConsegna { get; set; }
    public DateTime DataCreazione { get; set; } = DateTime.Now.Date;
    public DateTime DataScadenza { get; set; } = DateTime.Now.Date.AddDays(30);

    public List<Associato> Membri { get; set; } = new();
}

public class VocePreventivo
{
    public int Id { get; set; }
    public int ProgettoId { get; set; }

    public string Descrizione { get; set; } = string.Empty;
    public int Quantita { get; set; } = 1;
    public decimal PrezzoUnitario { get; set; }

    // Calcolo automatico dell'importo per questa riga
    public decimal Importo => Quantita * PrezzoUnitario;
}

public class EventoCalendario
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.Now.Date;
    public string Titolo { get; set; } = string.Empty;
    // Salveremo le classi dei colori di Bootstrap (es. "primary", "danger", "purple")
    public string ColoreTag { get; set; } = "primary";
}

public class TransazioneFinanziaria
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.Now.Date;
    public string Descrizione { get; set; } = string.Empty;
    public decimal Importo { get; set; }
    public bool IsEntrata { get; set; } = true; // True = Entrata (Donazione), False = Uscita (Spesa extra)
}