using System;
using Newtonsoft.Json;

namespace Shopping_List_CSharp
{
    class Category
    {
        private int idCat;
        private string nameEn;
        private string nameFr;

        [JsonProperty(PropertyName = "id_cat")]
        public int IdCat { get; set; }

        [JsonProperty(PropertyName = "name_cat_en")]
        public string NameEn { get; set; }

        [JsonProperty(PropertyName = "name_cat_fr")]
        public string NameFr { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Category c = (Category)obj;

            return (idCat == c.idCat) && (nameEn == c.nameEn) && (nameFr == c.nameFr);
        }

    }
}