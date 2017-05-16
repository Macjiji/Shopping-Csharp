<?php

	// Etape 0 : On créer une référence à la base de données
	require_once 'include/DB_Functions.php';
	$db = new DB_Functions();
	
	// Etape 1 : On récupère les données des items sur la base de données
	$items = $db->getShoppingItems();
		
	// Etape 4 : On renvoie la réponse à l'application !
	echo utf8_encode(json_encode($items));

?>