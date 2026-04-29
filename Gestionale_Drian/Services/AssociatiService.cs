using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data;

namespace Gestionale_Drian.Services;

public class AssociatiService
{
    // READ
    public async Task<List<Associato>> GetAssociatiAsync()
    {
        using var db = new AppDbContext();
        return await db.Associati.ToListAsync();
    }

    // CREATE
    public async Task AggiungiAssociatoAsync(Associato nuovoAssociato)
    {
        using var db = new AppDbContext();
        db.Associati.Add(nuovoAssociato);
        await db.SaveChangesAsync();
    }

    // UPDATE
    public async Task AggiornaAssociatoAsync(Associato socioModificato)
    {
        using var db = new AppDbContext();
        var socioEsistente = await db.Associati.FindAsync(socioModificato.Id);

        if (socioEsistente != null)
        {
            // Copia i nuovi dati sopra quelli vecchi
            db.Entry(socioEsistente).CurrentValues.SetValues(socioModificato);
            await db.SaveChangesAsync();
        }
    }

    // DELETE
    public async Task EliminaAssociatoAsync(int id)
    {
        using var db = new AppDbContext();
        var associato = await db.Associati.FindAsync(id);
        if (associato != null)
        {
            db.Associati.Remove(associato);
            await db.SaveChangesAsync();
        }
    }
}