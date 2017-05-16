using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;

namespace Shopping_List_CSharp
{
    [Activity(Label = "CreerListe")]
    public class CreerListe : Activity
    {

        // Composants graphiques
        protected EditText nomListe;
        protected TextView apercuCouleur;
        protected Button ajouterListe;
        protected SeekBar red;
        protected SeekBar green;
        protected SeekBar blue;

        // Base de données et liste de courses déjà existantes
        private Database baseDeDonnees;
        private List<ShoppingList> shoppingLists = new List<ShoppingList>();

        /// <summary>
        ///     Méthode onCreate
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreerListe);
            initialiseBaseDeDonnees();
            initialiseTextViews();
            initialiseSeekBar();
            initialiseEditText();
            initialiseButtons();
        }

        /// <summary>
        ///     Méthode d'initialisation de la base de données.
        /// </summary>
        private void initialiseBaseDeDonnees()
        {
            baseDeDonnees = new Database("Shopping");
            shoppingLists = baseDeDonnees.GetAllShoppingLists();
        }

        /// <summary>
        ///     Méthode d'initialise des vues textes
        /// </summary>
        private void initialiseTextViews()
        {
            apercuCouleur = FindViewById<TextView>(Resource.Id.apercu_couleur);
        }

        /// <summary>
        ///     Méthode d'initialisation des barres de progression
        /// </summary>
        private void initialiseSeekBar()
        {
            red = FindViewById<SeekBar>(Resource.Id.red);
            green = FindViewById<SeekBar>(Resource.Id.green);
            blue = FindViewById<SeekBar>(Resource.Id.blue);

            // Etape 1 : On attribue une valeur par défaut aux SeekBar
            red.Progress = 125;
            green.Progress = 125;
            blue.Progress = 125;


            // Etape 2 : On crée les différents listener sur les SeekBar, en changeant la couleur de l'aperçu de la couleur de liste
            red.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    apercuCouleur.SetBackgroundColor(Color.Argb(255, red.Progress, green.Progress, blue.Progress));
                }
            };

            green.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    apercuCouleur.SetBackgroundColor(Color.Argb(255, red.Progress, green.Progress, blue.Progress));
                }
            };

            blue.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    apercuCouleur.SetBackgroundColor(Color.Argb(255, red.Progress, green.Progress, blue.Progress));
                }
            };

            // Etape 3 : On attribue à l'aperçu une couleur par défaut grace aux valeur définit en étape 1
            apercuCouleur.SetBackgroundColor(Color.Argb(255, red.Progress, green.Progress, blue.Progress));
        }

        /// <summary>
        ///     Méthode d'initialisation des champs d'édition
        /// </summary>
        private void initialiseEditText()
        {
            nomListe = FindViewById<EditText>(Resource.Id.nom_liste);
        }

        /// <summary>
        ///     Méthode d'initialisation des boutons
        /// </summary>
        private void initialiseButtons()
        {
            ajouterListe = FindViewById<Button>(Resource.Id.creer_liste);
            ajouterListe.Click += delegate
            {
                bool verifExistance = false;
                if(nomListe.Text.Trim().Length > 0)
                {
                    if (shoppingLists != null)
                    {
                        foreach (ShoppingList shoppingList in shoppingLists)
                        {
                            if (shoppingList.Nom.Equals(nomListe.Text.Trim()))
                            {
                                nomListe.Error = "Cette liste existe déjà !";
                                verifExistance = true;
                            }
                        }
                    }

                    if (!verifExistance)
                    {
                        ShoppingList shoppingList = new ShoppingList(nomListe.Text.ToString(), red.Progress, green.Progress, blue.Progress);
                        baseDeDonnees.AddShoppingList(shoppingList);
                        Log.Debug("SQL", "Valeurs : " + baseDeDonnees.Message);
                        StartActivity(typeof(MainActivity));
                    }
                }
                else
                {
                    nomListe.Error = "Vous devez renseigner un nom de liste de courses !";
                }

            };
        }

    }
}