using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data;

namespace Gestionale_Drian.Services;

public class ProgettiService
{
    // READ
    public async Task<List<Progetto>> GetProgettiAsync()
    {
        using var db = new AppDbContext();
        // Include() dice al database: "Oltre ai dati del progetto, portami anche i dati degli associati collegati"
        return await db.Progetti.Include(p => p.Membri)
                                .Include(p => p.VociPreventivo)
                                .ToListAsync();
    }

    // CREATE (Aggiornato per gestire il Team in sicurezza)
    public async Task AggiungiProgettoAsync(Progetto nuovoProgetto, List<int> idMembriSelezionati)
    {
        using var db = new AppDbContext();

        // 1. Troviamo nel database gli associati veri e propri tramite i loro ID
        var membriDaAssegnare = await db.Associati
                                        .Where(a => idMembriSelezionati.Contains(a.Id))
                                        .ToListAsync();

        // 2. Li leghiamo al progetto
        nuovoProgetto.Membri = membriDaAssegnare;

        // 3. Salviamo tutto
        db.Progetti.Add(nuovoProgetto);
        await db.SaveChangesAsync();
    }

    // UPDATE
    public async Task AggiornaProgettoAsync(Progetto progettoModificato)
    {
        using var db = new AppDbContext();

        // 1. Peschiamo il progetto "pulito" e originale dal database
        var progettoEsistente = await db.Progetti.FindAsync(progettoModificato.Id);

        if (progettoEsistente != null)
        {
            // 2. Questa funzione magica prende SOLO i valori testuali, le date e i numeri 
            // dal tuo progetto modificato e li copia in quello esistente. 
            // Ignora completamente le liste (come i Membri o le VociPreventivo), 
            // evitando così che il database cerchi di duplicarle!
            db.Entry(progettoEsistente).CurrentValues.SetValues(progettoModificato);

            // 3. Salviamo in totale sicurezza
            await db.SaveChangesAsync();
        }
    }

    // DELETE
    public async Task EliminaProgettoAsync(int id)
    {
        using var db = new AppDbContext();
        var progetto = await db.Progetti.FindAsync(id);
        if (progetto != null)
        {
            db.Progetti.Remove(progetto);
            await db.SaveChangesAsync();
        }
    }
}