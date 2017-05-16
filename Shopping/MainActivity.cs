using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Java.Util;
using Newtonsoft.Json;
using Square.OkHttp3;
using System.Collections.Generic;

namespace Shopping_List_CSharp
{
    [Activity(Label = "Shopping_List_CSharp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // URL faisant le lien entre l'application et le serveur à partir des scripts PHP
        private readonly string urlCategories = "http://192.168.1.63/shopping/getCategoriesFromDB.php";
        private readonly string urlItems = "http://192.168.1.63/shopping/getItemsFromDB.php";

        // Composants graphiques
        protected ListView listesCourses;
        protected Button creerListe;
        protected Button majListe;

        // Liste des items et catégories déjà présents en base de données
        private List<Category> listeCategories = new List<Category>();
        private List<Item> listeItems = new List<Item>();
        private List<ShoppingList> shoppingLists = new List<ShoppingList>();


        // Base de données
        private Database baseDeDonnees;


        /// <summary>
        ///     Méthode onCreate
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            initialiseBaseDeDonnees();
            initialiseBoutons();
            initialiseListView();
        }

        /// <summary>
        ///     Méthode d'initialisation de la base de données
        /// </summary>
        private void initialiseBaseDeDonnees()
        {
            baseDeDonnees = new Database("Shopping");
            listeCategories = baseDeDonnees.GetAllCategories();
            listeItems = baseDeDonnees.GetAllItems();
            shoppingLists = baseDeDonnees.GetAllShoppingLists();
        }


        /// <summary>
        ///     Méthode d'initialisation des boutons
        /// </summary>
        private void initialiseBoutons()
        {
            majListe = FindViewById<Button>(Resource.Id.maj_produit);
            majListe.Click += async delegate // On met la récupération des données en asynchrone
            {
                // Etape 0 : On crée une instance de OkHttpClient
                OkHttpClient client = new OkHttpClient();

                // Etape 1 : On crée les requêtes à partir des URL des scripts PHP
                Request requestCategories = new Request.Builder()
                    .Url(urlCategories)
                    .Build();

                Request requestItems = new Request.Builder()
                    .Url(urlItems)
                    .Build();

                // Etape 2 : On exécute les requêtes en vue de récupérer les réponses
                Response responseCategories = await client.NewCall(requestCategories).ExecuteAsync();
                Response responseItems = await client.NewCall(requestItems).ExecuteAsync();


                // Etape 3 : On récupère les données de la requête de récupération des catégories, et on transforme la réponse JSON en liste de Categories
                string bodyCategories = await responseCategories.Body().StringAsync();
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(bodyCategories);
                foreach(Category category in categories)
                {               
                    Log.Debug("Category", "Valeurs : " + category.IdCat + " " + category.NameFr + " " + category.NameEn);
                    if (!listeCategories.Contains(category))
                    {
                        baseDeDonnees.AddCategory(category);
                    }
                }

                listeCategories = baseDeDonnees.GetAllCategories();


                // Etape 4 : On récupère les données de la requête de récupération des items, et on transforme la réponse JSON en liste d'Items
                string bodyItems = await responseItems.Body().StringAsync();
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(bodyItems);
                foreach (Item item in items)
                {
                    Log.Debug("Item", "Valeurs : " + item.IdItem + " " + item.IdCat + " " + item.NameEn + " " + item.NameFr);
                    if (!listeItems.Contains(item))
                    {
                        baseDeDonnees.AddItem(item);
                    }
                }

                listeItems = baseDeDonnees.GetAllItems();

                initialiseListView();

            };

            creerListe = FindViewById<Button>(Resource.Id.creer_liste);
            creerListe.Click += delegate
            {
                StartActivity(typeof(CreerListe));
            };

        }

        /// <summary>
        ///     Méthode d'initialisation des listes de courses
        /// </summary>
        private void initialiseListView() {
            listesCourses = FindViewById<ListView>(Resource.Id.list_shopping);
            if(baseDeDonnees.GetAllShoppingLists() != null)
            {
                ShoppingList[] arrayShoppingList = shoppingLists.ToArray();
                ListViewAdapter adapter = new ListViewAdapter(this, arrayShoppingList);
                listesCourses.Adapter = adapter;
                listesCourses.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                {
                    Intent apercuListe = new Intent(this, typeof(ApercuListe));
                    apercuListe.PutExtra("name", shoppingLists[e.Position].Nom);
                    apercuListe.PutExtra("id", shoppingLists[e.Position].IdShoppingList);
                    StartActivity(apercuListe);
                };

            }
        }
   

    }
}

