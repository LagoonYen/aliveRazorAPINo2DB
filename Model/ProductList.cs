using System;
using System.Collections.Generic;

namespace AliveStoreTemplate.Model
{
    public partial class ProductList
    {
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string CardName { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Move> Moves { get; set; }
        public int HP { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public string ImgUrl { get; set; }
        public DateTime RealseTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public partial class Ability
    {
        public string AbilityName { get; set; }
        public string AbilityDesc { get; set; }
    }

    public partial class Move
    {
        public string MoveName { get; set; }
        public string MoveDesc { get; set; }
    }
}
