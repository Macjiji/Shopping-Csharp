<?php

	// Etape 0 : On cr�er une r�f�rence � la base de donn�es
	require_once 'include/DB_Functions.php';
	$db = new DB_Functions();
	
	// Etape 1 : On r�cup�re les donn�es des items sur la base de donn�es
	$items = $db->getShoppingItems();
		
	// Etape 4 : On renvoie la r�ponse � l'application !
	echo utf8_encode(json_encode($items));

?>