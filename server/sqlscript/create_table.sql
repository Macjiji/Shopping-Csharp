CREATE TABLE shopping.categories
(
    id_cat INT PRIMARY KEY AUTO_INCREMENT,
    name_cat_en TEXT NOT NULL,
    name_cat_fr TEXT NOT NULL
);

CREATE TABLE shopping.items
(
    id_item INT PRIMARY KEY AUTO_INCREMENT,
    id_cat INTEGER NOT NULL,
    name_item_en TEXT NOT NULL,
    name_item_fr TEXT NOT NULL,
    FOREIGN KEY (id_cat) REFERENCES categorie(id_cat)
);