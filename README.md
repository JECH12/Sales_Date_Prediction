# -Sales_Date_Prediction

Informacion relevante:

La cadena de conexion para la base de datos, es una cadena simple para conexion en base de datos creada localmente, de ser necesario, cambiar la cadena de conexion con el tipo de autorizacion necesaria donde se creo la base de datos (usario, contraseña).

Se instalaron paquetes como EntityFrameworkCore, EntityFrameworkCore.Desing, EntityFrameworkCore.SqlServer, EntityFrameworkCore.InMemory y Xunit(para UnitTest).

Se uso database Firts teniendo en cuenta que se tenia la base de datos.

Se realizaron los SP en base de datos con algunas ligeras modificaciones sobre lo que se pedia en el test para una mejor presentacion de informacion en FrontEnd.

Se creo un ejecutor generico de SP para evitar repeticion de codigo.

Se crearon DTOs.

Se uso try y catch con la intencion de darle un poco de manejo a los errores.

Se creo una clase generica para darle un mejor orden a las respuestas HTTP(falto Message), con esto seria mas facil manejar las respuestas en el frontEnd.

Se uso inyeccion de dependencias y principios SOLID.

Proyecto realizado en .NET 8 Web API.


