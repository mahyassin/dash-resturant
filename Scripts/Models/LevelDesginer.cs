using System.Collections.Generic;


namespace Scripts.Models
{
	public class MapModel
	{   

        // ⌜ ﹉ ⌝
        // ꤯     ꤯
        // ⌞ ﹍ ⌟

		
		private string[] _mapCode = {
			" WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
			" WW .. .. P. .. .. .. .. S. WW .. .. .. .. .. .. .. WW",
			" WW Go .. .. .. .. .. .. C. WW WW WW WW .. WW WW WW WW",
			" WW Gt .. Td T. T. .. .. .. WW .. .. .. .. .. .. .. WW",
			" WW WW WW WW WW WW WW .. WW WW .. .. .. .. .. .. .. WW",
			" WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
			" WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
			" WW .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WW",
			" WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW WW",
		};

		public GameState GetMapCodeState() => MapToState(_mapCode);


		public GameState MapToState(string[] mapCode)
		{
			int maphieght = _mapCode.Length;
			int mapWidth = _mapCode[0].Replace(" ", "").Length/2;

			Dictionary<Vector, CellState> map = new();	
			Player player = null;

			int y = 0;
			List<IInteractalbe> tools = new();

			char basetile = '?';
			char ontile = '?';
          

			

			foreach(var row in mapCode)
			{
				
				int x = 0;
				
				foreach(var cell in row.Replace(" ", ""))
                {

                    if (basetile == '?')
                    {
                        basetile = cell;
                        continue;
                    }
                    if (ontile == '?')
                    {
                        ontile = cell;
                    }

                    ICarriable onhand = ViewIngredint(ontile);

                    IOccupier entity = basetile switch
                    {
                        'W' => new Wall(),
                        'P' => new Player(new Vector(x, y), onhand),
                        'S' => new Stove(),
                        'C' => new CuttingBoard(onhand as Ingredint),
                        'G' => new GoodsStock(onhand as Ingredint),
                        'T' => new Table(onhand),
                        _ => null
                    };
                    if (entity is Player) player = entity as Player;
                    if (entity is IInteractalbe station) tools.Add(station);

                    map[new Vector(x, y)] = new CellState(pos: new(x, y), entity);

                    basetile = '?';
                    ontile = '?';

                    x++;
                }
                y++;
				
			}
		

			return new(mapWidth, maphieght, player, map, tools);
		}

        private static ICarriable ViewIngredint(char ontile)
        {
            return ontile switch
            {
                't' => new Ingredint(IngredintType.TOMATO),
                'o' => new Ingredint(IngredintType.ONION),
                'd' => new Plate(),
                _ => null
            };
        }
    }

}
