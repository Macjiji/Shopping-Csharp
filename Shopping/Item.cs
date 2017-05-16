using System;
using Newtonsoft.Json;

namespace Shopping_List_CSharp
{
    class Item
    {

        private int idItem;
        private int idCat;
        private string nameEn;
        private string nameFr;

        [JsonProperty(PropertyName = "id_item")]
        public int IdItem { get; set; }

        [JsonProperty(PropertyName = "id_cat")]
        public int IdCat { get; set; }

        [JsonProperty(PropertyName = "name_item_en")]
        public string NameEn { get; set; }

        [JsonProperty(PropertyName = "name_item_fr")]
        public string NameFr { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Item i = (Item)obj;

            return (idItem == i.idItem) && (idCat == i.idCat) && (nameEn == i.nameEn) && (nameFr == i.nameFr);
        }


    }
}