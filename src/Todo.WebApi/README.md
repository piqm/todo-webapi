# Todo WebApi

## Descripción

Este proyecto es una API RESTful para la gestión de tareas y usuarios, desarrollada en .NET 9 y C# 13. Permite la administración de tareas, asignación de roles a usuarios y control de estados y prioridades de tareas.

---

## Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- (Opcional) [Docker](https://www.docker.com/) para levantar la base de datos localmente

---

## Configuración y Ejecución Local

1. **Clona el repositorio:**
    git clone https://github.com/piqm/todo-webapi.git cd todo-webapi

2. **Configura la base de datos:**
   - Crea una base de datos PostgreSQL llamada `todo_db`.
   - Crea un usuario y otórgale permisos, o usa las credenciales por defecto.

3. **Configura la cadena de conexión:**
   - Edita el archivo `appsettings.Development.json` en `src/Todo.WebApi/`:
        ```
        "ConnectionStrings": {    
        //"Database": "Host=localhost;Database=todo;User Id=postgres;Password=postgres;"
                      
        //Si se ejecuta la aplicacion desde un contenedor  
        "Database": "Host=todo-webapi.postgres;Port=5432;Database=todo;User Id=postgres;Password=postgres;"
        }
       ```
4. **Aplica las migraciones:**
    ```  
    dotnet ef database update --project src/Todo.WebApi
    ```

5. **Ejecuta la API:**

    ```  
    dotnet run --project src/Todo.WebApi
    ```


6. **Accede a la documentación Swagger:**
   - Navega a `http://localhost:5000/swagger` (o el puerto configurado).

---

## Arquitectura del Sistema

- **Capa de Presentación:**  
  Endpoints HTTP expuestos mediante ASP.NET Core Minimal APIs.

- **Capa de Aplicación:**  
  Contiene los casos de uso (por ejemplo, asignación de roles, gestión de tareas).

- **Capa de Dominio:**  
  Entidades como `User`, `TodoTask`, `UserRole`, y enums como `TaskPriority` y `TaskStatus`.

- **Capa de Infraestructura:**  
  Acceso a datos mediante Entity Framework Core y PostgreSQL.

- **Configuración:**  
  Uso de `appsettings.json` para cadenas de conexión y parámetros de entorno.

---

## Decisiones Técnicas

- **.NET 9 y C# 13:**  
  Aprovecha las últimas características del lenguaje y mejoras de rendimiento.

- **Entity Framework Core:**  
  ORM para facilitar el acceso y migración de datos.

- **PostgreSQL:**  
  Base de datos relacional robusta y ampliamente soportada.

- **Swagger:**  
  Documentación automática de la API para facilitar pruebas y desarrollo.

- **Relaciones Opcionales:**  
  Las relaciones como `EmployeeId` y `ReviewerId` en `TodoTask` son opcionales (`Guid?`), permitiendo tareas sin asignación.

---

## Consideraciones Especiales

- **Manejo de Concurrencia:**  
  Se recomienda agregar tokens de concurrencia (`RowVersion`) para evitar conflictos en actualizaciones simultáneas.

- **Manejo de Errores:**  
  Las excepciones de base de datos y validaciones se manejan y retornan mensajes claros al cliente.

- **Extensibilidad:**  
  La arquitectura permite agregar nuevas entidades y funcionalidades fácilmente.

---

## Contacto

Para dudas o soporte, abre un issue en el repositorio o contacta a los mantenedores.

              