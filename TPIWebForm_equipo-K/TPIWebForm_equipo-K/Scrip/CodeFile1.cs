//CREATE PROCEDURE UsuariosAgregar
//    @Nombre VARCHAR(100),
//    @Apellido VARCHAR(100),
//    @Email VARCHAR(150),
//    @ClaveHash VARCHAR(255),
//    @Telefono VARCHAR(30),
//    @IdPerfil INT
//AS
//BEGIN
//    INSERT INTO USUARIOS (Nombre, Apellido, Email, ClaveHash, Telefono, IdPerfil, Activo) 
//    OUTPUT INSERTED.Id 
//    VALUES (@Nombre, @Apellido, @Email, @ClaveHash, @Telefono, @IdPerfil, 1);
//END