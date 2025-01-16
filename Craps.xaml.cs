using System.Text.Json;
using ProgettoCasinoGava.Models;

namespace ProgettoCasinoGava;

public partial class Craps : ContentPage
{

    Conto contobancario;
    Random TiraDado = new Random();
    string folderPath = FileSystem.AppDataDirectory;

    public Craps()
	{
		InitializeComponent();
        LeggiJason();
        Saldo.conto = contobancario.Banca;
        Saldo.puntata = contobancario.Puntata;

    }

    //Fa in modo che se aggiorno la scommessa cambia anche in questa pagina
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LeggiJason();

    }

    //prossimi codici fanno in modo che il conto in banca sia collegato
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
    private void UpdateUI(Conto dati)
    {
        Saldo.conto = contobancario.Banca;
        Saldo.puntata = contobancario.Puntata;
    }


    private readonly JsonSerializerOptions options = new()
    {

        PropertyNameCaseInsensitive = true,

        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        WriteIndented = true,

        IncludeFields = true

    };

    //Quando premi il pulsante tiri il dado
    private void DadoTirato(object sender, EventArgs e)
    {
        string ScommessaValida = EnScommessa.Text;

        //controllo che hai ancora abbastanza soldi
        if (Saldo.conto - Saldo.puntata < 0)
        {
            DisplayAlert("Attento!", "Non vogliamo che vai in debito con la banca abbassi la puntata!", "Ho capito");
        }
        else
        {
            //controllo per vedere che hai inserito un numero
            if (int.TryParse(ScommessaValida, out int scommessa))
            {
                //controllo per vedere che il numero inserito sia tra quelli consentiti
                if (scommessa >= 1 && scommessa <= 6)
                {
                    int NumeroUscito = TiraDado.Next(1, 7);
                    EnNumeroUscito.Text = NumeroUscito.ToString();
                    if (scommessa == NumeroUscito)
                    {
                        Vittoria();
                    }
                    else
                    {
                        Perso();
                    }
                }
                else
                {
                    DisplayAlert("Errore", "Il valore da lei messo non era tra 1 e 6", "OK");
                }
            }
            else
            {
                // Gestisci l'input non valido
                DisplayAlert("Errore", "Metta un numero superiore a 0", "OK");
            }
        }
    }

    // Se vinci prima moltiplica la tua puntata, poi aggiunge i soldi al tuo conto e lo salva, per poi fare la scritta e cambiare il colore del background
    private void Vittoria()
    {
        int risultato = Saldo.puntata * 2;    
        Saldo.conto += risultato;
        SalvaConto(); 
        EnRisultato.Text = "Hai vinto: "; 
        EnSoldi.Text = risultato.ToString();
        FrameRis.BackgroundColor = Colors.Green;
    }
    
    //Se perdi prima toglie i soldi poi fa la scritta
    private void Perso()
    {
        int risultato = Saldo.puntata;
        Saldo.conto -= risultato;
        SalvaConto();
        EnRisultato.Text = "Hai perso: ";
        EnSoldi.Text = Saldo.puntata.ToString();
        FrameRis.BackgroundColor = Colors.Red;

    }
    //Salva il conto aggiungendoti i soldi / togliendoteli 
    private void SalvaConto()
    {
        contobancario.Banca = Saldo.conto;
        string jsonString = JsonSerializer.Serialize(contobancario, options);
        File.WriteAllText(Path.Combine(folderPath, "conto.json"), jsonString);
    }

}