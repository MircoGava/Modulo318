using ProgettoCasinoGava.Models;

namespace ProgettoCasinoGava;

public partial class Info : ContentPage
{
    //Lista delle regole 
	List<Regole> regolelist = new List<Regole>();
	Regole Casino = new Regole("Casino","Vietato barare, in caso contrario le verranno sottratti tutti i soldi vinti e un ban a vita. \n Come c'era scritto sulle linee guida nella prima pagina che ha accettato venendo qui non non prendiamo resposabilità su nessuna scelta fatta da i nostri clienti, e non siamo legalmente denuciabili",0,"casino.png");
    Regole Craps = new Regole("Craps", "Scegli un numero tra 1 e 6, tiri il dado e se esce il numero che hai scelto vinci. \n Nel gioco sotto scommessa si inserisce il valore che si pensa che uscirà.", 2, "Craps.png");
    Regole Blackjack = new Regole("Blackjack", "Hai 2 carte, obbiettivo è arrivare a 21 o arrivarci più vicino del banco. \n Dopo aver visto le carte hai l'opzione di pescarne una terza o no. \n Gli assi valgono 11 o 1", 2, "Blackjack.png");
    public Info()
	{
		InitializeComponent();
        regolelist.Add(Casino);
        regolelist.Add(Craps);
        regolelist.Add(Blackjack);

        pickRegole.ItemsSource = regolelist;

    }
    //programmazione picker
    private void pickRegole_SelectIndexChange(object sender, EventArgs e)
    {
        Regole regolescelta = (Regole)pickRegole.SelectedItem;

        LaNome.Text = regolescelta.Nome;
        LaCome.Text = regolescelta.Come;
        LaMulti.Text = regolescelta.Multiplicatore.ToString();
        immagini.Source = regolescelta.Image;
    }
}