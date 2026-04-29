using Microsoft.EntityFrameworkCore;

namespace Gestionale_Drian.Data;

public class AppDbContext : DbContext
{
    // Queste sono le "Tabelle" che vedrai nel database
    public DbSet<Articolo> Articoli { get; set; }
    public DbSet<Movimento> Movimenti { get; set; }
    public DbSet<Associato> Associati { get; set; }
    public DbSet<Progetto> Progetti { get; set; }
    public DbSet<EventoCalendario> EventiCalendario { get; set; }
    public DbSet<TransazioneFinanziaria> TransazioniFinanziarie { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Creiamo il percorso in Documenti/Gestionale_Drian/
        string documenti = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string cartellaApp = Path.Combine(documenti, "Gestionale_Drian");

        // Assicuriamoci che la cartella esista
        if (!Directory.Exists(cartellaApp))
        {
            Directory.CreateDirectory(cartellaApp);
        }

        // Il file si chiamerà DrianDatabase.db
        string dbPath = Path.Combine(cartellaApp, "DrianDatabase.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}