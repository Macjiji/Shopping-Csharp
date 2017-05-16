
namespace Shopping_List_CSharp
{
    class ShoppingListItem
    {

        private int id;
        private int idList;
        private int idCat;
        private int idItem;

        public ShoppingListItem() { }

        public ShoppingListItem(int idList, int idCat, int idItem)
        {
            this.idList = idList;
            this.idCat = idCat;
            this.idItem = idItem;
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int IdList
        {
            get { return this.idList; }
            set { this.idList = value; }
        }

        public int IdCat
        {
            get { return this.idCat; }
            set { this.idCat = value; }
        }

        public int IdItem
        {
            get { return this.idItem; }
            set { this.idItem = value; }
        }

        public override string ToString()
        {
            return "ShoppingListItem { Identifiant : " + this.id
                        + ", IdList : " + this.idList
                        + ", IdCat : " + this.idCat
                        + ", IdItem : " + this.idItem + "}";
        }

    }
}