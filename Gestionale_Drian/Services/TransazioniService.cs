using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data; 

namespace Gestionale_Drian.Services;

public class TransazioniService
{
    public async Task<List<TransazioneFinanziaria>> GetTransazioniAsync()
    {
        using var db = new AppDbContext();
        return await db.TransazioniFinanziarie.ToListAsync();
    }

    public async Task AggiungiTransazioneAsync(TransazioneFinanziaria transazione)
    {
        using var db = new AppDbContext();
        db.TransazioniFinanziarie.Add(transazione);
        await db.SaveChangesAsync();
    }

    public async Task EliminaTransazioneAsync(int id)
    {
        using var db = new AppDbContext();
        var t = await db.TransazioniFinanziarie.FindAsync(id);
        if (t != null)
        {
            db.TransazioniFinanziarie.Remove(t);
            await db.SaveChangesAsync();
        }
    }
}