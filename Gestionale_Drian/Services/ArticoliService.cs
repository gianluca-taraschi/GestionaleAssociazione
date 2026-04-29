using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data; // Assicurati di avere la using corretta al tuo DB / Modelli

namespace Gestionale_Drian.Services;

public class ArticoliService
{
    // 1. READ (Ottieni tutti gli articoli)
    public async Task<List<Articolo>> GetArticoliAsync()
    {
        using var db = new AppDbContext();
        return await db.Articoli.ToListAsync();
    }

    // 2. CREATE (Aggiungi un nuovo articolo)
    public async Task AggiungiArticoloAsync(Articolo nuovoArticolo)
    {
        using var db = new AppDbContext();
        db.Articoli.Add(nuovoArticolo);
        await db.SaveChangesAsync();
    }

    // 3. UPDATE (Modifica un articolo esistente - CORRETTO E BLINDATO)
    public async Task AggiornaArticoloAsync(Articolo articoloModificato)
    {
        using var db = new AppDbContext();
        var articoloEsistente = await db.Articoli.FindAsync(articoloModificato.Id);

        if (articoloEsistente != null)
        {
            // Sovrascriviamo solo i campi per evitare conflitti con altre tabelle
            db.Entry(articoloEsistente).CurrentValues.SetValues(articoloModificato);
            await db.SaveChangesAsync();
        }
    }

    // 4. DELETE (Elimina un articolo)
    public async Task EliminaArticoloAsync(int id)
    {
        using var db = new AppDbContext();
        var articolo = await db.Articoli.FindAsync(id);
        if (articolo != null)
        {
            db.Articoli.Remove(articolo);
            await db.SaveChangesAsync();
        }
    }
}