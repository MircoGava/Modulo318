using System.Text.Json;
using ProgettoCasinoGava.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ProgettoCasinoGava;

public partial class Blackjack : ContentPage
{

    Conto contobancario;
    Random nuovacarta = new Random();
    string folderPath = FileSystem.AppDataDirectory;
    int carte12 = 0;

    public Blackjack()
	{
		InitializeComponent();
        LeggiJason();
        Saldo.conto = contobancario.Banca;
        Saldo.puntata = contobancario.Puntata;
    }

    //Per far in modo che aggiorni la puntata
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LeggiJason();

    }

    //Codice per far funzionare il json della banca
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

    //Quando premi il pulsante inizia, controlla prima che hai i soldi poi inizia
    private void buInizia(object sender, EventArgs e)
    {
        butInizia.IsEnabled = false;
        EnRisultato.Text = "";
        FrameRis.BackgroundColor = Colors.White;
        EnCarteBanco.Text = "";

        if (Saldo.conto - Saldo.puntata < 0)
        {
            DisplayAlert("Attento!", "Non vogliamo che vai in debito con la banca abbassi la puntata!", "Ho capito");
        }
        else
        {
            //Ho fatto in modo che non ti desse le due carte uguali per fare dopo in modo che non puoi avere tre carte uguali
            int carta1 = nuovacarta.Next(1, 11);
            int carta2 = nuovacarta.Next(1, 11);
            if (carta1 == carta2) 
            {
                carta2 = nuovacarta.Next(1, 11);
            }
            else
            {
                //Il codice sotto fa in modo che l'asso valga 11 o 1 in base a se sei sopra o sotto al 21
                if (carta1 == 1 || carta2 == 1)
                {
                    if (carta1 + carta2 + 10 <= 21)
                    {
                        carte12 = (carta1 + carta2) + 10;
                        int carteasso = carte12 - 11;
                        EnCarte.Text = "A," + carteasso.ToString();
                    }
                    else
                    {
                        carte12 = carta1 + carta2;
                        int carteasso = carte12 - 1;
                        EnCarte.Text = "A," + carteasso.ToString();
                    }
                }
                else 
                {
                    carte12 = carta1 + carta2;
                    EnCarte.Text = carta1.ToString() + "," + carta2.ToString() ;
                }

                //Controllo per vedere se hai già vinto, in caso controrio attiva i pulsanti per pescare o rimanere
                if(carte12 == 21)
                {
                    vittoria();
                }
                if (carte12 > 21)
                {
                    perso();
                }
                if(carte12 < 21)
                {
                    butPesca.IsEnabled = true;
                    butResta.IsEnabled = true;
                    butPesca.BackgroundColor = Color.FromArgb("#1f402d");
                    butResta.BackgroundColor = Color.FromArgb("#1f402d");
                }
            }
        }
        }

    
    //Se scegli di pescare la terza, e ultima carta
    private void buPesca(object sender, EventArgs e)
    {

        int carta3 = nuovacarta.Next(1, 11);    
        //Controllo per asso
        if (carta3 == 1)
        {
            if (carte12 + carta3 + 10 <= 21)
            {
                carte12 += carta3 + 10;
                EnCarte.Text += ",A";
            }
            else
            {
                carte12 += carta3;

                EnCarte.Text += ",A";
            }
        }
        else
        {
            carte12 += carta3;
            EnCarte.Text += "," + carta3.ToString() ;
        }
        //Controllo vittorîa
        //Serve a fare in modo che se hai 21 nel secondo round c'è una piccola possibilità di fare pareggio con il tavolo
        if (carte12 == 21)
        {
            int cartebanco = nuovacarta.Next(14, 22);
            if(cartebanco == 21)
            {
                EnRisultato.Text = "Pareggio, nessuno vince";
                SalvaConto();
            }
            else
            {
                vittoria();
            }
            
        }
        if (carte12 > 21)
        {
            perso();
        }
        if (carte12 < 21)
        {
            fine();
        }    

    }

    //Se scegli di restare con le tue carte fa il controllo finale
    private void buResta(object sender, EventArgs e)
    {
        fine();
    }

    //Controllo finale per determinare vittoria o sconfitta
    private void fine()
    {
        int cartebanco = nuovacarta.Next(14, 22);
        EnCarteBanco.Text = cartebanco.ToString();
        if (carte12 > cartebanco)
        {
            vittoria();
        }
        if (carte12 == cartebanco)
        {
            EnRisultato.Text = "Pareggio, nessuno vince";
            SalvaConto();
        }
        if (carte12 < cartebanco)
        {
            perso();
        }
    }

    //Se vinci
    private void vittoria ()
    {
        EnRisultato.Text = "Hai vinto:";
        int risultato = Saldo.puntata * 2;
        Saldo.conto += risultato;
        EnRisultato.Text += risultato.ToString();
        FrameRis.BackgroundColor = Colors.Green;

        SalvaConto();
    }

    //Se perdi
    private void perso()
    {
        EnRisultato.Text = "Hai Perso:";
        int risultato = Saldo.puntata;
        Saldo.conto -= risultato;
        EnRisultato.Text += risultato.ToString();
        FrameRis.BackgroundColor = Colors.Red;

        SalvaConto();
    }

    //Salva dati della banca e prepara il gioco al prossimo round
    private void SalvaConto()
    {
        butPesca.IsEnabled = false;
        butResta.IsEnabled = false;
        butInizia.IsEnabled = true;
        butInizia.BackgroundColor = Color.FromArgb("#1f402d");
        contobancario.Banca = Saldo.conto;
        string jsonString = JsonSerializer.Serialize(contobancario, options);
        File.WriteAllText(Path.Combine(folderPath, "conto.json"), jsonString);
    }
}