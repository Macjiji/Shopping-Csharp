using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Shopping_List_CSharp
{
    [Activity(Label = "ApercuListe")]
    public class ApercuListe : Activity
    {

        // Attributs pour les composants graphique et la BDD
        protected ImageButton ajouterArticle;
        protected ExpandableListView listArticles;
        private CustomExpandableListViewAdapter listAdapter;

        // Base de données
        private Database baseDeDonnees;

        // Attributs contenant l'ensemble des données de la BDD
        private List<ShoppingListItem> shoppingListItems = new List<ShoppingListItem>();
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
            SetContentView(Resource.Layout.ApercuListe);
            InitialiseBaseDeDonnees();
            InitialiseExpandableListView();
            InitialiseButton();
        }

        /// <summary>
        ///     Méthode d'initialisation de la base de données
        /// </summary>
        private void InitialiseBaseDeDonnees()
        {
            baseDeDonnees = new Database("Shopping");
            shoppingListItems = baseDeDonnees.GetAllShoppingListItemsWithId(Intent.GetIntExtra("id", -1));
        }

        /// <summary>
        ///     Méthode d'initialisation de la liste extensible
        /// </summary>
        private void InitialiseExpandableListView()
        {
            listArticles = FindViewById<ExpandableListView>(Resource.Id.apercu_liste_shopping);
            PrepareListDatas();
            listAdapter = new CustomExpandableListViewAdapter(this, listDataHeader, listDataChild);
            listArticles.SetAdapter(listAdapter);
            listArticles.ChildClick += (s, e) => {
                foreach(Item item in items)
                {
                    if(item.NameFr.Equals(listAdapter.GetChild(e.GroupPosition, e.ChildPosition).ToString()))
                    {

                        // Création de la fenêtre de dialogue
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Suppression d'un item dans la liste");
                        alert.SetMessage("Souhaitez-vous réellement supprimer " + listAdapter.GetChild(e.GroupPosition, e.ChildPosition).ToString() + " de votre liste de courses ?");
                        alert.SetPositiveButton("Oui", (senderAlert, args) => {
                            foreach(Item itemToDelete in items)
                            {
                                if (itemToDelete.NameFr.Equals(listAdapter.GetChild(e.GroupPosition, e.ChildPosition).ToString()))
                                {
                                    baseDeDonnees.DeleteItemIntoShoppingList(Intent.GetIntExtra("id", -1), item.IdItem);
                                }
                            }
                            shoppingListItems = baseDeDonnees.GetAllShoppingListItemsWithId(Intent.GetIntExtra("id", -1));
                            InitialiseExpandableListView();
                        });

                        alert.SetNegativeButton("Non", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                }

            };

        }

        /// <summary>
        ///     Méthode d'initialisation du bouton d'ajout d'un item à une liste de courses
        /// </summary>
        private void InitialiseButton()
        {
            ajouterArticle = FindViewById<ImageButton>(Resource.Id.ajouter_item);
            ajouterArticle.Click += delegate
            {
                Intent ajouterItem = new Intent(this, typeof(AjouterItemListe));
                ajouterItem.PutExtra("name", Intent.GetStringExtra("name"));
                ajouterItem.PutExtra("id", Intent.GetIntExtra("id", -1));
                StartActivity(ajouterItem);
            };
        }

        /// <summary>
        ///     Méthode permettant de préparer les données présentes dans la liste de courses
        /// </summary>
        private void PrepareListDatas()
        {
            listDataHeader = new List<string>();
            listDataChild = new Dictionary<string, List<string>>();


            if(shoppingListItems != null) // On vérifie qu'il existe bien des valeurs dans une liste de courses
            {
                Log.Debug("Item", "Valeurs : " + shoppingListItems.Count);
                foreach (ShoppingListItem shoppingListItem in shoppingListItems)
                {
                    Log.Debug("Item", "Valeurs : " + shoppingListItem.ToString());
                    if(listDataHeader.Count == 0) // On teste si la liste ne possède pas encore de catégories (Evite d'ajouter deux fois une catégorie)
                    {
                        listDataHeader.Add(baseDeDonnees.GetCategoryNameWithId(shoppingListItem.IdCat));
                        listDataChild.Add(baseDeDonnees.GetCategoryNameWithId(shoppingListItem.IdCat), baseDeDonnees.GetPersonnalItemsFromShoppingList(Intent.GetIntExtra("id", -1), shoppingListItem.IdCat));
                    }
                    else 
                    {
                        if (!listDataHeader.Contains(baseDeDonnees.GetCategoryNameWithId(shoppingListItem.IdCat)))
                        { // Si la liste des Headers ne possède pas encore le nom de la catégorie, on l'inclus dans ces cas là !
                            listDataHeader.Add(baseDeDonnees.GetCategoryNameWithId(shoppingListItem.IdCat));
                            listDataChild.Add(baseDeDonnees.GetCategoryNameWithId(shoppingListItem.IdCat), baseDeDonnees.GetPersonnalItemsFromShoppingList(Intent.GetIntExtra("id", -1), shoppingListItem.IdCat));
                        }
                    }
                }
            }

        }


    }
}