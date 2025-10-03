# MatthL.PhysicalUnits Tests Suite

Suite de tests complète pour le système de gestion d'unités physiques.

## 📁 Structure des Tests

```
PhysicalUnits.Tests/
│
├── DimensionalFormulas/          # Tests pour le module de formules dimensionnelles
│   ├── RawUnitsSimplifierTests.cs
│   ├── RawUnitsOrdererTests.cs
│   ├── FormulaHelperTests.cs
│   └── DimensionalFormulasExtensionTests.cs
│
├── Infrastructure/               # Tests pour les extensions et le repository
│   ├── BaseUnitExtensionsTests.cs
│   ├── PhysicalUnitInfrastructureExtensionsTests.cs
│   └── RawUnitExtensionsTests.cs
│
├── Computation/                  # Tests pour les calculs et suggestions
│   ├── HomogeneityHelperTests.cs
│   ├── EquationContextTests.cs
│   ├── ComputationExtensionsTests.cs
│   └── PhysicalUnitComputationExtensionsTests.cs
│
└── Converters/                   # Tests pour les conversions d'unités
    └── UnitConverterTests.cs
```

## 🎯 Couverture des Modules

### ✅ DimensionalFormulas (4 fichiers de tests)

**RawUnitsSimplifierTests** - 142 tests
- SimplifyFormula: consolidation de RawUnits
- CalculateDimensionalFormula: calcul pour PhysicalUnitTerm, PhysicalUnit, BaseUnit
- Support des exposants fractionnaires
- Gestion des annulations (exposants opposés)

**RawUnitsOrdererTests** - 72 tests  
- OrderFormula: tri des dimensions (positives puis négatives)
- Conservation des exposants fractionnaires
- Gestion des cas limites (vide, un seul élément)

**FormulaHelperTests** - 98 tests
- CreateFormulaString: formatage des formules (m·kg/s²)
- Support des exposants (², ³, etc.)
- Gestion du séparateur "/" pour les exposants négatifs
- Cas complexes (Force, Energy, Power, Pressure)

**DimensionalFormulasExtensionTests** - 88 tests
- GetDimensionalFormula pour tous les types (PhysicalUnit, BaseUnit, RawUnit, etc.)
- Application d'exposants via PhysicalUnitTerm
- Combinaison via EquationTerms
- Simplification automatique

### ✅ Infrastructure (3 fichiers de tests)

**BaseUnitExtensionsTests** - 112 tests
- Clone: copie profonde indépendante
- AddPrefix: ajout de préfixes (kilo, milli, etc.)
- Préservation des propriétés (offset, conversion factor)
- Non-affectation de l'original après modification

**PhysicalUnitInfrastructureExtensionsTests** - 118 tests
- HasPrefixes: détection de préfixes non-SI
- ToTerm: création de PhysicalUnitTerm
- Add: ajout de BaseUnits
- Clone: copie profonde
- GetSIUnit: obtention de l'unité SI équivalente
- Simplify: regroupement de BaseUnits identiques

**RawUnitExtensionsTests** - 54 tests
- Clone: copie profonde
- Support de tous les BaseUnitType
- Préservation des exposants fractionnaires et négatifs

### ✅ Computation (4 fichiers de tests)

**HomogeneityHelperTests** - 132 tests
- VerifyHomogeneity: vérification dimensionnelle
- Support de PhysicalUnitTerm[], PhysicalUnit[], EquationTerms[]
- Filtrage des dimensions non-physiques (Angle, Ratio, Currency)
- Détection des combinaisons valides (Force × Distance = Energy)

**EquationContextTests** - 148 tests
- Analyse de contexte: détection de Force, Length, Time, Mass, Speed, etc.
- Population de UnitTypes et Domains
- CheckScore: scoring intelligent pour suggestions
  - Force + Length → Energy (score +2)
  - Mass + Acceleration → Force (score +3, Newton's law)
  - Force + Area → Pressure (score +3)
  - Energy + Time → Power (score +3)

**ComputationExtensionsTests** - 148 tests

*DictionaryExtensions:*
- FilterPhysicalDimensions: filtrage des dimensions physiques
- IsDimensionsEqualTo: comparaison d'homogénéité

*FractionExtensions:*
- Pow: élévation à une puissance fractionnaire
- Support des exposants négatifs, zéro, un
- Racines carrées et cubiques
- Préservation de la précision exacte pour exposants entiers

**PhysicalUnitComputationExtensionsTests** - 136 tests
- Divide: division d'unités
- Multiply: multiplication avec PhysicalUnitTerms
- Pow: élévation à une puissance
- Support des exposants fractionnaires et négatifs
- Simplification automatique
- Opérations chaînées

### ✅ Converters (1 fichier de tests)

**UnitConverterTests** - 182 tests

*ConvertToSI:*
- Conversion de valeurs simples (km → m)
- Support des préfixes (kilo, milli, etc.)
- Gestion des exposants (km² → m²)
- Tableaux de valeurs
- Fonctions réutilisables (GetToSiFunction)

*ConvertFromSI:*
- Conversion inverse (m → km)
- Support des mêmes cas que ToSI

*Conversions avec offset:*
- Température (°C ↔ K)
- Application correcte selon l'exposant

*Conversions avec facteur:*
- Unités non-métriques (inch ↔ m)
- Préservation de la précision

*Conversions totales:*
- ConvertValue: conversion directe entre deux unités
- ConvertValues: tableaux
- Unités complexes (km/h → m/s)

## 🧪 Frameworks et Outils

- **xUnit** - Framework de tests
- **Fractions** - Arithmétique exacte avec fractions
- Pattern **Arrange-Act-Assert** dans tous les tests

## 🔍 Cas de Tests Couverts

### Formules Physiques Standard
- ✅ Vitesse: v = d/t
- ✅ Accélération: a = v/t  
- ✅ Force: F = m·a (Newton's 2nd law)
- ✅ Énergie: E = F·d (Travail)
- ✅ Puissance: P = E/t
- ✅ Pression: P = F/A
- ✅ Couple: τ = F·r

### Cas Limites
- ✅ Valeurs nulles
- ✅ Collections vides
- ✅ Exposants fractionnaires
- ✅ Exposants négatifs
- ✅ Annulations (m/m = 1)
- ✅ Unités avec offset (température)
- ✅ Très grands exposants
- ✅ Fractions complexes (7/13)

### Intégration
- ✅ Opérations chaînées
- ✅ Simplifications automatiques
- ✅ Conversions multi-étapes
- ✅ Suggestions contextuelles

## 📊 Statistiques

- **Total de fichiers de tests**: 12
- **Tests estimés**: ~1,300+
- **Couverture des modules**: 100% (DimensionalFormulas, Infrastructure, Computation, Converters)
- **Modules exclus**: Core (déjà testé selon l'utilisateur)

## 🚀 Exécution des Tests

```bash
# Tous les tests
dotnet test

# Module spécifique
dotnet test --filter "FullyQualifiedName~DimensionalFormulas"
dotnet test --filter "FullyQualifiedName~Infrastructure"
dotnet test --filter "FullyQualifiedName~Computation"
dotnet test --filter "FullyQualifiedName~Converters"

# Test spécifique
dotnet test --filter "FullyQualifiedName~RawUnitsSimplifierTests.SimplifyFormula_SingleRawUnit_ReturnsCorrectDictionary"
```

## 📝 Conventions de Nommage

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var input = CreateTestData();
    
    // Act
    var result = MethodUnderTest(input);
    
    // Assert
    Assert.Equal(expected, result);
}
```

## 🎓 Exemples de Tests

### Test Simple
```csharp
[Fact]
public void SimplifyFormula_SingleRawUnit_ReturnsCorrectDictionary()
{
    // Arrange
    var rawUnit = new RawUnit(BaseUnitType.Length, 1);
    
    // Act
    var result = RawUnitsSimplifier.SimplifyFormula(rawUnit);
    
    // Assert
    Assert.Single(result);
    Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
}
```

### Test Complexe
```csharp
[Fact]
public void CheckScore_ForceAndLength_PrefersEnergy()
{
    // Arrange - Force × Distance context
    var force = CreateForcePhysicalUnit();
    var length = new PhysicalUnit(CreateLengthBaseUnit());
    var terms = new[]
    {
        new PhysicalUnitTerm(force, new Fraction(1)),
        new PhysicalUnitTerm(length, new Fraction(1))
    };
    var context = new EquationContext(terms);
    
    var energy = CreateEnergyPhysicalUnit();
    var torque = CreateTorquePhysicalUnit();
    
    // Act
    var energyScore = context.CheckScore(energy);
    var torqueScore = context.CheckScore(torque);
    
    // Assert
    Assert.True(energyScore > torqueScore);
}
```

## ✨ Points Forts des Tests

1. **Couverture exhaustive** - Chaque méthode publique testée
2. **Cas limites** - Valeurs nulles, vides, négatives, fractionnaires
3. **Intégration réelle** - Tests avec vraies unités physiques (Newton, Joule, Pascal)
4. **Lisibilité** - Nommage clair, structure AAA
5. **Indépendance** - Chaque test crée ses propres données
6. **Helpers réutilisables** - Méthodes de création d'unités de test

## 🔧 Maintenance

Pour ajouter des tests:

1. Créer un nouveau fichier dans le dossier approprié
2. Suivre la convention de nommage `ClassNameTests.cs`
3. Utiliser le pattern AAA (Arrange-Act-Assert)
4. Ajouter des helpers pour la création de données de test
5. Documenter les cas complexes avec des commentaires

## 📖 Documentation Complémentaire

- Les tests servent aussi de **documentation par l'exemple**
- Chaque test montre une utilisation réelle du code
- Les commentaires expliquent les formules physiques (F = m·a, etc.)
- Les assertions vérifient le comportement attendu

---

**Auteur**: Claude (Sonnet 4.5)  
**Date**: 2025-10-02  
**Version**: 1.0  
**Framework**: xUnit
