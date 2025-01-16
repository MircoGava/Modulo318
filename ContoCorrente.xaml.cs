using ProgettoCasinoGava.Models;
using System.Text.Json;

namespace ProgettoCasinoGava;

public partial class ContoCorrente : ContentPage
{


    string folderPath = FileSystem.AppDataDirectory;
    Conto contobancario;
    public ContoCorrente()
	{
		InitializeComponent();
        LeggiJason();
        Saldo.conto = contobancario.Banca;
        Saldo.puntata = contobancario.Puntata;
        EnConto.Text = Saldo.conto.ToString();
        EnPuntataAttuale.Text = contobancario.Puntata.ToString();
    }
    //la classe OnAppearing e UpdateUI fatte con chat gpt per fare in modo che ogni volta che apro la pagina i dati vengono aggiornati
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LeggiJason();
        
    }
    //La parte del folderPath ho dovuto usare internet perchè non riuscivo a farlo funzionare
    private void LeggiJason()
    {
        Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, "conto.json");

        if (File.Exists(filePath))
        {
            var jsonString = File.ReadAllText(filePath);
            contobancario = JsonSerializer.Deserialize<Conto>(jsonString, options);
        }
        else
        {
            contobancario = new Conto(50000, 250);
            SalvaConto();
        }
        UpdateUI(contobancario);

    }

    //ogni colta che lo apri il conto in banca è aggiornato
    private void UpdateUI(Conto dati)
    {
        EnConto.Text = Saldo.conto.ToString();
        EnPuntataAttuale.Text = Saldo.puntata.ToString();
    }

    private readonly JsonSerializerOptions options = new()
    {

        PropertyNameCaseInsensitive = true,

        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        WriteIndented = true,

        IncludeFields = true

    };

    //nella parte finale ho chiesto aiuto a chat gpt perchè non mi salvava i dati, il codice praticamente non è cambiato ma ora funziona
    //Codice per fare una nuova puntata
    private void ButPuntata(object sender, EventArgs e)
    {
        //Controllo che sia stato inserito un vero numero nella puntata
        if (int.TryParse(EnPuntata.Text, out int nuovaPuntata))
        {   
            //Controllo per vedere che hai abbastanza soldi per fare quella puntata
            if (nuovaPuntata > Saldo.conto || nuovaPuntata <= 10)
            {
                DisplayAlert("Errore", "Hai puntato più di quello che hai nel conto o troppo poco (min 10)", "Ho capito");
            }

            //Salva la nuova puntata e fa un refresh della pagina per vedere i dati aggiornati
            else {
                Saldo.puntata = nuovaPuntata;
                SalvaConto();
                RefreshPageAsync();
            }
        }
        else
            {
                DisplayAlert("Errore", "Inserisca un valore valido.", "Ho capito");
            }
        
    }

    //Salva i dati inseriti nel json/banca
    private void SalvaConto()
    {
        contobancario.Banca = Saldo.conto;
        contobancario.Puntata = Saldo.puntata;
        EnConto.Text = Saldo.conto.ToString();
        EnPuntataAttuale.Text = Saldo.puntata.ToString();

        string jsonString = JsonSerializer.Serialize(contobancario, options);
        File.WriteAllText(Path.Combine(folderPath, "conto.json"), jsonString);
    }

    private async Task RefreshPageAsync()
    {
        await Navigation.PushAsync(new ContoCorrente());      
    }

    //Per aggiungere i soldi al conto in banca 
    private void ButPrestito(object sender, EventArgs e)
    {
        //controllo che sia stato richiesto un numero
        if (int.TryParse(EnPrestito.Text, out int nuovoPrestito))
        {
            //Controllo per fare in modo che non si può chiedere prestiti anche se hai già soldi per giocare
            if(Saldo.conto > 5000)
            {
                DisplayAlert("Errore", "Hai ancora abbastanza soldi per giocare", "Ho capito");
            }
            else { 
            //COntrollo per fare in modo che non si possano richiede troppi soldi o troppi pochi
            if (nuovoPrestito > 250000 || nuovoPrestito < 5000)
            {
                DisplayAlert("Errore", "Hai chiesto troppo soldi o troppi pochi (max 25000, min 5000), la banca ha rifiutato", "Ho capito");
            }
            else
            {
                Saldo.conto += nuovoPrestito;
                SalvaConto();
                RefreshPageAsync();
            }
            }
        }
        else
        {
            DisplayAlert("Errore", "Inserisca un numero vero", "Ho capito");
        }
        
    }
}