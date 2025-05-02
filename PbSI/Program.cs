using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PbSI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Menu menu = new Menu();
            //await menu.InitialiserAsync();

            ReseauMetro truc = new ReseauMetro("MetroParis.xlsx");
            Coloration<StationMetro> coco = new Coloration<StationMetro> (truc.Graphe) ;
            coco.WelshPowell();
            coco.AfficherColoration();

        }

    }
}
