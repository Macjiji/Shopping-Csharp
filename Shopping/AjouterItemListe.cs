using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Shopping_List_CSharp
{
    [Activity(Label = "AjouterItemListe")]
    public class AjouterItemListe : Activity
    {

        // Attributs pour les composants graphique et la BDD
        protected ExpandableListView listArticles;
        private CustomExpandableListViewAdapter listAdapter;

        // Base de données
        private Database baseDeDonnees;

        // Attributs contenant l'ensemble des données de la BDD
        private List<Category> categories = new List<Category>();
        private List<Item> items = new List<Item>();

        // Attributs permettant de gérer la liste de courses
        private List<String> listDataHeader;
        private Dictionary<string, List<string>> listDataChild;

        /// <summary>
        ///     Méthode onCreate
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AjouterItemListe);
            InitialiseBaseDeDonnees();
            InitialiseListArticles();
        }

        /// <summary>
        ///     Méthode d'initialisation de la base de données
        /// </summary>
        private void InitialiseBaseDeDonnees()
        {
            baseDeDonnees = new Database("Shopping");
            categories = baseDeDonnees.GetAllCategories();
            items = baseDeDonnees.GetAllItems();
        }

        /// <summary>
        ///     Méthode d'initialisation de la liste extensible
        /// </summary>
        private void InitialiseListArticles()
        {
            listArticles = FindViewById<ExpandableListView>(Resource.Id.liste_items);
            PrepareListDatas();
            listAdapter = new CustomExpandableListViewAdapter(this, listDataHeader, listDataChild);
            listArticles.SetAdapter(listAdapter);
            listArticles.ChildClick += (s, e) => {
                foreach(Item item in items)
                {
                    if (item.NameFr.Equals(listAdapter.GetChild(e.GroupPosition, e.ChildPosition).ToString()))
                    {
                        ShoppingListItem shoppingListItem = new ShoppingListItem(Intent.GetIntExtra("id", -1), item.IdCat, item.IdItem);
                        baseDeDonnees.AddShoppingListItem(shoppingListItem); // On crée le lien entre la liste de courses et l'item sélectionné
                        Intent retourListe = new Intent(this, typeof(ApercuListe)); // Pour on redirige vers l'aperçu de la liste de courses
                        retourListe.PutExtra("id", Intent.GetIntExtra("id", -1));
                        retourListe.PutExtra("name", Intent.GetStringExtra("name"));
                        StartActivity(retourListe);
                    }
                }
            };
        }

        /// <summary>
        ///     Méthode permettant de préparer les données présentes dans la liste de courses
        /// </summary>
        private void PrepareListDatas()
        {
            listDataHeader = new List<string>();
            listDataChild = new Dictionary<string, List<string>>();

            foreach(Category category in categories)
            { // Pour chaque categorie
                listDataHeader.Add(category.NameFr); // On ajoute le nom de la catégorie
                List<String> listItemsIntoCategory = new List<string>(); // On initialise une liste qui va nous permettra d'afficher tous les items d'une catégorie
                foreach(Item item in items)
                { // Et pour chaque items
                    if (category.IdCat == item.IdCat)
                    { // On teste si l'identifiant de la catégorie correspond à la catégorie d'un items
                        listItemsIntoCategory.Add(item.NameFr); // Le cas échéant, on l'ajoute
                    }
                }
                listDataChild.Add(category.NameFr, listItemsIntoCategory); // On ajoute enfin l'ensemble de nos données
            }

        }

    }
}