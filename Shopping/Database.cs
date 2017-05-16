using Android.Database.Sqlite;
using System.IO;
using System.Collections.Generic;
using Android.Util;

namespace Shopping_List_CSharp
{
    class Database
    {

        /// <summary>
        /// Liste des attributs
        /// </summary>
        private SQLiteDatabase sqldb;
        private string sqldb_query;
        private string sqldb_message;
        private bool sqldb_available;


        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database(); 
        ///     </code>
        /// </example>
        public Database()
        {
            sqldb_message = "";
            sqldb_available = false;
        }

        /// <summary>
        /// Constructeur prenant en paramètre le nom de la base de données
        /// </summary>
        /// <param name="sqldb_name">Le nom de la base de données sur laquelle effectuer les opération</param>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database("Shopping")
        ///     </code>
        /// </example>
        public Database(string sqldb_name)
        {
            try
            {
                sqldb_message = "";
                sqldb_available = false;
                CreateDatabase(sqldb_name);
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        /// <summary>
        /// Getter et Setter du booléen sqldb_available
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre pour le Getter :
        ///     <code>
        ///         boolean isDatabaseAvailable = Database.DatabaseAvailable
        ///     </code>
        ///     Méthode de mise en oeuvre pour le Setter :
        ///     <code>
        ///         Database.DatabaseAvailable = true;
        ///     </code>
        /// </example>
        public bool DatabaseAvailable
        {
            get { return sqldb_available; }
            set { sqldb_available = value; }
        }

        /// <summary>
        ///     Getter et Setter pour le message
        /// </summary>
        public string Message
        {
            get { return sqldb_message; }
            set { sqldb_message = value; }
        }

        /// <summary>
        ///     Méthode de création de la base de données. Cette méthode est mise en privée car elle n'est utilisée que par le constructeur Database(string dbname)
        /// </summary>
        /// <param name="sqldb_name">Le nom de la base de données à créer</param>
        /// <see cref="Database(string)"/>
        private void CreateDatabase(string sqldb_name)
        {
            try
            {
                sqldb_message = "";
                string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sqldb_path = Path.Combine(sqldb_location, sqldb_name);
                bool sqldb_exists = File.Exists(sqldb_path);
                if (!sqldb_exists)
                {
                    sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path, null);
                    sqldb_query = "CREATE TABLE IF NOT EXISTS Categories (_id INTEGER PRIMARY KEY NOT NULL, nom_fr TEXT NOT NULL, nom_en TEXT NOT NULL);";
                    sqldb.ExecSQL(sqldb_query);
                    sqldb_query = "CREATE TABLE IF NOT EXISTS Items (_id INTEGER PRIMARY KEY NOT NULL, id_cat INTEGER NOT NULL, nom_fr TEXT NOT NULL, nom_en TEXT NOT NULL);";
                    sqldb.ExecSQL(sqldb_query);
                    sqldb_query = "CREATE TABLE IF NOT EXISTS PersonnalShopping (_id INTEGER PRIMARY KEY AUTOINCREMENT, nom TEXT NOT NULL, red INTEGER NOT NULL, green INTEGER NOT NULL, blue INTEGER NOT NULL);";
                    sqldb.ExecSQL(sqldb_query);
                    sqldb_query = "CREATE TABLE IF NOT EXISTS PersonnalShoppingItems (_id INTEGER PRIMARY KEY AUTOINCREMENT, id_list INTEGER NOT NULL, id_cat INTEGER NOT NULL, id_item INTEGER NOT NULL);";
                    sqldb.ExecSQL(sqldb_query);
                    sqldb_message = "Base de données : " + sqldb_name + " a été créée avec succès !";
                }
                else
                {
                    sqldb = SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
                    sqldb_message = "Base de données : " + sqldb_name + " a été ouverte avec succès !";
                }
                sqldb_available = true;
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }




        /// <summary>
        ///     Méthode permettant d'ajouter une catégorie à la base de données
        /// </summary>
        /// <param name="category">La catégorie à ajouter</param>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database("Shopping");
        ///         bdd.AddCategory(new Category(0, "Fruits", "Fruits"));
        ///     </code>
        /// </example>
        public void AddCategory(Category category)
        {
            try
            {
                sqldb_query = "INSERT INTO Categories (_id, nom_fr, nom_en) VALUES ("
                            + category.IdCat + ", '"
                            + category.NameFr + "', '"
                            + category.NameEn + "');";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Ajout de la catégorie effectué !";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        /// <summary>
        ///     Méthode permettant d'ajouter un item à la base de données
        /// </summary>
        /// <param name="item">L'item à ajouter</param>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database("Shopping");
        ///         bdd.AddItem(new Item(0, 1, "Banane", "Banana"));
        ///     </code>
        /// </example>
        public void AddItem(Item item)
        {
            try
            {
                sqldb_query = "INSERT INTO Items (_id, id_cat, nom_fr, nom_en) VALUES ("
                            + item.IdItem + ", "
                            + item.IdCat + ", '"
                            + item.NameFr + "', '"
                            + item.NameEn + "');";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Ajout de l'item effectué !";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        /// <summary>
        ///     Méthode permettant d'ajouter une liste de courses à la base de données
        /// </summary>
        /// <param name="shoppingList">La liste de courses</param>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database("Shopping");
        ///         bdd.AddShoppingList(new ShoppingList("Test Liste, 255, 255, 255));
        ///     </code>
        /// </example>
        public void AddShoppingList(ShoppingList shoppingList)
        {
            try
            {
                Log.Debug("Database", "Valeurs : " + shoppingList.ToString());
                sqldb_query = "INSERT INTO PersonnalShopping (nom, red, green, blue) VALUES ('"
                            + shoppingList.Nom + "', "
                            + shoppingList.Red + ", "
                            + shoppingList.Green + ", "
                            + shoppingList.Blue + ");";
                Log.Debug("SQL", "Request : " + sqldb_query);
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Ajout de la liste de courses effectué !";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        /// <summary>
        ///     Méthode permettant d'ajouter un item à une liste de courses au sein de la base de données
        /// </summary>
        /// <param name="shoppingListItem">L'item de liste de courses</param>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database bdd = new Database("Shopping");
        ///         bdd.AddShoppingListItem((new ShoppingListItem(0, 1, 1));
        ///     </code>
        /// </example>
        public void AddShoppingListItem(ShoppingListItem shoppingListItem)
        {
            try
            {
                sqldb_query = "INSERT INTO PersonnalShoppingItems (id_list, id_cat, id_item) VALUES ("
                            + shoppingListItem.IdList + ", "
                            + shoppingListItem.IdCat + ", "
                            + shoppingListItem.IdItem + ");";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Item ajouté à la liste de courses !";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }




        /// <summary>
        ///     Méthode permettant de récupérer toutes les listes de courses présentes en base de données.
        /// </summary>
        /// <returns>Les listes de courses si elles existent, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetAllShoppinLists();
        ///     </code>
        /// </example>
        public List<ShoppingList> GetAllShoppingLists()
        {
            List<ShoppingList> listeShopping = new List<ShoppingList>();
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM PersonnalShopping;";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas de liste de courses trouvées ! ";
                    listeShopping = null;
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Listes de courses trouvées ! ";
                    sqldb_cursor.MoveToFirst();
                    for (int i = 0; i < sqldb_cursor.Count; i++)
                    {
                        ShoppingList shoppingList = new ShoppingList();
                        shoppingList.IdShoppingList = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id"));
                        shoppingList.Nom = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom"));
                        shoppingList.Red = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("red"));
                        shoppingList.Green = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("green"));
                        shoppingList.Blue = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("blue"));
                        Log.Debug("Database", "Valeurs : " + sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id")));
                        listeShopping.Add(shoppingList);
                        sqldb_cursor.MoveToNext();
                    }
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return listeShopping;
        }

        /// <summary>
        ///     Méthode permettant de récupérer toutes les catégories présentes en base de données.
        /// </summary>
        /// <returns>La liste de catégories si elles existent, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetAllCAtegories();
        ///     </code>
        /// </example>
        public List<Category> GetAllCategories()
        {
            List<Category> listeCategories = new List<Category>();
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM Categories;";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas de catégories trouvées ! ";
                    listeCategories = null;
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Catégories trouvées ! ";
                    sqldb_cursor.MoveToFirst();
                    for (int i = 0; i < sqldb_cursor.Count; i++)
                    {
                        Category category = new Category();
                        category.IdCat = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id"));
                        category.NameFr = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_fr"));
                        category.NameEn = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_en"));
                        listeCategories.Add(category);
                        sqldb_cursor.MoveToNext();
                    }
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return listeCategories;
        }

        /// <summary>
        ///     Méthode permettant de récupérer tous les items présents en base de données.
        /// </summary>
        /// <returns>La liste d'items si ils existent, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetAllItems();
        ///     </code>
        /// </example>
        public List<Item> GetAllItems()
        {
            List<Item> listeItems = new List<Item>();
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM Items;";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas d'items trouvés ! ";
                    listeItems = null;
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Items trouvées ! ";
                    sqldb_cursor.MoveToFirst();
                    for (int i = 0; i < sqldb_cursor.Count; i++)
                    {
                        Item item = new Item();
                        item.IdItem = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id"));
                        item.IdCat = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("id_cat"));
                        item.NameFr = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_fr"));
                        item.NameEn = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_en"));
                        listeItems.Add(item);
                        sqldb_cursor.MoveToNext();
                    }
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return listeItems;
        }

        /// <summary>
        ///     Méthode permettant de récupérer tous les items d'une liste de courses présents en base de données.
        /// </summary>
        /// <returns>La liste d'items d'une liste de courses si ils existent, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetAllShoppingListItemsWithId(1);
        ///     </code>
        /// </example>
        public List<ShoppingListItem> GetAllShoppingListItemsWithId(int listId)
        {
            List<ShoppingListItem> shoppingListItems = new List<ShoppingListItem>();
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM PersonnalShoppingItems WHERE id_list = '" + listId + "';";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas d'items trouvés dans la liste de courses ! ";
                    shoppingListItems = null;
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Items trouvées dans la liste de courses ! ";
                    sqldb_cursor.MoveToFirst();
                    for (int i = 0; i < sqldb_cursor.Count; i++)
                    {
                        Log.Debug("Item", "Valeurs : " + sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id")));
                        ShoppingListItem shoppingListItem = new ShoppingListItem();
                        shoppingListItem.Id = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("_id"));
                        shoppingListItem.IdList = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("id_list"));
                        shoppingListItem.IdCat = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("id_cat"));
                        shoppingListItem.IdItem = sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("id_item"));
                        shoppingListItems.Add(shoppingListItem);
                        sqldb_cursor.MoveToNext();
                    }
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return shoppingListItems;
        }

        /// <summary>
        ///     Méthode permettant de récupérer tous le nom des items d'une liste de courses présents en base de données.
        /// </summary>
        /// <returns>La liste des noms d'items d'une liste de courses si ils existent, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetPersonnalItemsFromShoppingList(1, 3);
        ///     </code>
        /// </example>
        public List<string> GetPersonnalItemsFromShoppingList(int idList, int idCat)
        {
            List<string> personnalItems = new List<string>();
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM PersonnalShoppingItems WHERE id_list = " + idList + " AND id_cat = " + idCat + ";";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas d'items trouvés dans la liste de courses ! ";
                    personnalItems = null;
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Items trouvées dans la liste de courses ! ";
                    sqldb_cursor.MoveToFirst();
                    for (int i = 0; i < sqldb_cursor.Count; i++)
                    {
                        personnalItems.Add(GetItemNameWithId(sqldb_cursor.GetInt(sqldb_cursor.GetColumnIndex("id_item"))));
                        sqldb_cursor.MoveToNext();
                    }
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return personnalItems;
        }




        /// <summary>
        ///     Méthode permettant de récupérer le nom d'une catégorie présente en base de données.
        /// </summary>
        /// <returns>Le nom de la catégorie, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetCategoryNameWithId(5);
        ///     </code>
        /// </example>
        public string GetCategoryNameWithId(int idCat)
        {
            string result = null;
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM Categories WHERE _id = " + idCat + ";";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas de catégorie trouvée ! ";
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Catégorie trouvée ! ";
                    sqldb_cursor.MoveToFirst();
                    result = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_fr"));
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return result;
        }

        /// <summary>
        ///     Méthode permettant de récupérer le nom d'un item présent en base de données.
        /// </summary>
        /// <returns>Le nom de l'item, null sinon</returns>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.GetItemNameWithId(5);
        ///     </code>
        /// </example>
        public string GetItemNameWithId(int idItem)
        {
            string result = null;
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM Items WHERE _id = " + idItem + ";";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Pas d'item trouvé ! ";
                    sqldb_cursor.Close();
                }
                else
                {
                    sqldb_message = "Item trouvé ! ";
                    sqldb_cursor.MoveToFirst();
                    result = sqldb_cursor.GetString(sqldb_cursor.GetColumnIndex("nom_fr"));
                    sqldb_cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return result;
        }




        /// <summary>
        ///     Méthode permettant de supprimer une catégorie présente en base de données.
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.DeleteCategoryWithId(1);
        ///     </code>
        /// </example>
        public void DeleteCategoryWithId(int idCat)
        {
            sqldb.Delete("Categories", "_id = " + idCat, null);
        }

        /// <summary>
        ///     Méthode permettant de supprimer un item présent en base de données.
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.DeleteItemWithId(10);
        ///     </code>
        /// </example>
        public void DeleteItemWithId(int idItem)
        {
            sqldb.Delete("Items", "_id = " + idItem, null);
        }

        /// <summary>
        ///     Méthode permettant de supprimer une liste de courses et tous ces items rattachés.
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.DeleteShoppingListWithId(1);
        ///     </code>
        /// </example>
        public void DeleteShoppingListWithId(int idList)
        {
            sqldb.Delete("PersonnalShoppingItems", "id_list = " + idList, null);
            sqldb.Delete("PersonnalShopping", "_id = " + idList, null);
        }

        /// <summary>
        ///     Méthode permettant de supprimer un item rattaché à une liste de courses.
        /// </summary>
        /// <example>
        ///     Méthode de mise en oeuvre :
        ///     <code>
        ///         Database baseDeDonnees = new Database("Shopping");
        ///         baseDeDonnees.DeleteItemIntoShoppingList(1, 10);
        ///     </code>
        /// </example>
        public void DeleteItemIntoShoppingList(int idList, int idItem)
        {
            sqldb.Delete("PersonnalShoppingItems", "id_list = " + idList + " AND id_item = " + idItem, null);
        }

    }
}