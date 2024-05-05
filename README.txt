Hecho en Unity 2022.3.12f1

Código hecho para detectar code smells, y refactorear utilizando reglas SOLID y Clean Code.


Cosas que se mejoraron:

Clase MapBuilder:

Sacar el enum TerrainType a otro archivo que se llame TerrainType.cs

Sacar las configuraciones (public static) a una clase Configurations.cs


Clase GameController:

Método Update:

Se repite un monton de código!!!!
- en el update dejar solo los if, y adentro de cada caso hacer un método que se llame MoveToRight, MoveToLeft, etc etc en cada uno.

- En cada método MoveTo... Utilizar un método CheckIfIsValidPosition() dentro de los if. Este método puede tener un && entre 2 métodos internamente: CheckIfCellExists, CheckIfCellIsNotATree
- Hacer un método MoveCharacterToPosition() que haga el movimiento del jugador y chequee si ganó.

Método InitializeMap: Se puede llamar, alñ final, al mismo método MoveCharacterToPosition() que creé antes.


Extra:
Separación de clases en vista y comportamiento: dejar en el monobehaviopr, solo lo que esta en Start, Update y el mostrar que se ganó. Todo lo demás puede ir a una clase que no herede de MB.
