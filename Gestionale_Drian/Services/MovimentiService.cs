using Microsoft.EntityFrameworkCore;
using Gestionale_Drian.Data;

namespace Gestionale_Drian.Services;

public class MovimentiService
{
    // READ: Ottieni tutti i movimenti (includendo il nome dell'articolo)
    public async Task<List<Movimento>> GetMovimentiAsync()
    {
        using var db = new AppDbContext();
        // Li ordiniamo per data, dal più recente al più vecchio
        return await db.Movimenti
                       .Include(m => m.Articolo)
                       .OrderByDescending(m => m.Data)
                       .ToListAsync();
    }

    // CREATE: Aggiungi movimento E aggiorna la quantità in magazzino
    public async Task AggiungiMovimentoAsync(Movimento movimento)
    {
        using var db = new AppDbContext();

        // 1. Troviamo l'articolo nel DB
        var articolo = await db.Articoli.FindAsync(movimento.ArticoloId);
        if (articolo == null) return;

        // 2. Facciamo la matematica in base all'operazione
        switch (movimento.TipoOperazione)
        {
            case "Carico":
                articolo.QuantitaInMagazzino += movimento.Quantita;
                break;

            case "Scarico":
                articolo.QuantitaInMagazzino -= movimento.Quantita;
                break;

            case "MessaInUso":
                // Tolgo dal magazzino "sano" e lo metto in quello "aperto"
                articolo.QuantitaInMagazzino -= movimento.Quantita;
                articolo.QuantitaInUso += movimento.Quantita;
                break;

            case "ScaricoUso":
                // Consumo definitivamente la roba già aperta
                articolo.QuantitaInUso -= movimento.Quantita;
                break;
        }

        // 3. Salviamo movimento e aggiorniamo le quantità
        db.Movimenti.Add(movimento);
        db.Articoli.Update(articolo);

        await db.SaveChangesAsync();
    }

    // NOTA: Di solito in un gestionale i movimenti non si eliminano e non si modificano 
    // per non falsare lo storico (si fa un movimento opposto per correggere un errore). 
    // Ma per flessibilità, ecco il codice del DELETE.
    public async Task EliminaMovimentoAsync(int id)
    {
        using var db = new AppDbContext();
        var movimento = await db.Movimenti.FindAsync(id);
        if (movimento != null)
        {
            db.Movimenti.Remove(movimento);
            await db.SaveChangesAsync();
        }
    }

}