namespace Shopping_List_CSharp
{
    class ShoppingList
    {

        private int idShoppingList;
        private string nom;
        private int red;
        private int green;
        private int blue;

        public ShoppingList() { }

        public ShoppingList(string nom, int red, int green, int blue)
        {
            this.nom = nom;
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public int IdShoppingList
        {
            get { return this.idShoppingList; }
            set { this.idShoppingList = value; }
        }

        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }

        public int Red
        {
            get { return this.red; }
            set { this.red = value; }
        }

        public int Green
        {
            get { return this.green; }
            set { this.green = value; }
        }

        public int Blue
        {
            get { return this.blue; }
            set { this.blue = value; }
        }

        public override string ToString()
        {
            return "ShoppingList { Identifiant : " + this.idShoppingList
                        + ", Nom : " + this.nom
                        + ", Red : " + this.red
                        + ", Green: " + this.green
                        + ", Blue : " + this.blue + "}";
        }

    }
}