# sysProgProject
## Convention de nommage :
Classes :
```c#
public class ClasseTest
{
  ....
}
```
Structures :
```c#
public struct StructureTest
{
  ....
}
```
Records :
```c#
public record RecordTest(
  string TestUn,
  string TestDeux,
  ....);
```
Interfaces : 
```c#
public interface IInterfaceTest
{
  ....
}
```

Attributs publiques :
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

Attributs privés ou internes :
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

Attributs statiques :
```c#
public class ClasseTest
{
  private static int s_intTest;
  
  [ThreadStatic]
    private static bool t_boolTest;
}
```

Méthodes :
```c#
public class ClasseTest
{
  public int MethodeTest<int>(int intTest, bool boolTest)
  {
    ....
  }
}
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
