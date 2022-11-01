using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ChessApp.Controllers
{
    public class ChessController : Controller
    {
        static List<Models.ChessMatch> _MatchList = new();
        static int _MatchCounter = 1;
        public IActionResult Index()
        {
            return View();
        }
        //also used to request ongoing match by giving an id on the request
        public IActionResult StartMatch(int _Id = -1)
        {
            List<Models.ChessMatch> MatchList = _MatchList;
            int MatchCounter = _MatchCounter;
            Trace.WriteLine("------ DEBUG ------");
            Trace.WriteLine("IActionResult StartMatch");
            if (_Id < 0)
            {
                Trace.WriteLine("creating new match");
                Trace.WriteLine(MatchList.Count);
                MatchList.Add(new(MatchCounter));
                Trace.WriteLine(MatchList.Count);
                Trace.WriteLine("new match created");
                _Id = MatchCounter;
                MatchCounter++;
                _MatchList = MatchList;
                _MatchCounter = MatchCounter;
            }
            foreach (Models.ChessMatch match in MatchList)
            {
                if (_Id == match.Id)
                {
                    Trace.WriteLine(JsonConvert.SerializeObject(match));
                    Trace.WriteLine("Json away");
                    Trace.WriteLine("------  END  ------");
                    return Json(JsonConvert.SerializeObject(match));
                }
            }
            return Content("Error");
        }
        [HttpPost]
        public IActionResult Move(string _move)
        {
            List<Models.ChessMatch> MatchList = _MatchList;
            int MatchCounter = _MatchCounter;
            Trace.WriteLine("------ DEBUG ------");
            Trace.WriteLine("IActionResult Move");
            Trace.WriteLine($"Matches in MatchList: {MatchList.Count}");
            Trace.WriteLine($"json received: {_move}");
            Models.Move Movement;
            try
            { 
                Movement = JsonConvert.DeserializeObject<Models.Move>(_move);
            }
            catch
            {
                Trace.WriteLine("json error");
                return Content("Error: json parce error");
            }
            Trace.WriteLine("json - ok, validating");
            Trace.WriteLine("comparing ids");
            Trace.WriteLine(MatchList.Count);
            foreach (Models.ChessMatch Match in MatchList)
            {
                Trace.WriteLine($"{Match.Id} | {Movement.Id}");
                if(Match.Id == Movement.Id)
                {
                    if (Match.Move(Movement)) { Trace.WriteLine("movement made"); }
                    else { Trace.WriteLine("movement not made"); }

                    _MatchList = MatchList;
                    _MatchCounter = MatchCounter;
                    return Content("Success");
                }
            }
            _MatchList = MatchList;
            _MatchCounter = MatchCounter;
            return Content("Error: id not found");
            }
    }
}
