use livinparis;

-- Insertion des 10 catégories
INSERT INTO
    CategorieAmbiance (Nom)
VALUES
    ('Épices'),
    ('Familliale'),
    ('Exotique'),
    ('Traditionnelle'),
    ('Romantique'),
    ('Soleil'),
    ('Montagnarde'),
    ('Festive'),
    ('Cosy'),
    ('Chic');

-- Association des 5 premiers plats à des catégories
INSERT INTO
    PlatCategorieAmbiance (IdPlat, IdCategorie)
VALUES
    (1, 1), -- Lasagnes avec Épices
    (1, 2), -- Lasagnes avec Familliale
    (2, 3), -- Plat 2 avec Exotique
    (2, 6), -- Plat 2 avec Soleil
    (2, 4),
    (3, 4), -- Plat 3 avec Traditionnelle
    (4, 5), -- Plat 4 avec Romantique
    (5, 7), -- Plat 5 avec Montagnarde
    (5, 9);

-- Plat 5 avec Cosy
-- Cuisinier 1
INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (1, 5);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (1, 4);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (1, 5);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (1, 5);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (1, 4);

-- Cuisinier 2
INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (2, 3);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (2, 4);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (2, 4);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (2, 3);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (2, 4);

-- Cuisinier 3
INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (3, 2);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (3, 3);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (3, 3);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (3, 2);

INSERT INTO
    NotationCuisinier (IdCuisinier, Note)
VALUES
    (3, 3);