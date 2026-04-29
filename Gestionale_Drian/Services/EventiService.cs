using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data;

namespace Gestionale_Drian.Services;

public class EventiService
{
    public async Task<List<EventoCalendario>> GetEventiMeseAsync(int anno, int mese)
    {
        using var db = new AppDbContext();
        return await db.EventiCalendario
                       .Where(e => e.Data.Year == anno && e.Data.Month == mese)
                       .ToListAsync();
    }

    public async Task AggiungiEventoAsync(EventoCalendario evento)
    {
        using var db = new AppDbContext();
        db.EventiCalendario.Add(evento);
        await db.SaveChangesAsync();
    }

    public async Task EliminaEventoAsync(int id)
    {
        using var db = new AppDbContext();
        var evt = await db.EventiCalendario.FindAsync(id);
        if (evt != null)
        {
            db.EventiCalendario.Remove(evt);
            await db.SaveChangesAsync();
        }
    }
}