<?php

	// Etape 0 : On créer une référence à la base de données
	require_once 'include/DB_Functions.php';
	$db = new DB_Functions();
	
	// Etape 1 : On récupère les données en Base de données
	$categories = $db->getShoppingCategories();
		
	// Etape 2 : On renvoie la réponse à l'application !
	echo utf8_encode(json_encode($categories));

?>