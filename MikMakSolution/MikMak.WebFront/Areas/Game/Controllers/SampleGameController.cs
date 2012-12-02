using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MikMak.WebFront.Areas.Game.Controllers
{
    public class SampleGameController : ApiController
    {
        // GET api/samplegame
        public string Get()
        {
            return DateTime.Now.ToString("hh:mm:ss:fffffff");
        }

        public string Connect(string login, string password)
        {
            return login + password;
        }

        // GET api/samplegame/5
        public Map Get(int id)
        {
            return new Map();
        }

        // POST api/samplegame
        public Map Post([FromBody]int id)
        {
            if (id == 2)
                return new Map();

            return null;
        }

        // PUT api/samplegame/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/samplegame/5
        public void Delete(int id)
        {
        }

        public class Coord
        {
            public int X;
            public int Y;

            public Coord(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public class Map
        {
            public List<Coord> Coords;
            public Map()
            {
                Coords = new List<Coord>();
                Coords.Add(new Coord(1, 1));
                Coords.Add(new Coord(6, 2));
                Coords.Add(new Coord(5, 3));
            }
        }
    }
}
