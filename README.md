# sysProgProject
## Convention de nommage :

- Variables :
  ```c#
  var variableTest = 12;
  bool boolTest = True;
  ```

- Classes :
  ```c#
  public class ClasseTest
  {
    ....
  }
  ```
- Structures :
  ```c#
  public struct StructureTest
  {
    ....
  }
  ```
- Records :
  ```c#
  public record RecordTest(
    string TestUn,
    string TestDeux,
    ....);
  ```
- Interfaces : 
  ```c#
  public interface IInterfaceTest
  {
    ....
  }
  ```

- Attributs publiques :
  ```c#
  public class ClasseTest
  {
    public bool BoolTest;
    public int IntTest;

    public void MethodTest()
    {
      ....
    }
  }
  ```

- Attributs privés ou internes :
  ```c#
  public class ClasseTest
  {
    internal bool _boolTest;
    private int _intTest;

    private void _methodTest()
    {
      ....
    }
  }
  ```

- Attributs statiques :
  ```c#
  public class ClasseTest
  {
    private static int s_intTest;

    [ThreadStatic]
      private static bool t_boolTest;
  }
  ```

- Méthodes :
  ```c#
  public class ClasseTest
  {
    public int MethodeTest<int>(int intTest, bool boolTest)
    {
      ....
    }
  }
  ```
## Conventions de commentaire :
- Placez le commentaire sur une ligne séparée, pas à la fin d'une ligne de code.
- Commencez le commentaire par une lettre majuscule.
- Terminez le texte de commentaire par un point.
- Insérez un espace entre le délimiteur de commentaire (//) et le texte du commentaire.
- Ne créez pas de blocs mis en forme d’astérisque autour des commentaires.

## Conventions de typage :
- Utilisez une interpolation de chaîne pour concaténer les chaînes courtes. 
  ```c# 
   string displayName = $"{nameList[n].LastName}, {nameList[n].FirstName}";
  ```
- Pour ajouter des chaînes dans des boucles, en particulier lorsque vous utilisez de grandes quantités de texte, utilisez un StringBuilder objet.
  ```c#
  var phrase = "lalalalalalalalalalalalalalalalalalalalalalalalalalalalalala";
  var manyPhrases = new StringBuilder();
  for (var i = 0; i < 10000; i++)
  {
      manyPhrases.Append(phrase);
  }
  ``` 
- Utilisez le typage implicite pour les variables locales quand le type de la variable est évident à droite de l’assignation ou quand le type précis n’importe pas. 
  ```c#
  var var1 = "This is clearly a string.";
  var var2 = 27;
  ```
- N’utilisez pas ```var``` lorsque le type n’est pas apparent du côté droit de l’affectation. Ne supposez pas que le type est clair à partir d’un nom de méthode. Un type de variable est considéré comme clair s’il s’agit d’un opérateur ou d’un new cast explicite.
  ```c#
  int var3 = Convert.ToInt32(Console.ReadLine()); 
  int var4 = ExampleClass.ResultSoFar();
  ```
- Ne vous fiez pas au nom de la variable pour spécifier le type de la variable.
- En règle générale, utilisez int plutôt que les types non signés. L'utilisation de int est commun en C# et il est plus facile d'interagir avec d'autres bibliothèques lorsque vous utilisez int.
- Utilisez la syntaxe concise lorsque vous initialisez des tableaux sur la ligne de déclaration.
  ```c#
  string[] vowels1 = { "a", "e", "i", "o", "u" };
  var vowels2 = new string[] { "a", "e", "i", "o", "u" };
  ```



## Conventions diverses :
- Utilisez les paramètres par défaut de l'éditeur de code
- Écrivez une seule instruction par ligne.
- Écrivez une seule déclaration par ligne.
- Ajoutez au moins une ligne blanche entre les définitions des méthodes et les définitions des propriétés.
- Utilisez des parenthèses pour rendre apparentes les clauses d'une expression ```if ((val1 > val2) && (val1 > val3))
{
    ....
}```
- Utilisez une instruction try-catch pour la plus grande part de la gestion des exceptions.
- Utilisez l’une des formes concises d’instanciation d’objet
  ```c#
  var instance1 = new ExampleClass();
  ExampleClass instance2 = new();
  ExampleClass instance2 = new ExampleClass();
  ```
- Utilisez des initialiseurs d’objets pour simplifier la création d’objets.
  ```c#
  var instance3 = new ExampleClass { Name = "Desktop", ID = 37414, Location = "Redmond", Age = 2.3 };
  ```
- Appelez les membres static en utilisant le nom de la classe : Nom_classe.Membre_statique.
- 
