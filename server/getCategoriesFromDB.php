<?php

	// Etape 0 : On cr�er une r�f�rence � la base de donn�es
	require_once 'include/DB_Functions.php';
	$db = new DB_Functions();
	
	// Etape 1 : On r�cup�re les donn�es en Base de donn�es
	$categories = $db->getShoppingCategories();
		
	// Etape 2 : On renvoie la r�ponse � l'application !
	echo utf8_encode(json_encode($categories));

?>